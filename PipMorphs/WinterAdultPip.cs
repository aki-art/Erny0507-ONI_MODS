using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using STRINGS;
using UnityEngine;
using TUNING;
using Klei.AI;
using Object = UnityEngine.Object;

namespace PipMorphs
{
    class WinterAdultPip : IEntityConfig
    {
        private static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.4f;
        private static float CALORIES_PER_DAY_OF_PLANT_EATEN = SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;
        private static float KG_POOP_PER_DAY_OF_PLANT = 50f*4;
        private static float MIN_POOP_SIZE_KG = 40f;
        public static int EGG_SORT_ORDER = 3;
        public const string ID = "SquirrelWinter";
        public const string BASE_TRAIT_ID = "SquirrelBaseTrait";
        public const string EGG_ID = "SquirrelWinterEgg";
        public const float OXYGEN_RATE = 0.0234375f;
        public const float BABY_OXYGEN_RATE = 0.01171875f;
        private const SimHashes EMIT_ELEMENT = SimHashes.AluminumOre;
        public static string Name = UI.FormatAsLink("Winter Pip", ID);
        public const string Description = "They prefer cold environments.";
        public static string EggName = UI.FormatAsLink("Winter Pip Egg", EGG_ID);
        public const string BabyId = "SquirrelWinterBaby";
        public static string BabyName = UI.FormatAsLink("Winter Pipsqueak", BabyId);
        public const string BabyDescription = "They prefer cold environments.";
        public static EffectorValues tier = DECOR.BONUS.TIER2;
        public const string SymbolOverride = "";
        public const float FertilityCycles = 60.0f;
        public const float IncubationCycles = 20.0f;
        public const float MaxAge = 100f;
        public const float Hitpoints = 25f;
        public const float MIN_TEMP =-50 +273.15f;
        public const float MAX_TEMP = 20+273.15f;


        public static Diet.Info[] Diet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId = null, float diseasePerKgProduced = 0.0f)
        {
            return new Diet.Info[1]
              {
      new Diet.Info(new HashSet<Tag>()
      {
        (Tag)BeanPlantConfig.ID,
        (Tag)ColdWheatConfig.ID
      }, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, true)
              };

        }
        public static Trait CreateTrait(string name)
        {
            Trait trait = Db.Get().CreateTrait(BASE_TRAIT_ID, name, name, (string)null, false, (ChoreGroup[])null, true, true);
            trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
            trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float)(-(double)SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
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
            GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseSquirrelConfig.BaseSquirrel(id, name, desc, anim_file, "SquirrelBaseTrait", is_baby, (string)null), SquirrelTuning.PEN_SIZE_PER_CREATURE, 100f);
            CreateTrait(name);
            Diet.Info[] diet_infos = Diet(
                EMIT_ELEMENT.CreateTag(), CALORIES_PER_DAY_OF_PLANT_EATEN,
                KG_POOP_PER_DAY_OF_PLANT,
                (string)null, 0.0f);
            double minPoopSizeKg = (double)MIN_POOP_SIZE_KG;
            float warningLowTemperature = MIN_TEMP;
            float warningHighTemperature = MAX_TEMP;
            float lethalLowTemperature = warningLowTemperature - 20f;
            float lethalHighTemperature = warningHighTemperature + 20f;
            wildCreature.AddOrGet<TemperatureVulnerable>().Configure(warningLowTemperature, lethalLowTemperature, warningHighTemperature, lethalHighTemperature);
            return BaseSquirrelConfig.SetupDiet(wildCreature, diet_infos, (float)minPoopSizeKg);
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
                new ComplexRecipe.RecipeElement((Tag)SquirrelConfig.EGG_ID, 2f ),
                new ComplexRecipe.RecipeElement((Tag)RawEggConfig.ID, (float) (5)),
                new ComplexRecipe.RecipeElement(BeanPlantConfig.SEED_ID.ToTag(), 10f),
                new ComplexRecipe.RecipeElement(SimHashes.Clay.CreateTag(), 1000f),
           };
            ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement((Tag)EGG_ID, 1f)
            };
            var r = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, (IList<ComplexRecipe.RecipeElement>)ingredients,
                (IList<ComplexRecipe.RecipeElement>)results), ingredients, results)
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
                 "squirrel_winter_kanim",
                 is_baby: false
             ),
             EGG_ID,
             EggName,
             Description,
             "egg_squirrel_winter_kanim",
             SquirrelTuning.EGG_MASS,
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

    }
}
