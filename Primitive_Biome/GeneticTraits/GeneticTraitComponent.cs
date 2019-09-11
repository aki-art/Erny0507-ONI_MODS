using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei.AI;
using KSerialization;
using UnityEngine;

namespace Primitive_Biome.GeneticTraits
{

    public class GeneticTraitComponent : KMonoBehaviour, ISaveLoadable
    {
        [SerializeField]
        [Serialize]
        private bool appliedCritterTraits = false;
        private static readonly EventSystem.IntraObjectHandler<GeneticTraitComponent> OnSpawnedFromDelegate =
              new EventSystem.IntraObjectHandler<GeneticTraitComponent>(OnSpawnedFrom);

        private static readonly EventSystem.IntraObjectHandler<GeneticTraitComponent> OnLayEggDelegate =
          new EventSystem.IntraObjectHandler<GeneticTraitComponent>(OnLayEgg);

        protected override void OnPrefabInit()
        {
            GeneticTraits.InitAllTraits();
            gameObject.Subscribe((int)GameHashes.SpawnedFrom, from => transferTraits(from as GameObject));
            Subscribe((int)GameHashes.SpawnedFrom, OnSpawnedFromDelegate);
            Subscribe((int)GameHashes.LayEgg, OnLayEggDelegate);
        }

        protected override void OnSpawn()
        {
            if (!appliedCritterTraits)
            {
                var traitsToAdd = GeneticTraits.ChooseTraits(gameObject).Select(Db.Get().traits.Get);
                addTraits(traitsToAdd);

                appliedCritterTraits = true;
            }

        }

        // Transfer critter traits owned by the `from` object to this object
        private void transferTraits(GameObject from)
        {
            var fromTraits = from.GetComponent<Klei.AI.Traits>();
            if (fromTraits == null) return;

            var traitsToAdd = fromTraits.TraitList.Where(GeneticTraits.IsSupportedTrait);
            addTraits(traitsToAdd);

            appliedCritterTraits = true;
        }

        // Adds the provided list of traits to this object's Traits component
        private void addTraits(IEnumerable<Trait> traitsToAdd)
        {
            Debug.Log("addTraits");
            var traits = gameObject.AddOrGet<Klei.AI.Traits>();
            Debug.Log("traits to add");
            foreach (Klei.AI.Trait trait in traitsToAdd.ToList())
            {
                Debug.Log(trait.Name);
            }
            Debug.Log("traits");
            Debug.Log(traits);
            Debug.Log(traits.TraitList);
            foreach (Klei.AI.Trait trait in traits.TraitList)
            {
                Debug.Log(trait.Name);
            }
    
            traitsToAdd.ToList().ForEach(traits.Add);
            Debug.Log("finish adding");
        }
        //
        private static void OnSpawnedFrom(GeneticTraitComponent componentChild, object data)
        {
            var parent = (data as GameObject);
            Debug.Log("OnSpawnedFrom");
            Debug.Log(parent);
            /*if(parent.IsCritter())
            {
                    //heredar
                }
            if(parent==machine){
            //rollnewtraits
            }*/
            (data as GameObject).GetComponent<GeneticTraitComponent>()?.TransferTo(componentChild);
        }

        private static void OnLayEgg(GeneticTraitComponent component, object data)
        {
            Debug.Log("OnLayEgg");
            var egg = (data as GameObject);
            Debug.Log(egg);
            //pass traits
            Debug.Log(egg.GetComponent<GeneticTraitComponent>());
            component.TransferTo((data as GameObject).GetComponent<GeneticTraitComponent>());
        }

        private bool IsCritter()
        {
            return this.HasTag(GameTags.Creature);
        }

        private bool IsEgg()
        {
            return this.HasTag(GameTags.Egg);
        }
        public void SetName(string newName)
        {

        }

        private void setGameObjectName(string newName)
        {

        }

        public void ApplyName()
        {

        }

        public bool HasName()
        {
            return false;
        }

        public void TransferTo(GeneticTraitComponent componentChild)
        {
            if (componentChild == null) return;

            var fromTraits = GetComponent<Klei.AI.Traits>();
            if (fromTraits == null) return;

            var traitsToAdd = fromTraits.TraitList.Where(GeneticTraits.IsSupportedTrait);
            componentChild.addTraits(traitsToAdd);
        }

        public void ResetToPrefabName()
        {

        }
    }
}

