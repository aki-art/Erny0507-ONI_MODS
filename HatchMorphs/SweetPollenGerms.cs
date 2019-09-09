using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei.AI.DiseaseGrowthRules;
using Klei.AI;
using UnityEngine;
using System.Diagnostics;

namespace HatchMorphs
{
    class SweetPollenGerms : Disease
    {
        // Token: 0x0600043B RID: 1083 RVA: 0x0001F1D4 File Offset: 0x0001D5D4
        public SweetPollenGerms() : base("SweetPollenGerms", 5, new Disease.RangeInfo(263.15f, 273.15f, 363.15f, 373.15f), new Disease.RangeInfo(10f, 100f, 100f, 10f), new Disease.RangeInfo(0f, 0f, 1000f, 1000f), Disease.RangeInfo.Idempotent())
        {
            
            //Debug.Log("About to change color");
            Color32 overlayColour = new Color32(252, 3, 98, byte.MaxValue);
           // Color32 overlayColour = new Color32(66, 135, 245, byte.MaxValue);
            this.overlayColour = overlayColour;
         
          //  Debug.Log("Color changed");
        }

        // Token: 0x0600043C RID: 1084 RVA: 0x0001F240 File Offset: 0x0001D640
        protected override void PopulateElemGrowthInfo()
        {
            base.InitializeElemGrowthArray(ref this.elemGrowthInfo, Disease.DEFAULT_GROWTH_INFO);
            base.AddGrowthRule(new GrowthRule
            {
                underPopulationDeathRate = new float?(0.6666667f),
                minCountPerKG = new float?(0.4f),
                populationHalfLife = new float?(3000f),
                maxCountPerKG = new float?(500f),
                overPopulationHalfLife = new float?(10f),
                minDiffusionCount = new int?(3000),
                diffusionScale = new float?(0.001f),
                minDiffusionInfestationTickCount = new byte?(1)
            });
            base.AddGrowthRule(new StateGrowthRule(Element.State.Solid)
            {
                minCountPerKG = new float?(0.4f),
                populationHalfLife = new float?(10f),
                overPopulationHalfLife = new float?(10f),
                diffusionScale = new float?(1E-06f),
                minDiffusionCount = new int?(1000000)
            });
            base.AddGrowthRule(new StateGrowthRule(Element.State.Gas)
            {
                minCountPerKG = new float?(500f),
                underPopulationDeathRate = new float?(2.66666675f),
                populationHalfLife = new float?(2000f),
                overPopulationHalfLife = new float?(10f),
                maxCountPerKG = new float?(1000000f),
                minDiffusionCount = new int?(1000),
                diffusionScale = new float?(0.015f)
            });
            base.AddGrowthRule(new ElementGrowthRule(SimHashes.Chlorine)
            {
                populationHalfLife = new float?(10f),
                overPopulationHalfLife = new float?(10f)
            });
            base.AddGrowthRule(new StateGrowthRule(Element.State.Liquid)
            {
                minCountPerKG = new float?(0.4f),
                populationHalfLife = new float?(100f),
                overPopulationHalfLife = new float?(10f),
                maxCountPerKG = new float?(100f),
                diffusionScale = new float?(0.01f)
            });
            base.InitializeElemExposureArray(ref this.elemExposureInfo, Disease.DEFAULT_EXPOSURE_INFO);
            base.AddExposureRule(new ExposureRule
            {
                populationHalfLife = new float?(1200f)
            });
            base.AddExposureRule(new ElementExposureRule(SimHashes.Oxygen)
            {
                populationHalfLife = new float?(float.PositiveInfinity)
            });
        }

        // Token: 0x040003BA RID: 954
        public const string ID = "SweetPollenGerms";
        public new const string Name = "Sweet Scents";
        public const string Tooltip = "Sweet Scent allergens present";
    }
}