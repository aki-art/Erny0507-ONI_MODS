using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using STRINGS;
using UnityEngine;
using TUNING;
using Klei.AI;
using Object = UnityEngine.Object;

namespace DreckoMorphs
{
    class AlgaeDrecko : IEntityConfig
    {
        private static float MIN_POOP_SIZE_KG = 40f;
        public const float OXYGEN_RATE = 0.0234375f;
        public const float BABY_OXYGEN_RATE = 0.01171875f;
        public static string Name = UI.FormatAsLink("Mossy Drecko", ID);
        public const string Description = "It grows a type of symbiotic algae on its back.";
        public static string EggName = UI.FormatAsLink("Mossy Drecklet Egg", EGG_ID);
        public const string BabyId = "MossyDrecklet";
        public static string BabyName = UI.FormatAsLink("Mossy Drecklet", BabyId);
        public const string BabyDescription = "It grows soft and small pieces of algae on its back.";
        public static EffectorValues tier = DECOR.BONUS.TIER4;
        public const string SymbolOverride = "";
        public const float FertilityCycles = 90.0f;
        public const float IncubationCycles = 30.0f;
        public const float MaxAge = 150f;
        public const float Hitpoints = 25f;

        public static Tag POOP_ELEMENT = SimHashes.ToxicSand.CreateTag();
        public static Tag EMIT_ELEMENT = SimHashes.Algae.CreateTag();
        private static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 3f;
        private static float CALORIES_PER_DAY_OF_PLANT_EATEN = DreckoTuning.STANDARD_CALORIES_PER_CYCLE / AlgaeDrecko.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;
        private static float KG_POOP_PER_DAY_OF_PLANT = 3f;
        private static float MIN_POOP_SIZE_IN_KG = 1.5f;
        private static float MIN_POOP_SIZE_IN_CALORIES = AlgaeDrecko.CALORIES_PER_DAY_OF_PLANT_EATEN * AlgaeDrecko.MIN_POOP_SIZE_IN_KG / AlgaeDrecko.KG_POOP_PER_DAY_OF_PLANT;
        public static float SCALE_GROWTH_TIME_IN_CYCLES = 3f;
        public static float FIBER_PER_CYCLE = 50f;
        public static int EGG_SORT_ORDER = 1;
        public const string ID = "DreckoMossy";
        public const string BASE_TRAIT_ID = "DreckoMossyTrait";
        public const string EGG_ID = "DreckoMossyEgg";

        public static Trait CreateTrait(string name)
        {
            Trait trait = Db.Get().CreateTrait(BASE_TRAIT_ID, name, name, (string)null, false, (ChoreGroup[])null, true, true);
            trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DreckoTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
            trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float)(-(double)DreckoTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
            trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, Hitpoints, name, false, false, true));
            trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, MaxAge, name, false, false, true));
            return trait;
        }
        public static GameObject CreateCritter(
  string id,
  string name,
  string desc,
  string anim_file,
  bool is_baby)
        {
            GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseDreckoConfig.BaseDrecko(id, name, desc, anim_file, BASE_TRAIT_ID, is_baby, (string)null, 308.15f, 363.15f), DreckoTuning.PEN_SIZE_PER_CREATURE);
            CreateTrait(name);

            Diet diet = new Diet(new Diet.Info[1]
    {
      new Diet.Info(new HashSet<Tag>()
      {
       (Tag)SeaLettuceConfig.ID
      }, AlgaeDrecko.POOP_ELEMENT, AlgaeDrecko.CALORIES_PER_DAY_OF_PLANT_EATEN, AlgaeDrecko.KG_POOP_PER_DAY_OF_PLANT, (string) null, 0.0f, false, true)
    });
            CreatureCalorieMonitor.Def def1 = wildCreature.AddOrGetDef<CreatureCalorieMonitor.Def>();
            def1.diet = diet;
            def1.minPoopSizeInCalories = AlgaeDrecko.MIN_POOP_SIZE_IN_CALORIES;
            wildCreature.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
            ScaleGrowthMonitor.Def def2 = wildCreature.AddOrGetDef<ScaleGrowthMonitor.Def>();
            def2.defaultGrowthRate = (float)(1.0 / (double)AlgaeDrecko.SCALE_GROWTH_TIME_IN_CYCLES / 600.0);
            def2.dropMass = AlgaeDrecko.FIBER_PER_CYCLE * AlgaeDrecko.SCALE_GROWTH_TIME_IN_CYCLES;
            def2.itemDroppedOnShear = AlgaeDrecko.EMIT_ELEMENT;
            def2.levelCount = 6;
            def2.targetAtmosphere = SimHashes.CarbonDioxide;
            return wildCreature;
        }
        public static List<FertilityMonitor.BreedingChance> EggChances = new List<FertilityMonitor.BreedingChance>()
        {

            new FertilityMonitor.BreedingChance()
            {
                egg = EGG_ID.ToTag(),
                weight =1f
            }
        };
        public GameObject CreatePrefab()
        {
            ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[4]
           {
                new ComplexRecipe.RecipeElement((Tag)DreckoConfig.EGG_ID, 2f ),
                new ComplexRecipe.RecipeElement((Tag)RawEggConfig.ID, (float) (5)),
                new ComplexRecipe.RecipeElement(SimHashes.Algae.CreateTag(), 5000f),
                new ComplexRecipe.RecipeElement(SimHashes.ToxicSand.CreateTag(), 1000f),
           };
            ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement((Tag)EGG_ID, 1f)
            };
            var r = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, (IList<ComplexRecipe.RecipeElement>)ingredients,
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
             CreateCritter(
                 id: ID,
                 name: Name,
                 desc: Description,
                 anim_file:
                 "adult_algae_drecko_kanim",
                 is_baby: false
             ),
             EGG_ID,
             EggName,
             Description,
             "egg_algae_drecko_kanim",
             DreckoTuning.EGG_MASS,
             BabyId,
             FertilityCycles,
             IncubationCycles,
             EggChances,
             EGG_SORT_ORDER);

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
    }
}
