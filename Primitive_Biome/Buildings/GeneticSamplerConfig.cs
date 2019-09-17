using Primitive_Biome.GeneticTraits;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUNING;
using UnityEngine;
using static STRINGS.BUILDINGS.PREFABS;

namespace Primitive_Biome.Buildings
{
    class GeneticSamplerConfig : IBuildingConfig
    {
        public const string ID = "GeneticSampler";
        public static string NAME = (LocString)UI.FormatAsLink("Genetic Sampler", ID);
        public static string DESC = (LocString)"Use fossils found on the primite biome and alter critter's biology";
        public static string EFFECT = (LocString)("Processes " + UI.FormatAsLink("Fossils", "RAREMATERIALS") + " into living critter.\n\nFossils can be found on the primitive biome.\n\nDuplicants will not fabricate items unless recipes are queued.");
        public static string RECOMBINATION_RECIPE_DESCRIPTION = (LocString)"Modify an egg dna with unknown results";
        public static ComplexRecipe RECIPE_RECOMBINATION;
        private const float INPUT_KG = 100f;
        private const float OUTPUT_KG = 100f;
        private const float OUTPUT_TEMPERATURE = 313.15f;


        public override BuildingDef CreateBuildingDef()
        {
            string id = ID;
            int width = 4;
            int height = 5;
            string anim = "genetic_sampler_kanim";
            int hitpoints = 30;
            float construction_time = 480f;
            float[] tieR5 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
            string[] allMetals = MATERIALS.ALL_METALS;
            float melting_point = 2400f;
            BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
            EffectorValues tieR6 = TUNING.NOISE_POLLUTION.NOISY.TIER6;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR5, allMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tieR6, 0.2f);
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 1600f/8;
            buildingDef.SelfHeatKilowattsWhenActive = 16f/8;
            buildingDef.ViewMode = OverlayModes.Power.ID;
            buildingDef.AudioCategory = "HollowMetal";
            buildingDef.AudioSize = "large";
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<DropAllWorkable>();
            go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
            ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
            fabricator.resultState = ComplexFabricator.ResultState.Heated;
            fabricator.heatedTemperature = 313.15f;
            fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
            fabricator.duplicantOperated = true;
            go.AddOrGet<FabricatorIngredientStatusManager>();
            go.AddOrGet<CopyBuildingSettings>();
            ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
            BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
            fabricatorWorkable.overrideAnims = new KAnimFile[1]
            {
      Assets.GetAnim((HashedString) "anim_interacts_supermaterial_refinery_kanim")
            };
            Prioritizable.AddRef(go);


            float num7 = 0.35f;
            ComplexRecipe.RecipeElement[] ingredients4 = new ComplexRecipe.RecipeElement[2]
            {
                new ComplexRecipe.RecipeElement((Tag)HatchConfig.EGG_ID, 1f ),
                new ComplexRecipe.RecipeElement((Tag)RawEggConfig.ID, (float) (40 /8))
            };
            ComplexRecipe.RecipeElement[] results4 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement((Tag)HatchConfig.EGG_ID, 1f)
            };
            var r = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, (IList<ComplexRecipe.RecipeElement>)ingredients4, (IList<ComplexRecipe.RecipeElement>)results4), ingredients4, results4)
            {
                time = 80f/8,
                description = RECOMBINATION_RECIPE_DESCRIPTION,
                nameDisplay = ComplexRecipe.RecipeNameDisplay.Result
            };
            RECIPE_RECOMBINATION = r;
            r.fabricators = new List<Tag>()
            {
                TagManager.Create(ID)
            };

        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn)(game_object =>
            {
                ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
                component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
                component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
                component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
                component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
                component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
            });
        }
    }
}
