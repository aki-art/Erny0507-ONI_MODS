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
            var traits = gameObject.AddOrGet<Klei.AI.Traits>();
            traitsToAdd.ToList().ForEach(traits.Add);
        }
        private static void OnSpawnedFrom(GeneticTraitComponent component, object data)
        {
        var parent=(data as GameObject)
        if(parent.IsCritter())
        {
        }
        if(parent==machine){
        //rollnewtraits
        }
            (data as GameObject).GetComponent<GeneticTraitComponent>()?.TransferTo(component);
        }

        private static void OnLayEgg(GeneticTraitComponent component, object data)
        {
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
            
        }

        public void TransferTo(GeneticTraitComponent other)
        {
            if (other == null ) return;

            other.critterName = critterName;
            other.generation = generation;

            if (other.IsEgg())
            {
                other.generation += 1;
            }

            other.ApplyName();
        }

        public void ResetToPrefabName()
        {
           
        }
    }
}

