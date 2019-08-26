using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Harmony;
using UnityEngine;

namespace HatchMorphs
{
    class Patches
    {
        public static void OnLoad()
        {
            TUNING.CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.Add(
                Traverse.Create(typeof(TUNING.CREATURES.EGG_CHANCE_MODIFIERS)).Method("CreateDietaryModifier", new[] { typeof(string), typeof(Tag), typeof(Tag), typeof(float) })
                .GetValue<System.Action>(
                    DiamondHatchConfig.Id,
                    DiamondHatchConfig.EggId.ToTag(),
                    SimHashes.Diamond.CreateTag(),
                    0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE));
        }

        [HarmonyPatch(typeof(EntityTemplates), nameof(EntityTemplates.ExtendEntityToFertileCreature))]
        public class EntityTemplates_ExtendEntityToFertileCreature_Patch
        {
            private static void Prefix(string eggId, List<FertilityMonitor.BreedingChance> egg_chances)
            {
                if (eggId.Equals("HatchHardEgg"))
                {
                    egg_chances.Add(new FertilityMonitor.BreedingChance()
                    {
                        egg = DiamondHatchConfig.EggId.ToTag(),
                        weight = 0.02f
                    });
                }
            }
        }
        [HarmonyPatch(typeof(BaseHatchConfig))]
        [HarmonyPatch( nameof(BaseHatchConfig.HardRockDiet))]
        
        public class HardHatchModifiedDiet
        {
            public static void Postfix(List<Diet.Info> __result, Tag poopTag,
        float caloriesPerKg,
        float producedConversionRate,
        string diseaseId,
        float diseasePerKgProduced)
            {
                HashSet<Tag> hashSet = new HashSet<Tag>();
                hashSet.Add(SimHashes.Diamond.CreateTag());
                __result.Add(new Diet.Info(hashSet, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false));
            }
        }
    }
}
