using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using STRINGS;
using UnityEngine;
using TUNING;
using Klei.AI;

namespace HatchMorphs
{
    public class DiamondHatchConfig : IEntityConfig
    {

        public const float KgFoodEatenPerCycle = 140f;
        public static float CaloriesPerKgOfFood = HatchTuning.STANDARD_CALORIES_PER_CYCLE / KgFoodEatenPerCycle;
        public const float MinPoopSizeKg = 50f;
        public static int EggSortOrder = HatchConfig.EGG_SORT_ORDER + 3; // so the base hatches are +0, +1, and +2
        public const SimHashes EmitElement = SimHashes.Diamond;
        public const string BaseTraitId = "HatchDiamondBaseTrait";
        public const float FertilityCycles = 60.0f;
        public const float IncubationCycles = 20.0f;
        public const float MaxAge = 100.0f;
        public const float Hitpoints = 25.0f;

        public const string SymbolOverride = "";
        public static List<Diet.Info> DiamondDiet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId=null, float diseasePerKgProduced= 0.0f)
        {
            return new List<Diet.Info>
    {
        new Diet.Info(new HashSet<Tag>(new Tag[]
        {
            SimHashes.Katairite.CreateTag()
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

            List<Diet.Info> diet_infos = DiamondDiet(
                poopTag: EmitElement.CreateTag(),
                caloriesPerKg: CaloriesPerKgOfFood,
                producedConversionRate: TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_3);
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
                egg = EggId.ToTag(),
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
                   anim_file: "diamond_hatch_kanim", // this is your new hatch anim - it should be made from the unmodified anim + build but your modified texture/png file.
                   is_baby: false
               ),
               EggId,
               EggName,
               Description,
               "egg_hatch_new_kanim", // replace this with your egg anim
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

        public const string BASE_TRAIT_ID = "HatchDiamondBaseTrait";
        public const string Id = "HatchDiamond";
        public static string Name = UI.FormatAsLink("Diamond Hatch", Id);
        public const string Description = "It's body is full of shiny crystals.";
        public const string EggId = "HatchDiamondEgg";
        public static string EggName = UI.FormatAsLink("Diamond Hatchling Egg", EggId);
        public const string BabyId = "HatchDiamondBaby";
        public static string BabyName = UI.FormatAsLink("Diamond Hatchling", BabyId);
        public const string BabyDescription = "It's tiny body is full of shiny crystals.";
        public static EffectorValues tier = DECOR.BONUS.TIER3;
    }
}
