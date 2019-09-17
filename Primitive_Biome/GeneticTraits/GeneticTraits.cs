﻿using Primitive_Biome.GeneticTraits.Traits;
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
        public static List<string> ChooseTraitsFromEggToEgg(GameObject inst)
        {
            Debug.Log("ChooseTraitsFromEggToEgg");
            var gtc = inst.AddOrGet<GeneticTraitComponent>();
            var result = new List<string>();
            var fromTraits = inst.GetComponent<AIGeneticTraits>();
            Debug.Log("All groups");
            var groups = traits.GroupBy(trait => trait.Group);
           DebugHelper.LogForEach(groups);
            if (fromTraits == null)
            {
                Debug.Log("No traits present");
            }
            else
            {
                Debug.Log("Traits presents");
                var traits_present = fromTraits.GetTraitIds();
                var traits_locked = new List<GeneticTraitBuilder>();
                foreach (String t in traits_present)
                {
                    var trait_locked = traits.Where(x => x.ID == t).FirstOrDefault();
                    traits_locked.Add(trait_locked);
                    result.Add(t);
                    Debug.Log(trait_locked);
                }
                var groups_locked = traits_locked.GroupBy(trait => trait.Group);
                Debug.Log("groups_locked");
                DebugHelper.LogForEach(groups_locked);
                groups = groups.Except(groups_locked);

            }
            Debug.Log("groups");
            DebugHelper.LogForEach(groups);
            Chances chances = new Chances();
            chances.addChance(0.5f);//1 positive trait
            chances.addChance(0.25f);//2 positives and 1 negative
            chances.addChance(0.25f);//None 
            var chance_result = chances.rollChance();
            chance_result = 0;//for testing purposes
            Debug.Log("chance_result " + chance_result);
            var number_positives = 0;
            var number_negatives = 0;
            switch (chance_result)
            {
                case 0:
                    number_positives = 1;
                    break;
                case 1:
                    number_positives = 2;
                    number_negatives = 1;
                    break;
                case 2:
                    break;

            }
            groups.OrderBy(x => Util.GaussianRandom());//is not ordering randomly must be fix
            groups = groups.ToList();
            for (int i = 0; i < number_positives && groups.Count()>0; i++)
            {
                var first = groups.First();
                var temp=ChooseTraitFromGroup(groups.First(),true,true);
                result.Add(temp);
                groups= groups.Where(u => u.Key != first.Key).ToList();
            }
            for (int i = 0; i < number_negatives && groups.Count() > 0; i++)
            {
                var first = groups.First();
                var temp = ChooseTraitFromGroup(groups.First(), true, false);
                result.Add(temp);
                groups = groups.Where(u => u.Key != first.Key).ToList();
            }
          /*  int numTraitsToChoose = UnityEngine.Random.Range(2, 4);
            Debug.Log("------------");
            Debug.Log(inst);
            Debug.Log(numTraitsToChoose);
            

            foreach (var group in groups)
            {
                if (group.Key.HasRequirements(inst))
                {
                    result.Add(ChooseTraitFrom(group));
                }
            }*/
            Debug.Log(result);
            DebugHelper.LogForEach(result);
            // If there are more traits than asked for we don't want to bias to the ones that were chosen first
            return result
              .Where(s => s != null)
              //.OrderBy(x => Util.GaussianRandom())
              //.Take(numTraitsToChoose)
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
        private static string ChooseTraitFromGroup(IGrouping<Group, GeneticTraitBuilder> group, bool specific = false, bool positive = true)
        {
            if (specific)
            {
                return Util.GetRandom(group.Where(x=>x.Positive==positive).ToList()).ID;
            }
            else
            {
                return Util.GetRandom(group.ToList()).ID;
            }
            
        
        }

        /**
         * Checks if the trait is explicitly supported by this mod.
         */
        public static bool IsSupportedTrait(string traitId) => traitLookup.Contains(traitId);
        public static bool IsSupportedTrait(Klei.AI.Trait trait) => IsSupportedTrait(trait.Id);
    }
}
