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
using System.Diagnostics;
using Primitive_Biome.GeneticTraits;

namespace Primitive_Biome.Patches
{
    class GeneticTraitsPatch
    {
        [HarmonyPatch(typeof(EntityTemplates), "ExtendEntityToBasicCreature")]
        class EntityTemplates_ExtendEntityToBasicCreature
        {
            //adds traits to criatures
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
                __result.AddOrGet<GeneticTraitComponent>();
            }
        }
        [HarmonyPatch(typeof(EggConfig), "CreateEgg")]
        class EggConfig_CreateEgg
        {
            static void Postfix(ref GameObject __result,
                                string id,
                                string name,
                                string desc,
                                Tag creature_id,
                                string anim,
                                float mass,
                                int egg_sort_order,
                                float base_incubation_rate)
            {
                Debug.Log("EGGS added trait");

                var mo = __result.AddOrGet<Modifiers>();
                mo.attributes = new Attributes(__result);
                Debug.Log(mo.GetAttributes());
                //__result.AddComponent<Modifiers>();
                __result.AddOrGet<AIGeneticTraits>();
                __result.AddOrGet<GeneticTraitComponent>();
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
                if (target != null && (target.GetComponent<Klei.AI.Traits>() != null || target.GetComponent<AIGeneticTraits>() != null) && (target.HasTag(GameTags.Creature) || target.HasTag(GameTags.Egg)))
                {
                    InitTraitsPanel(__instance);
                    TraitsPanel.gameObject.SetActive(true);
                    TraitsPanel.HeaderLabel.text = "GENETIC TRAITS";
                    TraitsDrawer.BeginDrawing();
                    List<Trait> traits = null;
                    if (target.HasTag(GameTags.Egg) && target.GetComponent<AIGeneticTraits>() != null)
                    {
                        traits = target.GetComponent<AIGeneticTraits>().TraitList;
                    }
                    else
                    {
                        traits = target.GetComponent<Klei.AI.Traits>().TraitList;
                    }
                    foreach (Trait trait in traits)
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
        [HarmonyPatch(typeof(Amount), "Copy")]
        static class Amount_Copy
        {
            static void Prefix(GameObject to, GameObject from)
            {
                string callingMethod = new StackFrame(2).GetMethod().Name;
                Debug.LogWarning(callingMethod);
                if (callingMethod == "SpawnBaby")
                {
                    from.GetComponent<GeneticTraitComponent>()?.TransferTo(to.AddOrGet<GeneticTraitComponent>());//replace this for the traits

                }
            }
        }

    }
}
