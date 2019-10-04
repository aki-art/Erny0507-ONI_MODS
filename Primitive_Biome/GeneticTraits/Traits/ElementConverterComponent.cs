using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSerialization;
using UnityEngine;
namespace Primitive_Biome.GeneticTraits.Traits
{
    [SerializationConfig(MemberSerialization.OptIn)]
    class ElementConverterComponent : KMonoBehaviour, ISaveLoadable

    {
        [Serialize]
        public bool isSet = false;
        [Serialize]
        public bool isGood = true;
        [Serialize]
        public SimHashes element_input;
        [Serialize]
        public SimHashes element_output;
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

        }
        protected void OnSpawn(GameObject go)
        {
            if (!isSet)
            {
                setConfiguration(go);
                
            }
            if (//go.GetComponent<GeneticTraitComponent>().IsBaby() || 
               go.GetComponent<GeneticTraitComponent>().IsAdult()
               //||go.GetComponent<GeneticTraitComponent>().IsEgg()
               )
            {
                var ID = ElementConverterGoodTrait.ID_Type;
                var reduction_factor = 4;
                var consumptionRate = 0.2f / reduction_factor;
                var outputKgPerSecond = 0.2f / reduction_factor;

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
        protected override void OnCleanUp()
        {
            base.OnCleanUp();
        }
        public void setConfiguration(GameObject go)
        {
            List<SimHashes> Possible_Inputs;
            List<SimHashes> Possible_Outputs;

            if (isGood)
            {
                Possible_Inputs = ElementConverterGoodTrait.Possible_Inputs;
                Possible_Outputs = ElementConverterGoodTrait.Possible_Outputs;
            }
            else
            {
                Possible_Inputs = ElementConverterBadTrait.Possible_Inputs;
                Possible_Outputs = ElementConverterBadTrait.Possible_Outputs;
            }

            if (//go.GetComponent<GeneticTraitComponent>().IsBaby() || 
                go.GetComponent<GeneticTraitComponent>().IsAdult()
                //||go.GetComponent<GeneticTraitComponent>().IsEgg()
                )
            {
                var reduction_factor = 4;
                var consumptionRate = 0.2f / reduction_factor;
                var outputKgPerSecond = 0.2f / reduction_factor;
                Util.Shuffle(Possible_Inputs);
                element_input = Possible_Inputs.First();
                Util.Shuffle(Possible_Outputs);
                List<SimHashes> complete_list = Possible_Outputs.Where(x => x != element_input).ToList();
                element_output = complete_list.First();
                element_input = SimHashes.ContaminatedOxygen;
                element_output = SimHashes.Hydrogen;
            }
            isSet = true;
        }
    }
}
