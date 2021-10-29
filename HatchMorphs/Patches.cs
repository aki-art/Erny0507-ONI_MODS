using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Database;
using HarmonyLib;
using Klei.AI;
using TUNING;
using UnityEngine;
using static CreatureCalorieMonitor;

namespace HatchMorphs
{
    class Patches:KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            harmony.PatchAll();

            TUNING.CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.Add(
                Traverse.Create(typeof(TUNING.CREATURES.EGG_CHANCE_MODIFIERS)).Method("CreateDietaryModifier", new[] { typeof(string), typeof(Tag), typeof(Tag), typeof(float) })
                .GetValue<System.Action>(
                    DiamondHatchConfig.Id,
                    DiamondHatchConfig.EggId.ToTag(),
                    SimHashes.Diamond.CreateTag(),
                    0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE));
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
                    FloralHatchConfig.EggId.ToTag(),
                    (Tag)PrickleFruitConfig.ID,
                    0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE));
            TUNING.CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.Add(
                Traverse.Create(typeof(TUNING.CREATURES.EGG_CHANCE_MODIFIERS)).Method("CreateDietaryModifier", new[] { typeof(string), typeof(Tag), typeof(Tag), typeof(float) })
                .GetValue<System.Action>(
                    FloralHatchConfig.Id,
                    FloralHatchConfig.EggId.ToTag(),
                    (Tag)PrickleFlowerConfig.SEED_ID,
                    0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE));
            TUNING.CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.Add(
                Traverse.Create(typeof(TUNING.CREATURES.EGG_CHANCE_MODIFIERS)).Method("CreateDietaryModifier", new[] { typeof(string), typeof(Tag), typeof(Tag), typeof(float) })
                .GetValue<System.Action>(
                    WoodenHatchConfig.Id,
                    WoodenHatchConfig.EggId.ToTag(),
                 (Tag)TUNING.FOOD.FOOD_TYPES.BASICPLANTFOOD.Id,
                    0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE));
            TUNING.CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.Add(
                Traverse.Create(typeof(TUNING.CREATURES.EGG_CHANCE_MODIFIERS)).Method("CreateDietaryModifier", new[] { typeof(string), typeof(Tag), typeof(Tag), typeof(float) })
                .GetValue<System.Action>(
                    WoodenHatchConfig.Id,
                    WoodenHatchConfig.EggId.ToTag(),
                 WoodLogConfig.TAG,
                    0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE));

            var t = GERM_EXPOSURE.TYPES[4];
            t.excluded_effects = new List<string>
            {
                         "HistamineSuppression",FloralAntihistamineConfig.Effect_
            };
            /*List<ExposureType> types = new List<ExposureType>();

            types.AddRange(GERM_EXPOSURE.TYPES);
            ExposureType t = new ExposureType
            {
                germ_id = SweetPollenGerms.ID,
                sickness_id = "Allergies",
                exposure_threshold = 2,
                infect_immediately = true,
                required_traits = new List<string>
                    {
                        "Allergies"
                    },
                excluded_effects = new List<string>
                    {
                        "HistamineSuppression",FloralAntihistamineConfig.Effect_
                    }
            };
            ExposureType t2 = new ExposureType
            {
                germ_id = SweetPollenGerms.ID,
                infection_effect = "SmelledFlowersLonger",
                exposure_threshold = 2,
                infect_immediately = true,
                excluded_traits = new List<string>
                    {
                        "Allergies"
                    }
            };
            types.Add(t);
            types.Add(t2);
            GERM_EXPOSURE.TYPES = types.ToArray();*/

            //TUNING.MEDICINE.
            /*  MedicineInfo ANTIHISTAMINE = new MedicineInfo("antihistamine", "HistamineSuppression", MedicineInfo.MedicineType.CureSpecific, new string[1]
      {
        "Allergies"
      });*/

        }

        [HarmonyPatch(typeof(EntityTemplates), nameof(EntityTemplates.ExtendEntityToFertileCreature))]
        public class EntityTemplates_ExtendEntityToFertileCreature_Patch
        {
            private static void Prefix(string eggId, List<FertilityMonitor.BreedingChance> egg_chances)
            {
                if (eggId.Equals(HatchHardConfig.EGG_ID))
                {
                    egg_chances.Add(new FertilityMonitor.BreedingChance()
                    {
                        egg = DiamondHatchConfig.EggId.ToTag(),
                        weight = 0.02f
                    });
                }
                if (eggId.Equals(HatchVeggieConfig.EGG_ID))
                {
                    egg_chances.Add(new FertilityMonitor.BreedingChance()
                    {
                        egg = FloralHatchConfig.EggId.ToTag(),
                        weight = 0.02f
                    });
                    egg_chances.Add(new FertilityMonitor.BreedingChance()
                    {
                        egg = WoodenHatchConfig.EggId.ToTag(),
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
              
                Strings.Add("STRINGS.ITEMS.FOOD." + FilamentsConfig.Id.ToUpper() + ".NAME", FilamentsConfig.Name);
                Strings.Add("STRINGS.ITEMS.FOOD." + FilamentsConfig.Id.ToUpper() + ".DESC", FilamentsConfig.Description);

                Strings.Add("STRINGS.ITEMS.FOOD." + MacedoniaConfig.Id.ToUpper() + ".NAME", MacedoniaConfig.Name);
                Strings.Add("STRINGS.ITEMS.FOOD." + MacedoniaConfig.Id.ToUpper() + ".DESC", MacedoniaConfig.Description);

                Strings.Add("STRINGS.ITEMS.FOOD." + NectarConfig.Id.ToUpper() + ".NAME", NectarConfig.Name);
                Strings.Add("STRINGS.ITEMS.FOOD." + NectarConfig.Id.ToUpper() + ".DESC", NectarConfig.Description);

      /*          Strings.Add("STRINGS.DUPLICANTS.DISEASES." + SweetPollenGerms.ID.ToUpper() + ".NAME", SweetPollenGerms.Name);
                Strings.Add("STRINGS.DUPLICANTS.DISEASES." + SweetPollenGerms.ID.ToUpper() + ".LEGEND_HOVERTEXT", SweetPollenGerms.Tooltip);*/

                Strings.Add("STRINGS.ITEMS.PILLS." + FloralAntihistamineConfig.ID.ToUpper() + ".NAME", FloralAntihistamineConfig.Name);
                Strings.Add("STRINGS.ITEMS.PILLS." + FloralAntihistamineConfig.ID.ToUpper() + ".DESC", FloralAntihistamineConfig.Description);
                Strings.Add("STRINGS.ITEMS.PILLS." + FloralAntihistamineConfig.ID.ToUpper() + ".RECIPEDESC", FloralAntihistamineConfig.Description);

                Strings.Add("STRINGS.ITEMS.PILLS." + MendingSerumConfig.ID.ToUpper() + ".NAME", MendingSerumConfig.Name);
                Strings.Add("STRINGS.ITEMS.PILLS." + MendingSerumConfig.ID.ToUpper() + ".DESC", MendingSerumConfig.Description);
                Strings.Add("STRINGS.ITEMS.PILLS." + MendingSerumConfig.ID.ToUpper() + ".RECIPEDESC", MendingSerumConfig.Description);


                //Db.Get().Diseases.Add(new SweetPollenGerms());
                //var effect1 = new Effect("SmelledFlowersLonger", "Smelled sweet scents", "This dupe has smelled sweet scents and feels relaxed", 600.00f * 2, true, true, false, (string)null, 0.0f, (string)null);
                //effect1.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.008333334f * 2, "The sweet smell relieves some stress", false, false, true));
                var effect2 = new Effect(FloralAntihistamineConfig.Effect_, "Floral Histamine Suppression", "Helps with allergies", 600.00f * 10, true, true, false, (string)null, 0.0f, (string)null);
                //effect2.Add(new AttributeModifier(Db.Get().Amounts.ImmuneLevel.deltaAttribute.Id, +0.008333334f * 10, "Inmunity level is rising", false, false, true));
                var effect3 = new Effect(MendingSerumConfig.Effect_, "Regeneration", "This dupe is constantly healing", 600.00f * 5, true, true, false, (string)null, 0.0f, (string)null);
                effect3.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.deltaAttribute.Id, +0.008333334f, "Healing", false, false, true));

                //Db.Get().effects.Add(effect1);
                Db.Get().effects.Add(effect2);
                Db.Get().effects.Add(effect3);
                

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


        [HarmonyPatch(typeof(CreatureCalorieMonitor.Stomach))]
        [HarmonyPatch(nameof(CreatureCalorieMonitor.Stomach.Poop))]
        public static class PatchPoop
        {

            public static bool Prefix(Stomach __instance, ref GameObject ___owner,
                ref List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry> ___caloriesConsumed)
            {
                            
                if (___owner.PrefabID() == FloralHatchConfig.Id || ___owner.PrefabID() == FloralHatchConfig.BabyId || ___owner.PrefabID() == WoodenHatchConfig.Id || ___owner.PrefabID() == WoodenHatchConfig.BabyId)
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
                        GameObject prefab = Assets.GetPrefab(tag);
                        GameObject gameObject2 = GameUtil.KInstantiate(prefab, Grid.SceneLayer.Ore, null, 0);
                      
                        int units_c = 0;
                        PrimaryElement component2 = null;
                        if (___owner.PrefabID() == WoodenHatchConfig.Id || ___owner.PrefabID() == WoodenHatchConfig.BabyId)
                        {
                            // EdiblesManager.FoodInfo food_info = EdiblesManager.GetFoodInfo(tag.ToString());
                            var out_put = prefab.GetComponent<PrimaryElement>();

                        
                            units_c = (int)(num / out_put.Mass);
                            Facing component = ___owner.GetComponent<Facing>();
                            
                            int num3 = Grid.PosToCell(___owner.transform.GetPosition());
                            var pos = Grid.CellToPosCCC(num3, Grid.SceneLayer.Ore);
                            gameObject2.transform.SetPosition(pos);
                            component2 = gameObject2.GetComponent<PrimaryElement>();
                            component2.Units = units_c;

                        }
                        else
                        {
                            var out_put_edible = prefab.GetComponent<Edible>();

                            var out_put_food_info = out_put_edible.FoodInfo;
                          
                            units_c = (int)(num / out_put_food_info.CaloriesPerUnit);
                            Facing component = ___owner.GetComponent<Facing>();
                           
                            int num3 = Grid.PosToCell(___owner.transform.GetPosition());
                            var pos = Grid.CellToPosCCC(num3, Grid.SceneLayer.Ore);
                            gameObject2.transform.SetPosition(pos);
                            component2 = gameObject2.GetComponent<PrimaryElement>();
                            component2.Mass = num;
                        }

                        float temperature = ___owner.GetComponent<PrimaryElement>().Temperature;
                        component2.Temperature = temperature;

                        gameObject2.SetActive(true);
                        component2.AddDisease(disease_idx, num2, "ComplexFabricator.CompleteOrder");

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
                    return false;
                }
                else
                {
                    return true;
                }
              
            }
        }

      
    }
}
