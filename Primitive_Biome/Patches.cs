using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei;
using Primitive_Biome.Elements;
using System.Collections;
using UnityEngine;
using Klei.AI;
namespace Primitive_Biome
{
    public class Patches
    {
        [HarmonyPatch(typeof(ElementLoader), "CollectElementsFromYAML")]
        class ElementLoader_CollectElementsFromYAML
        {
            static void Postfix(ref List<ElementLoader.ElementEntry> __result)
            {
                var containsIron = $"Contains traces of {STRINGS.UI.FormatAsLink("cyanide", "IRON")}.";//to add cyanide later
                Strings.Add("STRINGS.ELEMENTS." + ToxicWaterElement.ID.ToUpper() + ".NAME", STRINGS.UI.FormatAsLink("Toxic water", ToxicWaterElement.ID.ToUpper()));
                Strings.Add("STRINGS.ELEMENTS." + ToxicWaterElement.ID.ToUpper() + ".DESC", $"Water preserved from a long time. {containsIron}");
                Strings.Add("STRINGS.ELEMENTS." + ToxicWaterElement.IDFrozen.ToUpper() + ".NAME", STRINGS.UI.FormatAsLink("Toxic ice", ToxicWaterElement.IDFrozen.ToUpper()));
                Strings.Add("STRINGS.ELEMENTS." + ToxicWaterElement.IDFrozen.ToUpper() + ".DESC", $"Frozen toxic water. {containsIron}");

                var elementCollection = YamlIO.Parse<ElementLoader.ElementEntryCollection>(ToxicWaterElement.CONFIG, "ElementLoader.cs");
                __result.AddRange(elementCollection.elements);
            }
        }

        [HarmonyPatch(typeof(ElementLoader), "Load")]
        class ElementLoader_Load
        {
            static void Prefix(ref Hashtable substanceList, SubstanceTable substanceTable)
            {
                var water = substanceTable.GetSubstance(SimHashes.Water);
                var ice = substanceTable.GetSubstance(SimHashes.Ice);
                substanceList[ToxicWaterElement.ElementSimHash] = ToxicWaterElement.CreateSubstance(water);
                substanceList[ToxicWaterElement.FrozenElementSimHash] = ToxicWaterElement.CreateFrozenSubstance(ice.material, water.anim);
            }
        }

        [HarmonyPatch(typeof(Enum), "ToString", new Type[] { })]
        class SimHashes_ToString
        {
            static bool Prefix(ref Enum __instance, ref string __result)
            {
                if (!(__instance is SimHashes)) return true;
                return !ToxicWaterElement.SimHashNameLookup.TryGetValue((SimHashes)__instance, out __result);
            }
        }

        [HarmonyPatch(typeof(Enum), nameof(Enum.Parse), new Type[] { typeof(Type), typeof(string), typeof(bool) })]
        class SimHashes_Parse
        {
            static bool Prefix(Type enumType, string value, ref object __result)
            {
                if (!enumType.Equals(typeof(SimHashes))) return true;
                return !ToxicWaterElement.ReverseSimHashNameLookup.TryGetValue(value, out __result);
            }
        }
        [HarmonyPatch(typeof(EntityTemplates), "ExtendEntityToBasicCreature")]
        class EntityTemplates_ExtendEntityToBasicCreature
        {
            static void Postfix(ref GameObject __result,
              GameObject template,
              FactionManager.FactionID faction,
              string initialTraitID,
              string NavGridName,
              NavType navType,
              int max_probing_radius,
              float moveSpeed,
              string onDeathDropID,
              int onDeathDropCount,
              bool drownVulnerable,
              bool entombVulnerable,
              float warningLowTemperature,
              float warningHighTemperature,
              float lethalLowTemperature,
              float lethalHighTemperature)
            {
                __result.AddOrGet<GeneticTraits.GeneticTraitComponent>();
            }
        }
        [HarmonyPatch(typeof(SimpleInfoScreen), "OnSelectTarget")]
        static class SimpleInfoScreen_OnSelectTarget
        {
            public static CollapsibleDetailContentPanel TraitsPanel = null;
            public static DetailsPanelDrawer TraitsDrawer = null;
            public static GameObject LastParent = null;

            private static void InitTraitsPanel(SimpleInfoScreen instance)
            {
                if (TraitsPanel == null || LastParent != instance.gameObject)
                {
                    TraitsPanel = Util.KInstantiateUI<CollapsibleDetailContentPanel>(ScreenPrefabs.Instance.CollapsableContentPanel, instance.gameObject);
                    TraitsDrawer = new DetailsPanelDrawer(instance.attributesLabelTemplate, TraitsPanel.Content.gameObject);
                    LastParent = instance.gameObject;
                }
            }

            static void Prefix(ref SimpleInfoScreen __instance, GameObject target)
            {
                if (target != null && target.GetComponent<Klei.AI.Traits>() != null && target.HasTag(GameTags.Creature))
                {
                    InitTraitsPanel(__instance);

                    TraitsPanel.gameObject.SetActive(true);
                    TraitsPanel.HeaderLabel.text = "TRAITS";

                    TraitsDrawer.BeginDrawing();
                    foreach (Trait trait in target.GetComponent<Klei.AI.Traits>().TraitList)
                    {
                        if (!GeneticTraits.GeneticTraits.IsSupportedTrait(trait.Id)) continue;
                        var color = trait.PositiveTrait ? Constants.POSITIVE_COLOR : Constants.NEGATIVE_COLOR;
                        TraitsDrawer.NewLabel($"<color=#{color.ToHexString()}>{trait.Name}</color>").Tooltip(trait.GetTooltip());
                    }
                    TraitsDrawer.EndDrawing();
                }
                else
                {
                    TraitsPanel?.gameObject?.SetActive(false);
                }
            }
        }
     }
}
