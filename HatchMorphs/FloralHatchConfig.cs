using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TUNING;
namespace HatchMorphs
{
    class FloralHatchConfig : IEntityConfig
    {

        public const float KgFoodEatenPerCycle = 140f;
        public static float CaloriesPerKgOfFood = HatchTuning.STANDARD_CALORIES_PER_CYCLE / KgFoodEatenPerCycle;
        public const float MinPoopSizeKg = 50f;
        public static int EggSortOrder = HatchConfig.EGG_SORT_ORDER + 4; // so the base hatches are +0, +1, and +2
        public const SimHashes EmitElement = SimHashes.Carbon;
        public  string drops_1 = NectarConfig.Id;
        public string drops_2 = FilamentsConfig.Id;
        public const string BaseTraitId = "HatchFloralBaseTrait";
        public const float FertilityCycles = 60.0f;
        public const float IncubationCycles = 20.0f;
        public const float MaxAge = 140.0f;
        public const float Hitpoints = 200.0f;
        public const float seed_cal_multiplier = 3.0f;
        private const float dirt_cal_multiplier = 0.5f;
        public const float food_cal_multiplier = 40.0f;

        public const string SymbolOverride = "";
        public static List<Diet.Info> FloralDiet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId = null, float diseasePerKgProduced = 0.0f)
        {
            return new List<Diet.Info>
    {
        new Diet.Info(new HashSet<Tag>(new Tag[]
        {
          (Tag)PrickleFlowerConfig.SEED_ID
        }), NectarConfig.Id.ToTag(), caloriesPerKg*seed_cal_multiplier, food_cal_multiplier, diseaseId, diseasePerKgProduced, false, false),
        new Diet.Info(new HashSet<Tag>(new Tag[]
        {
            (Tag)BasicSingleHarvestPlantConfig.SEED_ID
       
        }), NectarConfig.Id.ToTag(), caloriesPerKg*seed_cal_multiplier, food_cal_multiplier, diseaseId, diseasePerKgProduced, false, false),
        new Diet.Info(new HashSet<Tag>(new Tag[]
        {
           (Tag) BasicFabricMaterialPlantConfig.SEED_ID
        }), NectarConfig.Id.ToTag(), caloriesPerKg*seed_cal_multiplier, food_cal_multiplier, diseaseId, diseasePerKgProduced, false, false),
        new Diet.Info(new HashSet<Tag>(new Tag[]
        {
            (Tag)ForestTreeConfig.SEED_ID
        }), NectarConfig.Id.ToTag(), caloriesPerKg*seed_cal_multiplier, food_cal_multiplier, diseaseId, diseasePerKgProduced, false, false),
        new Diet.Info(new HashSet<Tag>(new Tag[]
        {
            (Tag)MushroomPlantConfig.SEED_ID
        }), NectarConfig.Id.ToTag(), caloriesPerKg*seed_cal_multiplier, food_cal_multiplier, diseaseId, diseasePerKgProduced, false, false),
        new Diet.Info(new HashSet<Tag>(new Tag[]
        {
            (Tag)SwampLilyConfig.SEED_ID
        }), NectarConfig.Id.ToTag(), caloriesPerKg*seed_cal_multiplier, food_cal_multiplier, diseaseId, diseasePerKgProduced, false, false),
       new Diet.Info(new HashSet<Tag>(new Tag[]
        {
            SimHashes.Dirt.CreateTag()
        }),FilamentsConfig.Id.ToTag(), caloriesPerKg, producedConversionRate*dirt_cal_multiplier, diseaseId, diseasePerKgProduced, false, false),
     
    };
        }
        public static Trait CreateTrait(string name)
        {
            Trait trait = Db.Get().CreateTrait(BaseTraitId, name, name, null, false, null, true, true);
            trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
            trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float)(-HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
            trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, Hitpoints, name, false, false, true));
            trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, MaxAge, name, false, false, true));
            return trait;
        }
        public static GameObject CreateHatch(
          string id,
          string name,
          string desc,
          string anim_file,
          bool is_baby)
        {
            GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(
                BaseHatchConfig.BaseHatch(
                    id,
                    name,
                    desc,
                    anim_file,
                    BaseTraitId,
                    is_baby,
                    SymbolOverride
                ),
                HatchTuning.PEN_SIZE_PER_CREATURE);
            CreateTrait(name);

            List<Diet.Info> diet_infos = FloralDiet(
                poopTag: GameTags.Edible,
                caloriesPerKg: CaloriesPerKgOfFood,
                producedConversionRate: TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_1);//
            wildCreature.AddOrGet<DecorProvider>()?.SetValues(tier);

            DiseaseDropper.Def def = wildCreature.AddOrGetDef<DiseaseDropper.Def>();
            def.diseaseIdx = Db.Get().Diseases.GetIndex(PollenGerms.ID);
            def.emitFrequency = 1f;
            def.averageEmitPerSecond = 1000*40;
            def.singleEmitQuantity = 0;

      

            IlluminationVulnerable illuminationVulnerable = wildCreature.AddOrGet<IlluminationVulnerable>();
            illuminationVulnerable.SetPrefersDarkness(false);

            wildCreature.AddOrGetDef<CreatureLightMonitor.Def>();
            return BaseHatchConfig.SetupDiet(wildCreature, diet_infos, CaloriesPerKgOfFood, MinPoopSizeKg);
        }

        public static List<FertilityMonitor.BreedingChance> EggChances = new List<FertilityMonitor.BreedingChance>()
        {
             new FertilityMonitor.BreedingChance()
            {
                egg = HatchVeggieConfig.EGG_ID.ToTag(),
                weight = 0.15f
            },
             new FertilityMonitor.BreedingChance()
            {
                egg = WoodenHatchConfig.EggId.ToTag(),
                weight = 0.15f
            },
            new FertilityMonitor.BreedingChance()
            {
                egg = EggId.ToTag(),
                weight = 0.70f
            }
        };

        public GameObject CreatePrefab()
        {
            ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[4]
           {
                new ComplexRecipe.RecipeElement((Tag)HatchConfig.EGG_ID, 2f ),
                new ComplexRecipe.RecipeElement((Tag)RawEggConfig.ID, (float) (5)),
                new ComplexRecipe.RecipeElement(PrickleFlowerConfig.SEED_ID.ToTag(), 10f),
                new ComplexRecipe.RecipeElement(SpiceVineConfig.SEED_ID.ToTag(), 10f),
           };
            ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement((Tag)EggId, 1f)
            };
            var r = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(Id, (IList<ComplexRecipe.RecipeElement>)ingredients,
                (IList<ComplexRecipe.RecipeElement>)results), ingredients, results, 0)
            {
                time = 80f / 8,
                description = BabyDescription,
                nameDisplay = ComplexRecipe.RecipeNameDisplay.Result
            };
            r.fabricators = new List<Tag>()
            {
                TagManager.Create(SupermaterialRefineryConfig.ID)
            };

            return EntityTemplates.ExtendEntityToFertileCreature(
               CreateHatch(
                   id: Id,
                   name: Name,
                   desc: Description,
                   anim_file: "floral_hatch_adult_kanim", 
                   is_baby: false
               ),
               EggId,
               EggName,
               Description,
               "floral_hatch_egg_kanim",
               HatchTuning.EGG_MASS,
               BabyId,
               FertilityCycles,
               IncubationCycles,
               EggChances,
               HatchConfig.EGG_SORT_ORDER);

        }

        public void OnPrefabInit(GameObject inst)
        {

        }

        public void OnSpawn(GameObject inst)
        {

        }

        public string GetDlcId()
        {
            return DlcManager.VANILLA_ID;
        }
        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }
        public const string BASE_TRAIT_ID = "HatchFloralBaseTrait";
        public const string Id = "HatchFloral";
        public static string Name = UI.FormatAsLink("Floral Hatch", Id);
        public const string Description = "The flower on its back releases a sweet scent that helps with stress.";
        public const string EggId = "HatchFloralEgg";
        public static string EggName = UI.FormatAsLink("Floral Hatchling Egg", EggId);
        public const string BabyId = "HatchFloralBaby";
        public static string BabyName = UI.FormatAsLink("Floral Hatchling", BabyId);
        public const string BabyDescription = "It has a bulb growing on its back.";
        public static EffectorValues tier = DECOR.BONUS.TIER3;

        public static float Dirt_cal_multiplier => dirt_cal_multiplier;
    }
}
