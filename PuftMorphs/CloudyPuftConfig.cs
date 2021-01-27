using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using STRINGS;
using UnityEngine;
using TUNING;
using Klei.AI;

namespace PuftMorphs
{
    public class CloudyPuftConfig : IEntityConfig
    {
        private static float KG_ORE_EATEN_PER_CYCLE = 50f;
        private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / CloudyPuftConfig.KG_ORE_EATEN_PER_CYCLE;
        private static float MIN_POOP_SIZE_IN_KG = 25f;
        public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 5;
        public const string ID = "PuftHydrogen";
        public const string BASE_TRAIT_ID = "PuftHydrogenBaseTrait";
        public const string EGG_ID = "PuftHydrogenEgg";
        public const string BABY_ID = "PuftHydrogenBaby";
        public const SimHashes CONSUME_ELEMENT = SimHashes.Hydrogen;
        public const SimHashes EMIT_ELEMENT = SimHashes.DirtyIce;

        public static string Name = UI.FormatAsLink("Cloudy Puft", ID);
        public const string Description = "It resembles a cloud and loves the cold. It slowsly absorbs the heat in its food.";
        public static string EggName = UI.FormatAsLink("Cloudy Puftlet Egg", ID);
        public static string BabyName = UI.FormatAsLink("Cloudy Puftlet", BABY_ID);
        public const string BabyDescription = "It resembles a cloud and loves the cold. It slowsly absorbs the heat in its food.";
        public const float warningLowTemperature =0f;
        public const float warningHighTemperature = 303.15f; //303.15f;
        public static GameObject CreatePuft(
      string id,
      string name,
      string desc,
      string anim_file,
      bool is_baby)
        {
            GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BasePuftConfig.BasePuft(id, name, desc, BASE_TRAIT_ID, anim_file, is_baby, "", warningLowTemperature, warningHighTemperature), PuftTuning.PEN_SIZE_PER_CREATURE, 75f);
            Trait trait = Db.Get().CreateTrait(BASE_TRAIT_ID, name, name, (string)null, false, (ChoreGroup[])null, true, true);
            trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
            trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float)(-(double)PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
            trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
            trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
            GameObject go = BasePuftConfig.SetupDiet(wildCreature, CONSUME_ELEMENT.CreateTag(), EMIT_ELEMENT.CreateTag(), CloudyPuftConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, (string)null, 0.0f, CloudyPuftConfig.MIN_POOP_SIZE_IN_KG);
            go.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[1]
            {
      SimHashes.SolidHydrogen.CreateTag()
            };
            return go;
        }

        public static List<FertilityMonitor.BreedingChance> EggChances = new List<FertilityMonitor.BreedingChance>()
        {
            new FertilityMonitor.BreedingChance()
            {
                egg = CloudyPuftConfig.EGG_ID.ToTag(),
                weight = 1f
            },

        };
        public GameObject CreatePrefab()
        {

            ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[4]
            {
                new ComplexRecipe.RecipeElement((Tag)PuftConfig.EGG_ID, 2f ),
                new ComplexRecipe.RecipeElement((Tag)PuftAlphaConfig.EGG_ID, 1f ),
                 new ComplexRecipe.RecipeElement((Tag)PuftOxyliteConfig.EGG_ID, 2f ),
                 new ComplexRecipe.RecipeElement(SimHashes.Hydrogen.CreateTag(), 10f),
            };
            ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement((Tag)CloudyPuftConfig.EGG_ID, 1f)
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
            return EntityTemplates.ExtendEntityToFertileCreature(CloudyPuftConfig.CreatePuft(ID,
               Name,
               Description,
                "cloudy_puft_adult_kanim"
                // "puft_kanim"
                , false), EGG_ID,
               EggName,
                Description,
                "cloudy_puft_egg_kanim", PuftTuning.EGG_MASS, BABY_ID, 45f, 15f,
                EggChances, CloudyPuftConfig.EGG_SORT_ORDER, true, false, true, 1f);
        }

        public void OnPrefabInit(GameObject prefab)
        {
        }

        public void OnSpawn(GameObject inst)
        {
            BasePuftConfig.OnSpawn(inst);
        }
    }
}
