using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static CreatureCalorieMonitor;

namespace PuftMorphs
{
    class Patch
    {
        
        [HarmonyPatch(typeof(CreatureCalorieMonitor.Stomach))]
        [HarmonyPatch(nameof(CreatureCalorieMonitor.Stomach.Poop))]
        public static class PatchPoop
        {
            
            public static bool Prefix(Stomach __instance, ref GameObject ___owner,
                ref List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry> ___caloriesConsumed)
            {
              
               
                if (___owner.PrefabID() == CloudyPuftConfig.ID || ___owner.PrefabID() == CloudyPuftConfig.BABY_ID|| ___owner.PrefabID() == SmoggyPuftConfig.ID || ___owner.PrefabID() == SmoggyPuftConfig.BABY_ID)
                {
                    float deltaEmitTemperature = 0;
                    if (___owner.PrefabID() == CloudyPuftConfig.ID || ___owner.PrefabID() == CloudyPuftConfig.BABY_ID)
                    {
                        deltaEmitTemperature = -5f * 2;
                    }
                    if (___owner.PrefabID() == SmoggyPuftConfig.ID || ___owner.PrefabID() == SmoggyPuftConfig.BABY_ID)
                    {
                        deltaEmitTemperature = 5f * 8;
                    }
                    float num1 = 0.0f;
                    Tag tag = Tag.Invalid;
                    byte disease_idx = byte.MaxValue;
                    int num2 = 0;
                    bool flag = false;
                    for (int index = 0; index < ___caloriesConsumed.Count; ++index)
                    {
                        CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = ___caloriesConsumed[index];
                        if ((double)caloriesConsumedEntry.calories > 0.0)
                        {
                            Diet.Info dietInfo = __instance.diet.GetDietInfo(caloriesConsumedEntry.tag);
                            if (dietInfo != null && (!(tag != Tag.Invalid) || !(tag != dietInfo.producedElement)))
                            {
                                num1 += dietInfo.ConvertConsumptionMassToProducedMass(dietInfo.ConvertCaloriesToConsumptionMass(caloriesConsumedEntry.calories));
                                tag = dietInfo.producedElement;
                                disease_idx = dietInfo.diseaseIdx;
                                num2 = (int)((double)dietInfo.diseasePerKgProduced * (double)num1);
                                caloriesConsumedEntry.calories = 0.0f;
                                ___caloriesConsumed[index] = caloriesConsumedEntry;
                                flag = flag || dietInfo.produceSolidTile;
                            }
                        }
                    }
                    if ((double)num1 <= 0.0 || tag == Tag.Invalid)
                        return false;
                    Element element = ElementLoader.GetElement(tag);
                    Debug.Assert(element != null, (object)"TODO: implement non-element tag spawning");
                    int cell = Grid.PosToCell(___owner.transform.GetPosition());
                    float temperature = ___owner.GetComponent<PrimaryElement>().Temperature;
                    temperature = Mathf.Max(element.lowTemp + 5f, temperature + deltaEmitTemperature);
                    if (element.IsLiquid)
                        FallingWater.instance.AddParticle(cell, element.idx, num1, temperature, disease_idx, num2, true, false, false, false);
                    else if (element.IsGas)
                        SimMessages.AddRemoveSubstance(cell, (int)element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num1, temperature, disease_idx, num2, true, -1);
                    else if (flag)
                    {
                        int num3 = ___owner.GetComponent<Facing>().GetFrontCell();
                        if (!Grid.IsValidCell(num3))
                        {
                            Debug.LogWarningFormat("{0} attemping to Poop {1} on invalid cell {2} from cell {3}", (object)___owner, (object)element.name, (object)num3, (object)cell);
                            num3 = cell;
                        }
                        SimMessages.AddRemoveSubstance(num3, (int)element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num1, temperature, disease_idx, num2, true, -1);
                    }
                    else
                        element.substance.SpawnResource(Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore), num1, temperature, disease_idx, num2, false, false, false);
                    KPrefabID component = ___owner.GetComponent<KPrefabID>();
                    if (!Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(component.PrefabTag))
                        Game.Instance.savedInfo.creaturePoopAmount.Add(component.PrefabTag, 0.0f);
                    Dictionary<Tag, float> creaturePoopAmount;
                    Tag prefabTag;
                    (creaturePoopAmount = Game.Instance.savedInfo.creaturePoopAmount)[prefabTag = component.PrefabTag] = creaturePoopAmount[prefabTag] + num1;
                    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, element.name, ___owner.transform, 1.5f, false);
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
