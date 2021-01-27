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

namespace HatchMorphs
{
    class WoodenHatchConfig : IEntityConfig
    {

        public const float KgFoodEatenPerCycle = 140f;
        public static float CaloriesPerKgOfFood = HatchTuning.STANDARD_CALORIES_PER_CYCLE / KgFoodEatenPerCycle;
        public const float MinPoopSizeKg = 50f;
        public static int EggSortOrder = HatchConfig.EGG_SORT_ORDER + 5; // so the base hatches are +0, +1, and +2
        //public const SimHashes EmitElement = SimHashes.Diamond;
        public const string BaseTraitId = "HatchWoodenBaseTrait";
        public const float FertilityCycles = 60.0f;
        public const float IncubationCycles = 20.0f;
        public const float MaxAge = 110.0f;
        public const float Hitpoints = 210.0f;
        public static SimHashes element_input = SimHashes.ContaminatedOxygen;
        public static SimHashes element_output = SimHashes.Oxygen;
        public const string SymbolOverride = "";
        public static List<Diet.Info> WoodenDiet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId = null, float diseasePerKgProduced = 0.0f)
        {
            return new List<Diet.Info>
    {
        new Diet.Info(new HashSet<Tag>(new Tag[]
        {
            WoodLogConfig.ID
        }), SimHashes.Fertilizer.CreateTag(), caloriesPerKg*2, producedConversionRate, diseaseId, diseasePerKgProduced, false, false),
        new Diet.Info(new HashSet<Tag>(new Tag[]
        {
            SimHashes.Dirt.CreateTag()
        }),SimHashes.Carbon.CreateTag(), caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false),
        new Diet.Info(new HashSet<Tag>(new Tag[]
        {
            SimHashes.Clay.CreateTag()
        }),SimHashes.Ethanol.CreateTag(), caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false),
        new Diet.Info(new HashSet<Tag>(new Tag[]
        {
            SimHashes.Sand.CreateTag()
        }),SimHashes.Carbon.CreateTag(), caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false)
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

            List<Diet.Info> diet_infos = WoodenDiet(
                poopTag: WoodLogConfig.TAG,
                caloriesPerKg: CaloriesPerKgOfFood,
                producedConversionRate: TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2);//nerfed effiendy from 3 to normal
            wildCreature.AddOrGet<DecorProvider>()?.SetValues(tier);
            if (!is_baby)
            {

                wildCreature.AddComponent<Storage>().capacityKg = 10f;
                ElementConsumer elementConsumer = (ElementConsumer)wildCreature.AddOrGet<PassiveElementConsumer>();
                elementConsumer.elementToConsume = element_input;
                elementConsumer.consumptionRate = 0.2f;
                elementConsumer.capacityKG = 10f;
                elementConsumer.consumptionRadius = (byte)3;
                elementConsumer.showInStatusPanel = true;
                elementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f, 0.0f);
                elementConsumer.isRequired = false;
                elementConsumer.storeOnConsume = true;
                elementConsumer.showDescriptor = false;
                ElementConverter elementConverter = wildCreature.AddOrGet<ElementConverter>();
                elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
                {
        new ElementConverter.ConsumedElement(element_input.CreateTag(), 0.2f)
                };
                elementConverter.outputElements = new ElementConverter.OutputElement[1]
                {
        new ElementConverter.OutputElement(0.2f, element_output, 0.0f,false, false, 0.0f, 0.5f, 1f, byte.MaxValue, 0)
                };
            }

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
                egg = FloralHatchConfig.EggId.ToTag(),
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
                   anim_file: "wooden_hatch_adult_kanim",
                   is_baby: false
               ),
               EggId,
               EggName,
               Description,
               "wooden_hatch_egg_kanim",
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
            ElementConsumer component = inst.GetComponent<ElementConsumer>();
            if (!((Object)component != (Object)null))
                return;
            component.EnableConsumption(true);
        }

        public const string BASE_TRAIT_ID = "HatchWoodenBaseTrait";
        public const string Id = "HatchWooden";
        public static string Name = UI.FormatAsLink("Wooden Hatch", Id);
        public const string Description = "Its body is cover on bark skin.";
        public const string EggId = "HatchWoodenEgg";
        public static string EggName = UI.FormatAsLink("Wooden Hatchling Egg", EggId);
        public const string BabyId = "HatchWoodenBaby";
        public static string BabyName = UI.FormatAsLink("Wooden Hatchling", BabyId);
        public const string BabyDescription = "Its tiny body is cover on soft bark skin";
        public static EffectorValues tier = DECOR.BONUS.TIER1;
    }
}
