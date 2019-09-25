using STRINGS;

namespace Primitive_Biome.Elements
{
    public class SedimentaryCalciumCarbonateElement 
    {
        public const string Data = @"elements:
  - elementId: SedimentaryCalciumCarbonate
    specificHeatCapacity: 1
    thermalConductivity: 2
    solidSurfaceAreaMultiplier: 1
    liquidSurfaceAreaMultiplier: 1
    gasSurfaceAreaMultiplier: 1
    strength: 1
    highTemp: 1683
    highTempTransitionTarget: Magma
    defaultTemperature: 283.15
    defaultMass: 1840
    maxMass: 1840
    hardness: 25
    molarMass: 50
    lightAbsorptionFactor: 1
    materialCategory: ConsumableOre
    tags:
    - BuildableAny
    buildMenuSort: 5 
    isDisabled: false
    state: Solid
    localizationID: STRINGS.ELEMENTS.SEDIMENTARYCALCIUMCARBONATE.NAME";

        public const string Id = "SedimentaryCalciumCarbonate";
        public static string Name = UI.FormatAsLink("Energized Fragment", Id.ToUpper());
        public static string Description = $"The remnants of long dead creatures. Mine for a chance to get fossils and lime.";
        public static SimHashes SimHash = (SimHashes)Hash.SDBMLower(Id);
    }
}
