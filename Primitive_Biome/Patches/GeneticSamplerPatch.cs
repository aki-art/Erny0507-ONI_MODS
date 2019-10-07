using Harmony;
using Klei;
using Primitive_Biome.Buildings;
using Primitive_Biome.GeneticTraits;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUNING;
using UnityEngine;

namespace Primitive_Biome.Patches
{
    class GeneticSamplerPatch
    {
        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch("Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                var tech = "ImprovedCombustion";
                var buildingId = GeneticSamplerConfig.ID;
                var techList = new List<string>(Database.Techs.TECH_GROUPING[tech]) { buildingId };
                Database.Techs.TECH_GROUPING[tech] = techList.ToArray();
                var category = "Refining";
                string addAfterBuildingId = null;
                ModUtil.AddBuildingToPlanScreen(category, buildingId);
               /* var index = TUNING.BUILDINGS.PLANORDER.FindIndex(x => x.category == category);
                if (index == -1)
                    return;
                var planOrderList = TUNING.BUILDINGS.PLANORDER[index].data as IList<string>;
                if (planOrderList == null)
                {
                    return;
                }
                var neighborIdx = planOrderList.IndexOf(addAfterBuildingId);
                if (neighborIdx != -1)
                    planOrderList.Insert(neighborIdx + 1, buildingId);
                else
                    planOrderList.Add(buildingId);*/
            }
        }
        [HarmonyPatch(typeof(EntityConfigManager))]
        [HarmonyPatch("LoadGeneratedEntities")]
        public static class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Prefix()
            {

                Strings.Add("STRINGS.BUILDINGS.PREFABS." + GeneticSamplerConfig.ID.ToUpperInvariant() + ".NAME", GeneticSamplerConfig.NAME);
                Strings.Add("STRINGS.BUILDINGS.PREFABS." + GeneticSamplerConfig.ID.ToUpperInvariant() + ".DESC", GeneticSamplerConfig.DESC);
                Strings.Add("STRINGS.BUILDINGS.PREFABS." + GeneticSamplerConfig.ID.ToUpperInvariant() + ".EFFECT", GeneticSamplerConfig.EFFECT);
                Strings.Add("STRINGS.BUILDINGS.PREFABS." + GeneticSamplerConfig.ID.ToUpperInvariant() + ".RECOMBINATION_RECIPE_DESCRIPTION", GeneticSamplerConfig.RECOMBINATION_RECIPE_DESCRIPTION);
                //TagManager.Create("FossilStone");

                Debug.Log("Strings Added!!!!!!!!!!!!!!");
            }

        }
        [HarmonyPatch(typeof(ComplexFabricator))]
        [HarmonyPatch("SpawnOrderProduct")]
        public static class ComplexFabricator_SpawnOrderProduct_Patch
        {
            public static bool Prefix(ComplexFabricator __instance, ref ComplexRecipe recipe, ref List<GameObject> __result)
            {
                if (recipe == GeneticSamplerConfig.RECIPE_RECOMBINATION)
                {
                    Debug.Log("Should recombinate");
                    var ingredient_0 = recipe.ingredients[0];
                    Debug.Log(ingredient_0);
                    float amount = ingredient_0.amount;
                    var tag = ingredient_0.material;
                    Storage storage = __instance.buildStorage;
                    Debug.Log(amount);
                    Debug.Log(tag);
                    Debug.Log(storage.items.Count);
                    DebugHelper.LogForEach(storage.items);
                    for (int index = 0; index < storage.items.Count && (double)amount > 0.0; ++index)
                    {
                        GameObject item_0 = storage.items[index];
                        Debug.Log(item_0);
                        Debug.Log(item_0.HasTag(tag));
                        if (!((UnityEngine.Object)item_0 == (UnityEngine.Object)null) && item_0.HasTag(tag))
                        {
                            Debug.Log("About to add traits to add");
                            
                            var traitsToAdd = GeneticTraits.GeneticTraits.ChooseTraitsFromEggToEgg(item_0).Select(Db.Get().traits.Get);
                            //to the result
                            



                            List<GameObject> gameObjectList = new List<GameObject>();
                            SimUtil.DiseaseInfo diseaseInfo;
                            diseaseInfo.count = 0;
                            diseaseInfo.idx = (byte)0;
                            float num1 = 0.0f;
                            float num2 = 0.0f;
                            foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
                                num2 += ingredient.amount;
                            foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
                            {
                                float num3 = ingredient.amount / num2;
                                SimUtil.DiseaseInfo disease_info;
                                float aggregate_temperature;
                                __instance.buildStorage.ConsumeAndGetDisease(ingredient.material, ingredient.amount, out disease_info, out aggregate_temperature);
                                if (disease_info.count > diseaseInfo.count)
                                    diseaseInfo = disease_info;
                                num1 += aggregate_temperature * num3;
                            }
                            foreach (ComplexRecipe.RecipeElement result in recipe.results)
                            {
                                GameObject first = __instance.buildStorage.FindFirst(result.material);
                                if ((UnityEngine.Object)first != (UnityEngine.Object)null)
                                {
                                    Edible component = first.GetComponent<Edible>();
                                    if ((bool)((UnityEngine.Object)component))
                                        ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component.Calories, StringFormatter.Replace((string)UI.ENDOFDAYREPORT.NOTES.CRAFTED_USED, "{0}", component.GetProperName()), (string)UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
                                }
                                switch (__instance.resultState)
                                {
                                    case ComplexFabricator.ResultState.PassTemperature:
                                    case ComplexFabricator.ResultState.Heated:
                                        GameObject go = GameUtil.KInstantiate(Assets.GetPrefab(result.material), Grid.SceneLayer.Ore, (string)null, 0);
                                        int cell = Grid.PosToCell((KMonoBehaviour)__instance);
                                        go.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore) + __instance.outputOffset);
                                        PrimaryElement component1 = go.GetComponent<PrimaryElement>();
                                        component1.Units = result.amount;
                                        component1.Temperature = __instance.resultState != ComplexFabricator.ResultState.PassTemperature ? __instance.heatedTemperature : num1;
                                        go.SetActive(true);
                                        float num3 = result.amount / recipe.TotalResultUnits();
                                        component1.AddDisease(diseaseInfo.idx, Mathf.RoundToInt((float)diseaseInfo.count * num3), "ComplexFabricator.CompleteOrder");
                                        go.GetComponent<KMonoBehaviour>().Trigger(748399584, (object)null);

                                        var gtc = go.AddOrGet<GeneticTraitComponent>();
                                        gtc.addTraits(traitsToAdd, item_0);

                                        gameObjectList.Add(go);
                                        if (__instance.storeProduced)
                                        {
                                            __instance.outStorage.Store(go, false, false, true, false);
                                            break;
                                        }
                                        break;
                                    case ComplexFabricator.ResultState.Melted:
                                        if (__instance.storeProduced)
                                        {
                                            float temperature = ElementLoader.GetElement(result.material).lowTemp + (float)(((double)ElementLoader.GetElement(result.material).highTemp - (double)ElementLoader.GetElement(result.material).lowTemp) / 2.0);
                                            __instance.outStorage.AddLiquid(ElementLoader.GetElementID(result.material), result.amount, temperature, (byte)0, 0, false, true);
                                            break;
                                        }
                                        break;
                                }
                                if (gameObjectList.Count > 0)
                                {
                                    SymbolOverrideController component2 = __instance.GetComponent<SymbolOverrideController>();
                                    if ((UnityEngine.Object)component2 != (UnityEngine.Object)null)
                                    {
                                        KAnim.Build build = gameObjectList[0].GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build;
                                        KAnim.Build.Symbol symbol = build.GetSymbol((KAnimHashedString)build.name);
                                        if (symbol != null)
                                        {
                                            component2.TryRemoveSymbolOverride((HashedString)"output_tracker", 0);
                                            component2.AddSymbolOverride((HashedString)"output_tracker", symbol, 0);
                                        }
                                        else
                                            Debug.LogWarning((object)(component2.name + " is missing symbol " + build.name));
                                    }
                                }
                            }
                            __result=gameObjectList;

                        }
                    }
                    
                    return false;
                }
                else
                {
                    Debug.Log("Should NO recombinate");
                    return true;
                }

            }

        }
    }
}
