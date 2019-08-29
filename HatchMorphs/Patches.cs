using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Harmony;
using Klei.AI;
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
        [HarmonyPatch(nameof(BaseHatchConfig.HardRockDiet))]

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
        [HarmonyPatch(typeof(EntityConfigManager))]
        [HarmonyPatch("LoadGeneratedEntities")]
        public static class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Prefix()
            {
                /*Debug.Log("Adding strings");
                Strings.Add("STRINGS.DUPLICANTS.DISEASES." + SweetPollenGerms.ID.ToUpper() + ".NAME", "Sweet floral scents");
                Strings.Add("STRINGS.DUPLICANTS.DISEASES." + SweetPollenGerms.ID.ToUpper() + ".LEGEND_HOVERTEXT", "Sweet floral scents description");
                Debug.Log("Strings added");*/
                Strings.Add("STRINGS.ITEMS.FOOD." + FilamentsConfig.Id.ToUpper() + ".NAME", FilamentsConfig.Name);
                Strings.Add("STRINGS.ITEMS.FOOD." + FilamentsConfig.Id.ToUpper() + ".DESC", FilamentsConfig.Description);

                Strings.Add("STRINGS.ITEMS.FOOD." + MacedoniaConfig.Id.ToUpper() + ".NAME", MacedoniaConfig.Name);
                Strings.Add("STRINGS.ITEMS.FOOD." + MacedoniaConfig.Id.ToUpper() + ".DESC", MacedoniaConfig.Description);

                Strings.Add("STRINGS.ITEMS.FOOD." + NectarConfig.Id.ToUpper() + ".NAME", NectarConfig.Name);
                Strings.Add("STRINGS.ITEMS.FOOD." + NectarConfig.Id.ToUpper() + ".DESC", NectarConfig.Description);
            }
        }
        /*
        [HarmonyPatch(typeof(Diseases))]
        [HarmonyPatch("Diseases", MethodType.Setter)]
        public static class PatchDiseases
        {
            public static Diseases Postfix(Diseases __base)
            {
                __base.SweetPollenGerms = __base.Add(new SweetPollenGerms());
                __base.PollenGerms = __base.Add(new SweetPollenGerms());
                return __base;
            }

        }*/
        /*[HarmonyPatch(typeof(WoodGasGeneratorConfig))]
        [HarmonyPatch(nameof(WoodGasGeneratorConfig.DoPostConfigureComplete))]
        public static class PatchWoodGasGeneratorConfig
        {
            public static void Postfix(ref GameObject go)
            {
                var storage = go.GetComponent<Storage>();
                ManualDeliveryKG manualDeliveryKG2 = go.AddComponent<ManualDeliveryKG>();
                manualDeliveryKG2.SetStorage(storage);
                manualDeliveryKG2.requestedItemTag = BarkSkinConfig.TAG;
                manualDeliveryKG2.capacity = 360f;
                manualDeliveryKG2.refillMass = 180f;
                manualDeliveryKG2.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
                float max_stored_input_mass = 720f;
                 EnergyGenerator energyGenerator2 = go.AddComponent<EnergyGenerator>();
               energyGenerator2.powerDistributionOrder = 8;
               energyGenerator2.hasMeter = true;
               energyGenerator2.formula = EnergyGenerator.CreateSimpleFormula(BarkSkinConfig.TAG, 1.2f, max_stored_input_mass, SimHashes.CarbonDioxide, 0.17f, false, new CellOffset(0, 1), 383.15f);
            }
        }*/

    }
}
