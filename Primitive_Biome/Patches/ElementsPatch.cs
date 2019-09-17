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
    class ElementsPatch
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
    }
}
