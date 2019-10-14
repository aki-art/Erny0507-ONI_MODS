using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STRINGS;
using UnityEngine;
namespace PuftMorphs
{

    [SkipSaveFileSerialization]
    class CloudyBreath : StateMachineComponent<CloudyBreath.StatesInstance>, IGameObjectEffectDescriptor
    {


        public float deltaEmitTemperature = -5f;
        public Vector3 emitOffsetCell = new Vector3(0.0f, 0.0f);
        private List<GameObject> gases = new List<GameObject>();
        [MyCmpReq]
        private Storage storage;
        private const float EXHALE_PERIOD = 1f;
    
        private Tag lastEmitTag;
        private int nextGasEmitIndex;
        [SerializeField]
        private float minimun_tempeturare = 0;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            this.smi.StartSM();
        }

        protected override void OnPrefabInit()
        {

            base.OnPrefabInit();
        }

        public void Configuration(float minimun_tempeturare_)
        {
            minimun_tempeturare = minimun_tempeturare_;
        }
            
        protected override void OnCleanUp()
        {
            base.OnCleanUp();
        }

        protected void DestroySelf(object callbackParam)
        {
            CreatureHelpers.DeselectCreature(this.gameObject);
            Util.KDestroyGameObject(this.gameObject);
        }

        public List<Descriptor> GetDescriptors(GameObject go)
        {
            return new List<Descriptor>()
            {
              new Descriptor((string) UI.GAMEOBJECTEFFECTS.COLDBREATHER, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.COLDBREATHER, Descriptor.DescriptorType.Effect, false)
            };
        }

        private void Cool()
        {
            if (this.lastEmitTag != Tag.Invalid)
                return;
            this.gases.Clear();
            this.storage.Find(GameTags.Gas, this.gases);
            if (this.nextGasEmitIndex >= this.gases.Count)
                this.nextGasEmitIndex = 0;
            while (this.nextGasEmitIndex < this.gases.Count)
            {
                PrimaryElement component = this.gases[this.nextGasEmitIndex++].GetComponent<PrimaryElement>();
                if ((UnityEngine.Object)component != (UnityEngine.Object)null && (double)component.Mass > 0.0)
                {
                    float temperature = Mathf.Max(component.Element.lowTemp + 5f, component.Temperature + this.deltaEmitTemperature);
                    component.Temperature = temperature;
                    this.lastEmitTag = component.Element.tag;
                    break;
                }
            }
        }

        private bool CheckCanCool()
        {
            if (this.lastEmitTag != Tag.Invalid)
                return false;
            this.gases.Clear();
            this.storage.Find(GameTags.Gas, this.gases);
            if (this.nextGasEmitIndex >= this.gases.Count)
                this.nextGasEmitIndex = 0;
            while (this.nextGasEmitIndex < this.gases.Count)
            {
                PrimaryElement component = this.gases[this.nextGasEmitIndex++].GetComponent<PrimaryElement>();
                if ((UnityEngine.Object)component != (UnityEngine.Object)null && (double)component.Mass > 0.0)
                {
                    if (component.Temperature>0)
                    {
                        return true;

                    }
                    
                }
            }

            return false;
        }
        public class StatesInstance : GameStateMachine<CloudyBreath.States, CloudyBreath.StatesInstance, CloudyBreath, object>.GameInstance
        {
            public StatesInstance(CloudyBreath master)
              : base(master)
            {
            }
        }

        public class States : GameStateMachine<CloudyBreath.States, CloudyBreath.StatesInstance, CloudyBreath>
        {
            private StatusItem statusItemCooling;

            public GameStateMachine<CloudyBreath.States, CloudyBreath.StatesInstance, CloudyBreath, object>.State waiting;
            public GameStateMachine<CloudyBreath.States, CloudyBreath.StatesInstance, CloudyBreath, object>.State cooling;
            public override void InitializeStates(out StateMachine.BaseState default_state)
            {
               this.serializable = true;
                default_state = (StateMachine.BaseState)this.waiting;
                this.statusItemCooling = new StatusItem("cooling", (string)CREATURES.STATUSITEMS.COOLING.NAME, (string)CREATURES.STATUSITEMS.COOLING.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);

                this.waiting
                    //.Enter("Waiting", (StateMachine<CloudyBreath.States, CloudyBreath.StatesInstance, CloudyBreath, object>.State.Callback)
                    //(smi => smi.master.operational.SetActive(false, false)))
                    .EventTransition(GameHashes.OnStorageChange, this.cooling,
                    (StateMachine<CloudyBreath.States, CloudyBreath.StatesInstance, CloudyBreath, object>.Transition.ConditionCallback)
                    (smi => smi.master.CheckCanCool()));
                this.cooling
                    //.Enter("Cooling", (StateMachine<CloudyBreath.States, CloudyBreath.StatesInstance, CloudyBreath, object>.State.Callback)
                    //(smi => smi.master.operational.SetActive(true, false)))
                    .Transition(this.waiting, (StateMachine<CloudyBreath.States, CloudyBreath.StatesInstance, CloudyBreath, object>.Transition.ConditionCallback)
                    (smi => !smi.master.CheckCanCool()), UpdateRate.SIM_200ms)
                    .EventHandler(GameHashes.OnStorageChange, (StateMachine<CloudyBreath.States, CloudyBreath.StatesInstance, CloudyBreath, object>.State.Callback)
                    (smi => smi.master.Cool()));
            }



        }
    }
}
