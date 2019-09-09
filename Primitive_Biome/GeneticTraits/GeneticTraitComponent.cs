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

        protected override void OnPrefabInit()
        {
            GeneticTraits.InitAllTraits();
            gameObject.Subscribe((int)GameHashes.SpawnedFrom, from => transferTraits(from as GameObject));
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
    }
}

