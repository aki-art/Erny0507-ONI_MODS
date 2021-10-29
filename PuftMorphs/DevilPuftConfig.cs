using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using STRINGS;
using UnityEngine;
using TUNING;
using Klei.AI;


namespace PuftMorphs
{
    class DevilPuftConfig : IEntityConfig
    {
        private static float KG_ORE_EATEN_PER_CYCLE = 50f;
        private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / DevilPuftConfig.KG_ORE_EATEN_PER_CYCLE;
        private static float MIN_POOP_SIZE_IN_KG = 25f;
        public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 6;
        public const string ID = "PuftDevil";
        public const string BASE_TRAIT_ID = "PuftDevilBaseTrait";
        public const string EGG_ID = "PuftDevilEgg";
        public const string BABY_ID = "PuftDevilBaby";
        public const SimHashes CONSUME_ELEMENT = SimHashes.SourGas;
        public const SimHashes EMIT_ELEMENT = SimHashes.Sulfur;

        public static string Name = UI.FormatAsLink("Brimstone Puft", ID);
        public const string Description = "It was designed very different from its peers, It looks and searchs carefully for food.";
        public static string EggName = UI.FormatAsLink("Brimstone Puftlet Egg", ID);
        public static string BabyName = UI.FormatAsLink("Brimstone Puftlet", BABY_ID);
        public const string BabyDescription = "It looks menacing but It will fly away when threatened.";
        public const float warningLowTemperature = 273.15f;
        public const float warningHighTemperature = 873.15f; //303.15f;
        public static GameObject CreatePuft(
      string id,
      string name,
      string desc,
      string anim_file,
      bool is_baby)
        {
            GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BasePuftConfig.BasePuft(id, name, desc, BASE_TRAIT_ID, anim_file, is_baby, "", 
                warningLowTemperature, warningHighTemperature), PuftTuning.PEN_SIZE_PER_CREATURE);
            Trait trait = Db.Get().CreateTrait(BASE_TRAIT_ID, name, name, (string)null, false, (ChoreGroup[])null, true, true);
            trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
            trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float)(-(double)PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
            trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
            trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
            GameObject go = BasePuftConfig.SetupDiet(wildCreature, CONSUME_ELEMENT.CreateTag(), EMIT_ELEMENT.CreateTag(),
                DevilPuftConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2, (string)null, 0.0f,
                DevilPuftConfig.MIN_POOP_SIZE_IN_KG);
            go.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[1]
            {
      SimHashes.Sulfur.CreateTag()
            };
            return go;
        }

        public static List<FertilityMonitor.BreedingChance> EggChances = new List<FertilityMonitor.BreedingChance>()
        {
            new FertilityMonitor.BreedingChance()
            {
                egg = DevilPuftConfig.EGG_ID.ToTag(),
                weight = 1f
            },

        };
        public GameObject CreatePrefab()
        {

            ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[4]
            {
                new ComplexRecipe.RecipeElement((Tag)PuftConfig.EGG_ID, 2f ),
                new ComplexRecipe.RecipeElement((Tag)PuftBleachstoneConfig.EGG_ID, 1f ),
                 new ComplexRecipe.RecipeElement((Tag)PuftOxyliteConfig.EGG_ID, 1f ),
                 new ComplexRecipe.RecipeElement(SimHashes.SourGas.CreateTag(), 10f),
            };
            ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement((Tag)DevilPuftConfig.EGG_ID, 1f)
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
            return EntityTemplates.ExtendEntityToFertileCreature(DevilPuftConfig.CreatePuft(ID,
               Name,
               Description,
                "devil_puft_adult_kanim", false), EGG_ID,
               EggName,
                Description,
                "devil_puft_egg_kanim", PuftTuning.EGG_MASS, BABY_ID, 45f, 15f,
                EggChances, DevilPuftConfig.EGG_SORT_ORDER, true, false, true, 1f);
        }


        public string GetDlcId()
        {
            return DlcManager.VANILLA_ID;
        }
        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }
        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
            BasePuftConfig.OnSpawn(inst);
        }
    }
}
