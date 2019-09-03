using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Database;
using Harmony;
using Klei.AI;
using UnityEngine;
using static CreatureCalorieMonitor;

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
                    SimHashes.Katairite.CreateTag(),
                    0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE));
            
            TUNING.CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.Add(
                Traverse.Create(typeof(TUNING.CREATURES.EGG_CHANCE_MODIFIERS)).Method("CreateDietaryModifier", new[] { typeof(string), typeof(Tag), typeof(Tag), typeof(float) })
                .GetValue<System.Action>(
                    FloralHatchConfig.Id,
                    FloralHatchConfig .EggId.ToTag(),
                    (Tag)"PrickleFlowerSeed",
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
                 if (eggId.Equals("HatchVegEgg"))
                {
                    egg_chances.Add(new FertilityMonitor.BreedingChance()
                    {
                        egg = FloralHatchConfig.EggId.ToTag(),
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
        [HarmonyPatch(typeof(CreatureCalorieMonitor.Stomach))]
        [HarmonyPatch(nameof(CreatureCalorieMonitor.Stomach.Poop))]
        public static class PatchPoop
        {

          /*  public static bool Prepare(HarmonyInstance instance)
            {
                Debug.Log("MyInitializer");
                return true;
            }*/
            public static bool Prefix(Stomach __instance, ref GameObject ___owner,
                ref List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry> ___caloriesConsumed)
            {
                // var diet = __instance.diet;
                //var caloriesConsumed = __caloriesConsumed;
                Debug.Log("About to patch poop");
                // Debug.Log(___owner);
                Debug.Log(___owner.name);
                if (___owner.PrefabID() == FloralHatchConfig.Id)
                {
                    float num = 0f;//consumed calories acumulated
                    Tag tag = Tag.Invalid;
                    byte disease_idx = byte.MaxValue;
                    int num2 = 0;//total germs
                    bool flag = false;
                    for (int i = 0; i < ___caloriesConsumed.Count; i++)
                    {
                        CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry value = ___caloriesConsumed[i];
                        if (value.calories > 0f)
                        {
                            Diet.Info dietInfo = __instance.diet.GetDietInfo(value.tag);
                            if (dietInfo != null)
                            {
                                if (!(tag != Tag.Invalid) || !(tag != dietInfo.producedElement))
                                {
                                    num += dietInfo.ConvertConsumptionMassToProducedMass(dietInfo.ConvertCaloriesToConsumptionMass(value.calories));
                                    tag = dietInfo.producedElement;
                                    disease_idx = dietInfo.diseaseIdx;
                                    num2 = (int)(dietInfo.diseasePerKgProduced * num);
                                    value.calories = 0f;
                                    ___caloriesConsumed[i] = value;
                                    flag = (flag || dietInfo.produceSolidTile);
                                }
                            }
                        }
                    }
                    if (num <= 0f || tag == Tag.Invalid)
                    {
                        Debug.Log("tag invalid");
                        return false;
                    }

                    Element element = ElementLoader.GetElement(tag);

                    if (element == null)
                    {
                        //for food and others
                        Debug.Log("About to start special route");
                        GameObject prefab = Assets.GetPrefab(tag);
                        GameObject gameObject2 = GameUtil.KInstantiate(prefab, Grid.SceneLayer.Ground, null, 0);
                        Debug.Log("1");
                        Debug.Log(prefab);
                        // EdiblesManager.FoodInfo food_info = EdiblesManager.GetFoodInfo(tag.ToString());
                        var out_put_edible = prefab.GetComponent<Edible>();

                        //Debug.Log(out_put_edible);
                        var out_put_food_info = out_put_edible.FoodInfo;
                        //Debug.Log(out_put_food_info);
                        // var kcal_per_unit = out_put_edible.CaloriesPerUnit;
                        int units_c = (int)(num / out_put_food_info.CaloriesPerUnit);
                        Facing component = ___owner.GetComponent<Facing>();
                        //int cell = component.GetFrontCell();
                        Debug.Log(num);
                        Debug.Log(out_put_food_info.CaloriesPerUnit);
                        Debug.Log(units_c);

                        
                        int num3 = Grid.PosToCell(___owner.transform.GetPosition());
                        var pos = Grid.CellToPosCCC(num3, Grid.SceneLayer.Ground);
                        gameObject2.transform.SetPosition(pos);

                        

                        PrimaryElement component2 = gameObject2.GetComponent<PrimaryElement>();
                        component2.Mass = num;
                        //component2.Units = units_c;
                        
                        float temperature = ___owner.GetComponent<PrimaryElement>().Temperature;
                        component2.Temperature = temperature;
                        
                        gameObject2.SetActive(true);
                        component2.AddDisease(disease_idx, num2, "ComplexFabricator.CompleteOrder");
                        Debug.Log("continue");

                        KPrefabID component3 = ___owner.GetComponent<KPrefabID>();
                        if (!Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(component3.PrefabTag))
                        {
                            Game.Instance.savedInfo.creaturePoopAmount.Add(component3.PrefabTag, 0f);
                        }
                        Dictionary<Tag, float> creaturePoopAmount;
                        Tag prefabTag;
                        (creaturePoopAmount = Game.Instance.savedInfo.creaturePoopAmount)[prefabTag = component3.PrefabTag] = creaturePoopAmount[prefabTag] + num;
                        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, prefab.name, ___owner.transform, 1.5f, false);
                    }
                    else
                    {
                        ///default option
                        global::Debug.Assert(element != null, "Fail at default option");
                        int num3 = Grid.PosToCell(___owner.transform.GetPosition());
                        float temperature = ___owner.GetComponent<PrimaryElement>().Temperature;
                        if (element.IsLiquid)
                        {
                            FallingWater.instance.AddParticle(num3, element.idx, num, temperature, disease_idx, num2, true, false, false, false);
                        }
                        else if (element.IsGas)
                        {
                            SimMessages.AddRemoveSubstance(num3, (int)element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num, temperature, disease_idx, num2, true, -1);
                        }
                        else if (flag)
                        {
                            Facing component = ___owner.GetComponent<Facing>();
                            int num4 = component.GetFrontCell();
                            if (!Grid.IsValidCell(num4))
                            {
                                global::Debug.LogWarningFormat("{0} attemping to Poop {1} on invalid cell {2} from cell {3}",
                                    new object[]{___owner,  element.name, num4, num3
                                });
                                num4 = num3;
                            }
                            SimMessages.AddRemoveSubstance(num4, (int)element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num, temperature, disease_idx, num2, true, -1);
                        }
                        else
                        {
                            element.substance.SpawnResource(Grid.CellToPosCCC(num3, Grid.SceneLayer.Ore), num, temperature, disease_idx, num2, false, false, false);
                        }
                        KPrefabID component2 = ___owner.GetComponent<KPrefabID>();
                        if (!Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(component2.PrefabTag))
                        {
                            Game.Instance.savedInfo.creaturePoopAmount.Add(component2.PrefabTag, 0f);
                        }
                        Dictionary<Tag, float> creaturePoopAmount;
                        Tag prefabTag;
                        (creaturePoopAmount = Game.Instance.savedInfo.creaturePoopAmount)[prefabTag = component2.PrefabTag] = creaturePoopAmount[prefabTag] + num;
                        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, element.name, ___owner.transform, 1.5f, false);

                    }
                    Debug.Log("finished pooping");
                    return true;
                }
                else
                {
                    return false;
                }
                // Debug.Log(___owner.PrefabID());
                //Debug.Log(___owner.GetComponent<Tag>());

            }
        }





    }
}
