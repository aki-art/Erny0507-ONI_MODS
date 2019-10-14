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
using Primitive_Biome.GeneticTraits.Traits;

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
                        var traitComponent = GeneticTraits.GeneticTraits.getTrait(trait.Id);
                        if (traitComponent.CustomDescription)
                        {
                            var textholders = target.GetComponents<StringHolderComponent>();
                            if (textholders != null)
                            {
                                var text = textholders.Where(x => x.id == trait.Id).First();
                                if (text.text != null)
                                {
                                    TraitsDrawer.NewLabel(text.text);
                                }
                            }
                        }
                    }
                    TraitsDrawer.EndDrawing();
                }
                else
                {
                    TraitsPanel?.gameObject?.SetActive(false);
                }
            }
        }

        [HarmonyPatch(typeof(OverlayScreen))]
        [HarmonyPatch("ToggleOverlay")]
        public static class OverlayMenu_OnOverlayChanged_Patch
        {
            public static void Prefix(HashedString newMode, ref OverlayScreen __instance, out bool __state)
            {
                var val = Traverse.Create(__instance).Field("currentModeInfo").Field("mode").Method("ViewMode").GetValue<HashedString>();

                __state = val == OverlayModes.Decor.ID && newMode != OverlayModes.Decor.ID;
            }
            public static void Postfix(bool __state)
            {
                if (!__state)
                {
                    return;
                }
                
                foreach (var capturable in Components.Capturables.Items)
                {
                    var gtc = capturable.GetComponent<GeneticTraitComponent>();
                    if (gtc != null)
                    {
                        var flag = false;
                        if (gtc.IsEgg())
                        { 
                            var fromTraits = capturable.GetComponent<AIGeneticTraits>();
                            if (fromTraits != null)
                            {

                                var traits_present = fromTraits.GetTraitIds();
                                //var t = traits_present.Where(x => x == (new OffColor()).ID).First();
                                var t_ = traits_present.Where(x => x == (new OffColor()).ID).ToList();
                                string t = null;
                                if (t_.Count>0)
                                {
                                    t = t_.First();
                                }
                                
                                if (t != null)
                                {
                                    flag = true;
                                }
                            }
                        }
                        else
                        {
                            var fromTraits = capturable.GetComponent<Traits>();
                            if (fromTraits != null)
                            {
                                var traits_present = fromTraits.GetTraitIds();
                                //var t = traits_present.Where(x => x == (new OffColor()).ID).First();
                                var t_ = traits_present.Where(x => x == (new OffColor()).ID).ToList();
                                string t = null;
                                if (t_.Count > 0)
                                {
                                    t = t_.First();
                                }

                                if (t != null)
                                {
                                    flag = true;
                                }
                            }
                        }
                        if (flag)//check if is offcolor, this was discarded and must be deleted latter
                        {
                            var text_holders = capturable.GetComponents<StringHolderComponent>();
                            if (text_holders.Length > 0)
                            {
                                var text_holder = text_holders.Where(x => x.id == OffColor.id_color).First();
                                if (text_holder != null)
                                {
                                    //UtilPB.ApplyTint(capturable, text_holder.color);
                                }
                            }

                        }

                    }

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
