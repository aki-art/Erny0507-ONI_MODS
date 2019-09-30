using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using STRINGS;
using UnityEngine;
using TUNING;
using Klei.AI;

namespace Primitive_Biome
{
    public class SturdyHatchConfig : IEntityConfig
    {

        public const float KgFoodEatenPerCycle = 140f;
        public static float CaloriesPerKgOfFood = HatchTuning.STANDARD_CALORIES_PER_CYCLE / KgFoodEatenPerCycle;
        public const float MinPoopSizeKg = 50f;
        public static int EggSortOrder = HatchConfig.EGG_SORT_ORDER + 6; // so the base hatches are +0, +1, and +2
        public const SimHashes EmitElement = SimHashes.Diamond;
        public const string BaseTraitId = "HatchSturdyBaseTrait";
        public const float FertilityCycles = 60.0f;
        public const float IncubationCycles = 20.0f;
        public const float MaxAge = 120.0f;
        public const float Hitpoints = 220.0f;

        public const string SymbolOverride = "";
        public static List<Diet.Info> SturdyDiet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId = null, float diseasePerKgProduced = 0.0f)
        {
            return new List<Diet.Info>
    {
        new Diet.Info(new HashSet<Tag>(new Tag[]
        {
            SimHashes.Katairite.CreateTag()// abyssalite
        }), SimHashes.Diamond.CreateTag(), caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false),
        new Diet.Info(new HashSet<Tag>(new Tag[]
        {
            SimHashes.RefinedCarbon.CreateTag()
        }),SimHashes.Diamond.CreateTag(), caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false),
        new Diet.Info(new HashSet<Tag>(new Tag[]
        {
            SimHashes.Carbon.CreateTag()
        }),SimHashes.Diamond.CreateTag(), caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false),
        new Diet.Info(new HashSet<Tag>(new Tag[]
        {
            SimHashes.Glass.CreateTag()
        }),SimHashes.Diamond.CreateTag(), caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false)
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
                HatchTuning.PEN_SIZE_PER_CREATURE,
                MaxAge);
            CreateTrait(name);

            List<Diet.Info> diet_infos = SturdyDiet(
                poopTag: EmitElement.CreateTag(),
                caloriesPerKg: CaloriesPerKgOfFood,
                producedConversionRate: TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL);//nerfed effiendy from 3 to normal
            wildCreature.AddOrGet<DecorProvider>()?.SetValues(tier);
            return BaseHatchConfig.SetupDiet(wildCreature, diet_infos, CaloriesPerKgOfFood, MinPoopSizeKg);
        }

        public static List<FertilityMonitor.BreedingChance> EggChances = new List<FertilityMonitor.BreedingChance>()
        {
            new FertilityMonitor.BreedingChance()
            {
                egg = "HatchHardEgg".ToTag(),
                weight = 0.15f
            },
             new FertilityMonitor.BreedingChance()
            {
                egg = "HatchMetalEgg".ToTag(),
                weight = 0.15f
            },
            new FertilityMonitor.BreedingChance()
            {
                egg = EGG_ID.ToTag(),
                weight = 0.70f
            }
        };

        public GameObject CreatePrefab()
        {

            return EntityTemplates.ExtendEntityToFertileCreature(
               CreateHatch(
                   id: Id,
                   name: Name,
                   desc: Description,
                   anim_file: "diamond_hatch_adult_kanim",
                   is_baby: false
               ),
               EGG_ID,
               EggName,
               Description,
               "diamond_hatch_egg_kanim",
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

        public const string BASE_TRAIT_ID = "HatchSturdyBaseTrait";
        public const string Id = "HatchSturdy";
        public static string Name = UI.FormatAsLink("Sturdy Hatch", Id);
        public const string Description = "Its body is full of shiny crystals.";
        public const string EGG_ID = "HatchSturdyEgg";
        public static string EggName = UI.FormatAsLink("Sturdy Hatchling Egg", EGG_ID);
        public const string BabyId = "HatchSturdyBaby";
        public static string BabyName = UI.FormatAsLink("Sturdy Hatchling", BabyId);
        public const string BabyDescription = "Its tiny body is full of shiny crystals.";
        public static EffectorValues tier = DECOR.BONUS.TIER3;
    }
}
