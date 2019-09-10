using Primitive_Biome.GeneticTraits.Traits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Primitive_Biome.GeneticTraits
{
    class GeneticTraits
    {
        private static readonly GeneticTraitBuilder[] traits = {
      new LongLived(),
      new Fast(),
      new Fertile(),
      new Infertile(),
      new Slow(),
      new ShortLived()
    };

        private static readonly ILookup<string, GeneticTraitBuilder> traitLookup = traits.ToLookup(trait => trait.ID);

        private static bool traitsInitialized = false;

        /**
         * Initializes and registers all traits for use by the game if it has not already been done.
         */
        public static void InitAllTraits()
        {
            if (traitsInitialized == true) return;

            Array.ForEach(traits, trait => trait.CreateTrait());
            traitsInitialized = true;
        }

        /**
         * Chooses a random set of traits between 0 and 4. (max is capped randomly between 2 and 4)
         * Only applied traits that are relevant, i.e. Glowing trait on Shine Bug doesn't make sense.
         */
        public static List<string> ChooseTraits(GameObject inst)
        {
            var result = new List<string>();
            int numTraitsToChoose = UnityEngine.Random.Range(2, 4);
            Debug.Log("------------");
            Debug.Log(inst);
            Debug.Log(numTraitsToChoose);
            var groups = traits.GroupBy(trait => trait.Group);
            foreach (var group in groups)
            {
                if (group.Key.HasRequirements(inst))
                {
                    result.Add(ChooseTraitFrom(group));
                }
            }
            Debug.Log(result);

            // If there are more traits than asked for we don't want to bias to the ones that were chosen first
            return result
              .Where(s => s != null)
              .OrderBy(x => Util.GaussianRandom())
              .Take(numTraitsToChoose)
              .ToList();
        }

        /**
         * Chooses a trait from the given list with a probability. If the probability check fails it returns null.
         */
        private static string ChooseTraitFrom(IGrouping<Group, GeneticTraitBuilder> group)
        {
            float prob = UnityEngine.Random.Range(0f, 1f);
            if (prob <= group.Key.Probability)
            {
                Debug.Log("return something");
                return Util.GetRandom(group.ToList()).ID;
            }
            Debug.Log("return null");
            return null;
        }

        /**
         * Checks if the trait is explicitly supported by this mod.
         */
        public static bool IsSupportedTrait(string traitId) => traitLookup.Contains(traitId);
        public static bool IsSupportedTrait(Klei.AI.Trait trait) => IsSupportedTrait(trait.Id);
    }
}
