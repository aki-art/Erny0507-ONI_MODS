using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Primitive_Biome.GeneticTraits.Traits
{
    class ElementConverterBadTrait : GeneticTraitBuilder
    {
        public override string ID => "ElementEmitter";
        public override string Name => "Skin secretions";
        public override string Description => "This critter skin absorbs small quantities of certain gas and secretes substances.";
        public override Group Group => Group.ElementConverterGroup;
        public override bool Positive => true;
        public override bool CustomDescription => true;
        public SimHashes element_input = SimHashes.Oxygen;
        public SimHashes element_output = SimHashes.Oxygen;
        public static List<SimHashes> Solids = new List<SimHashes>(){
            SimHashes.Lime,
            SimHashes.Fertilizer,
            SimHashes.Clay,
            SimHashes.IronOre,
            SimHashes.AluminumOre,
            SimHashes.Wolframite,
            SimHashes.Snow,
            };
        public static List<SimHashes> Liquids = new List<SimHashes>(){
            SimHashes.Water,
            SimHashes.DirtyWater,
            SimHashes.Brine,
            SimHashes.LiquidCarbonDioxide,
            SimHashes.Ethanol
            };
        public static List<SimHashes> Gases = new List<SimHashes>(){
            SimHashes.Hydrogen,
            SimHashes.Methane,
            SimHashes.Oxygen,
            SimHashes.ChlorineGas,
            SimHashes.CarbonDioxide,
            SimHashes.ContaminatedOxygen,
            };
        public static List<SimHashes> Possible_Inputs = new List<SimHashes>(){
            SimHashes.Hydrogen,
            SimHashes.Methane,
            SimHashes.Oxygen,
            SimHashes.ChlorineGas,
            SimHashes.CarbonDioxide,
            SimHashes.ContaminatedOxygen,
            };
        public static List<SimHashes> Possible_Outputs = new List<SimHashes>(){
            SimHashes.Hydrogen,
            SimHashes.Methane,
            SimHashes.Oxygen,
            SimHashes.ChlorineGas,
            SimHashes.CarbonDioxide,
            SimHashes.ContaminatedOxygen,
            SimHashes.Water,
            SimHashes.DirtyWater,
            SimHashes.Brine,
            SimHashes.LiquidCarbonDioxide,
            SimHashes.Ethanol,
            SimHashes.Lime,
            SimHashes.Fertilizer,
            SimHashes.Clay,
            SimHashes.IronOre,
            SimHashes.AluminumOre,
            SimHashes.Cuprite,//copper ore
            SimHashes.GoldAmalgam,
            SimHashes.Lead,
            SimHashes.Wolframite,
            SimHashes.Snow,
            SimHashes.ToxicSand,
            SimHashes.SlimeMold,
            SimHashes.IgneousRock,
            SimHashes.Katairite,
            SimHashes.Granite,
            SimHashes.Dirt,
            };
        protected override void Init()
        {
            /* Debug.Log("Randomize elements");
             Util.Shuffle<SimHashes>(Gases);
             DebugHelper.LogForEach(Gases);
             element_input = Gases.First();
             Debug.Log("Element chossen");
             Debug.Log(element_input);
             List<SimHashes> complete_list = Gases.Where(x => x != element_input).ToList();
             //Except(element_input).ToList();
             DebugHelper.Separator();
             DebugHelper.LogForEach(complete_list);
             DebugHelper.Separator();
             complete_list = complete_list.Concat(Liquids).Concat(Solids).ToList();
             DebugHelper.LogForEach(complete_list);
             Util.Shuffle(complete_list);
             DebugHelper.LogForEach(complete_list);
             DebugHelper.Separator();
             element_output = (SimHashes)complete_list.First();
             var element1 = ElementLoader.FindElementByHash(element_input);
             var element2 = ElementLoader.FindElementByHash(element_output);
             Description_custom = "This critter skin absorbs small quantities of " + element1.name + " and drops " + element2.name;*/

            UtilPB.CreateTrait(ID, Name, Description,
              on_add: delegate (GameObject go)
              {
                  ChooseTarget(go);
              },
              positiveTrait: true
            );
        }

        protected override void ApplyTrait(GameObject go)
        {
            var reduction_factor = 4;
            var consumptionRate = 0.2f / reduction_factor;
            var outputKgPerSecond = 0.2f / reduction_factor;
            if (//go.GetComponent<GeneticTraitComponent>().IsBaby() || 
                go.GetComponent<GeneticTraitComponent>().IsAdult()
                //||go.GetComponent<GeneticTraitComponent>().IsEgg()
                )
            {

                Util.Shuffle(Possible_Inputs);
                element_input = Possible_Inputs.First();
                Util.Shuffle(Possible_Outputs);
                List<SimHashes> complete_list = Possible_Outputs.Where(x => x != element_input).ToList();
                element_output = complete_list.First();
                element_input = SimHashes.ContaminatedOxygen;
                element_output = SimHashes.Hydrogen;
                var element1 = ElementLoader.FindElementByHash(element_input);
                var element2 = ElementLoader.FindElementByHash(element_output);
                var string_holder = go.AddComponent<StringHolderComponent>();
                string_holder.text = "This critter skin absorbs small quantities of " + element1.name + " and drops " + element2.name;
                string_holder.id = ID;
                go.AddOrGet<Storage>().capacityKg = 10f;
                ElementConsumer elementConsumer = (ElementConsumer)go.AddOrGet<PassiveElementConsumer>();
                elementConsumer.elementToConsume = element_input;
                elementConsumer.consumptionRate = consumptionRate;
                elementConsumer.capacityKG = 10f;
                elementConsumer.consumptionRadius = (byte)3;
                elementConsumer.showInStatusPanel = true;
                elementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f, 0.0f);
                elementConsumer.isRequired = false;
                elementConsumer.storeOnConsume = true;
                elementConsumer.showDescriptor = false;
                elementConsumer.EnableConsumption(true);

                if (element2.IsGas)
                {
                    DebugHelper.Log("element2.IsGas");
                    go.AddOrGet<UpdateElementConsumerPosition>();
                    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
                    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
                    {
                        new ElementConverter.ConsumedElement(element_input.CreateTag(), consumptionRate)
                    };
                    elementConverter.outputElements = new ElementConverter.OutputElement[1]
                    {
                        new ElementConverter.OutputElement(outputKgPerSecond, element_output, 0.0f, true, false, 0.0f, 0.5f, 1f, byte.MaxValue, 0)
                    };
                }
                if (element2.IsLiquid)
                {
                    DebugHelper.Log("element2.IsLiquid");
                    go.AddOrGet<UpdateElementConsumerPosition>();
                    BubbleSpawner bubbleSpawner = go.AddComponent<BubbleSpawner>();
                    bubbleSpawner.element = element_output;
                    bubbleSpawner.emitMass = 2f;
                    bubbleSpawner.emitVariance = 0.5f;
                    bubbleSpawner.initialVelocity = (Vector2)new Vector2f(0, 1);
                    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
                    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
                    {
                        new ElementConverter.ConsumedElement(element_input.CreateTag(), consumptionRate)
                    };
                    elementConverter.outputElements = new ElementConverter.OutputElement[1]
                    {
                        new ElementConverter.OutputElement(outputKgPerSecond, element_output, 0.0f, true, true, 0.0f, 0.5f, 1f, byte.MaxValue, 0)
                    };
                }
                if (element2.IsSolid)
                {
                    DebugHelper.Log("element2.IsSolid");
                    go.AddOrGet<UpdateElementConsumerPosition>();
                    ElementDropper elementDropper = go.AddComponent<ElementDropper>();
                    elementDropper.emitMass = 2f;
                    elementDropper.emitTag = element_output.CreateTag();
                    elementDropper.emitOffset = new Vector3(0.0f, 0.0f, 0.0f);
                    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
                    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
                    {
                        new ElementConverter.ConsumedElement(element_input.CreateTag(), consumptionRate)
                    };
                    elementConverter.outputElements = new ElementConverter.OutputElement[1]
                    {
                        new ElementConverter.OutputElement(outputKgPerSecond, element_output, 0.0f, true, true, 0.0f, 0.5f, 1f, byte.MaxValue, 0)
                    };
                }

            }



        }
    }
}

