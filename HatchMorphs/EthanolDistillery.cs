using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

#pragma warning disable 649
//[SerializationConfig(MemberSerialization.OptIn)]
namespace HatchMorphs
{
    class EthanolDistillery : StateMachineComponent<EthanolDistillery.StatesInstance>
    {
        [SerializeField]
        public Tag emitTag;
        [SerializeField]
        public float emitMass;
        [SerializeField]
        public Vector3 emitOffset;
        [MyCmpAdd]
        private Storage storage;
        [MyCmpGet]
        private ElementConverter emitter;
        [MyCmpReq]
        private Operational operational;
        private ElementConverter[] converters;
        protected override void OnSpawn()
        {
            this.smi.StartSM();
        }
        private bool CheckCanConvert()
        {
            if (this.converters == null)
                this.converters = this.GetComponents<ElementConverter>();
            for (int index = 0; index < this.converters.Length; ++index)
            {
                if (this.converters[index].CanConvertAtAll())
                    return true;
            }
            return false;
        }

        private bool CheckEnoughMassToConvert()
        {
            if (this.converters == null)
                this.converters = this.GetComponents<ElementConverter>();
            for (int index = 0; index < this.converters.Length; ++index)
            {
                if (this.converters[index].HasEnoughMassToStartConverting())
                    return true;
            }
            return false;
        }

        public class StatesInstance : GameStateMachine<EthanolDistillery.States, EthanolDistillery.StatesInstance, EthanolDistillery, object>.GameInstance
        {
            public StatesInstance(EthanolDistillery smi)
              : base(smi)
            {
            }

            public void TryEmit()
            {
                Storage storage = this.smi.master.storage;
                GameObject first = storage.FindFirst(this.smi.master.emitTag);
                if (!((Object)first != (Object)null) || (double)first.GetComponent<PrimaryElement>().Mass < (double)this.master.emitMass)
                    return;
                storage.Drop(first, true).transform.SetPosition(this.transform.GetPosition() + this.master.emitOffset);
            }
        }

        public class States : GameStateMachine<EthanolDistillery.States, EthanolDistillery.StatesInstance, EthanolDistillery>
        {
            public GameStateMachine<EthanolDistillery.States, EthanolDistillery.StatesInstance, EthanolDistillery, object>.State disabled;
            public GameStateMachine<EthanolDistillery.States, EthanolDistillery.StatesInstance, EthanolDistillery, object>.State waiting;
            public GameStateMachine<EthanolDistillery.States, EthanolDistillery.StatesInstance, EthanolDistillery, object>.State converting;
            public GameStateMachine<EthanolDistillery.States, EthanolDistillery.StatesInstance, EthanolDistillery, object>.State overpressure;

            public override void InitializeStates(out StateMachine.BaseState default_state)
            {
                default_state = (StateMachine.BaseState)this.disabled;
                this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (StateMachine<EthanolDistillery.States, EthanolDistillery.StatesInstance, EthanolDistillery, object>.Transition.ConditionCallback)(smi => !smi.master.operational.IsOperational));
                this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (StateMachine<EthanolDistillery.States, EthanolDistillery.StatesInstance, EthanolDistillery, object>.Transition.ConditionCallback)(smi => smi.master.operational.IsOperational));
                this.waiting.Enter("Waiting", (StateMachine<EthanolDistillery.States, EthanolDistillery.StatesInstance, EthanolDistillery, object>.State.Callback)
                    (smi => smi.master.operational.SetActive(false, false))).EventTransition(GameHashes.OnStorageChange, this.converting, 
                    (StateMachine<EthanolDistillery.States, EthanolDistillery.StatesInstance, EthanolDistillery, object>.Transition.ConditionCallback)
                    (smi => smi.master.CheckEnoughMassToConvert()));
                this.converting.Enter("Ready", (StateMachine<EthanolDistillery.States, EthanolDistillery.StatesInstance, EthanolDistillery, object>.State.Callback)
                    (smi => smi.master.operational.SetActive(true, false)))
                    .Transition(this.waiting, (StateMachine<EthanolDistillery.States, EthanolDistillery.StatesInstance, EthanolDistillery, object>.Transition.ConditionCallback)
                    (smi => !smi.master.CheckCanConvert()), UpdateRate.SIM_200ms)
                    .EventHandler(GameHashes.OnStorageChange, (StateMachine<EthanolDistillery.States, EthanolDistillery.StatesInstance, EthanolDistillery, object>.State.Callback)
                    (smi => smi.TryEmit()));
            }
        }
    }
}