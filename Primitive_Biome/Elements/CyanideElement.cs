using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Primitive_Biome.Elements
{
    static class CyanideElement
    {
        public static string ID = "Cyanide";
        public static readonly Color32 CYANIDE_FADED_YELLOW = new Color32(242, 206, 82, 255);
        //public static readonly SimHashes BloodSimHash = (SimHashes)Hash.SDBMLower("Blood");
        //public static readonly SimHashes FrozenBloodSimHash = (SimHashes)Hash.SDBMLower("FrozenBlood");
 public const string Data = @"elements:
  - elementId: Cyanide
    specificHeatCapacity: 1
    thermalConductivity: 2
    solidSurfaceAreaMultiplier: 1
    liquidSurfaceAreaMultiplier: 1
    gasSurfaceAreaMultiplier: 1
    strength: 0.1
    highTemp: 2000
    highTempTransitionTarget: MoltenCyanide
    defaultTemperature:  290
    defaultMass: 700
    maxMass: 2000
    hardness: 5
    molarMass: 26.02
    lightAbsorptionFactor: 1
    materialCategory: ConsumableOre
    buildMenuSort: 5
    isDisabled: false
    state: Solid
    toxicity: 10
    localizationID: STRINGS.ELEMENTS.CYANIDE.NAME";

        public const string Id = "Cyanide";
        public static string Name = UI.FormatAsLink("Cyanide", Id.ToUpper());
        public static string Description = $"Cyanide in its purest form. Not safe to eat.";
        public static SimHashes SimHash = (SimHashes)Hash.SDBMLower(Id);
    }
}
