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
        public const string ID = "SweetPollenGerms";
        public new const string Name = "Sweet Scents";
        public const string Tooltip = "Sweet Scent allergens present";
        //public SweetPollenGerms() : base("SweetPollenGerms", 5, new Disease.RangeInfo(263.15f, 273.15f, 363.15f, 373.15f), new Disease.RangeInfo(10f, 100f, 100f, 10f), new Disease.RangeInfo(0f, 0f, 1000f, 1000f), Disease.RangeInfo.Idempotent())
        //{

        //Color32 overlayColour = new Color32(252, 3, 98, byte.MaxValue);
        //this.overlayColour = overlayColour;

        // }
        public SweetPollenGerms(bool statsOnly) : base(nameof(SweetPollenGerms),
            (byte)5,
            new Disease.RangeInfo(263.15f, 273.15f, 363.15f, 373.15f),
            new Disease.RangeInfo(10f, 100f, 100f, 10f), 
            new Disease.RangeInfo(0.0f, 0.0f, 1000f, 1000f), 
            Disease.RangeInfo.Idempotent(),
            statsOnly)
        {
        }
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

    }
}