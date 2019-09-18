using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Primitive_Biome.Elements
{
    public static class ToxicWaterElement
    {
        public static string ID = "ToxicWater";
        public static string IDFrozen = "ToxicIce";

        public static readonly Color32 ElementColor = new Color32(19, 3, 252, 255);//209, 195, 148, 255| 242, 206, 82 |
        public static readonly Color32 ElementFrozenColor = ElementColor;// new Color32(44, 23, 102, 255);

        public static readonly SimHashes ElementSimHash = (SimHashes)Hash.SDBMLower(ID);
        public static readonly SimHashes FrozenElementSimHash = (SimHashes)Hash.SDBMLower(IDFrozen);

        public static readonly Dictionary<SimHashes, string> SimHashNameLookup = new Dictionary<SimHashes, string>
    {
      { ElementSimHash, ID },
      { FrozenElementSimHash, IDFrozen }
    };

        public static readonly Dictionary<string, object> ReverseSimHashNameLookup =
          SimHashNameLookup.ToDictionary(x => x.Value, x => x.Key as object);
        public const string CONFIG = @"
---
elements:
  - elementId: ToxicWater
    maxMass: 1000
    liquidCompression: 1.02
    speed: 100
    minHorizontalFlow: 0.1
    minVerticalFlow: 0.01
    specificHeatCapacity: 3.49
    thermalConductivity: 0.58
    solidSurfaceAreaMultiplier: 1
    liquidSurfaceAreaMultiplier: 25
    gasSurfaceAreaMultiplier: 1
    lowTemp: 293.15
    highTemp: 393.15
    lowTempTransitionTarget: ToxicIce
    highTempTransitionTarget: Steam
    highTempTransitionOreId: Cyanide
    highTempTransitionOreMassConversion: 0.005
    defaultTemperature: 310
    defaultMass: 1000
    molarMass: 30
    toxicity: 10
    lightAbsorptionFactor: 0.8
    tags:
    - Mixture
    - AnyWater
    - Toxic
    isDisabled: false
    state: Liquid
    localizationID: STRINGS.ELEMENTS.TOXICWATER.NAME
  - elementId: ToxicIce
    specificHeatCapacity: 3.05
    thermalConductivity: 1
    solidSurfaceAreaMultiplier: 1
    liquidSurfaceAreaMultiplier: 1
    gasSurfaceAreaMultiplier: 1
    strength: 1
    highTemp: 272.5
    highTempTransitionTarget: ToxicWater
    defaultTemperature: 230
    defaultMass: 500
    maxMass: 800
    hardnessTier: 2
    hardness: 10
    molarMass: 35
    lightAbsorptionFactor: 0.8
    materialCategory: Liquifiable
    tags:
    - IceOre
    - Mixture
    - Toxic
    buildMenuSort: 5
    isDisabled: false
    state: Solid
    localizationID: STRINGS.ELEMENTS.TOXICICE.NAME
";

        public static Substance CreateSubstance(Substance source)
        {
            return ModUtil.CreateSubstance(
              name: ID,
              state: Element.State.Liquid,
              kanim: source.anim,
              material: source.material,
              colour: ElementColor,
              ui_colour: ElementColor,
              conduit_colour: ElementColor
            );
        }

        static Texture2D TintTexture(Texture sourceTexture, string name)
        {
            Texture2D newTexture = UtilPB.DuplicateTexture(sourceTexture as Texture2D);
            var pixels = newTexture.GetPixels32();
            for (int i = 0; i < pixels.Length; ++i)
            {
                var gray = ((Color)pixels[i]).grayscale * 1.5f;
                pixels[i] = (Color)ElementColor * gray;
            }
            newTexture.SetPixels32(pixels);
            newTexture.Apply();
            newTexture.name = name;
            return newTexture;
        }

        static Material CreateFrozenMaterial(Material source)
        {
            var frozenMaterial = new Material(source);

            Texture2D newTexture = TintTexture(frozenMaterial.mainTexture, IDFrozen.ToLower());

            frozenMaterial.mainTexture = newTexture;
            frozenMaterial.name = "mat"+IDFrozen;

            return frozenMaterial;
        }

        public static Substance CreateFrozenSubstance(Material sourceMaterial, KAnimFile sourceAnim)
        {
            return ModUtil.CreateSubstance(
              name: IDFrozen,
              state: Element.State.Solid,
              kanim: sourceAnim,
              material: CreateFrozenMaterial(sourceMaterial),
              colour: ElementFrozenColor,
              ui_colour: ElementFrozenColor,
              conduit_colour: ElementFrozenColor
            );
        }
    }
}
