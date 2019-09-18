using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Primitive_Biome.GeneticTraits.Traits
{
    class ElementConverterTrait : GeneticTraitBuilder
    {
        public override string ID => "ElementEmitter";
        public override string Name => "Element Emitter";
        public override string Description => "Element Emitter";
public string Description_custom="";
        public override Group Group => Group.ElementConverterGroup;

        public override bool Positive => throw new NotImplementedException();

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
SimHashes.Chlorine,
SimHashes.CarbonDioxide,
SimHashes.ContaminatedOxygen,
};
        protected override void Init()
        {
        Util.Shuffle(Gases);
        element_input=Gases.First();
            List<SimHashes> complete_list = Gases.Where(x => x != element_input).ToList();
            //Except(element_input).ToList();
            complete_list =  complete_list.Concat(Liquids).Concat(Solids).ToList();
          Util.Shuffle(complete_list);
          element_output=(SimHashes)complete_list.First();
        Description_custom="This critter skin absorbs small quantities of "+element_input+" and drops "+element_output;

            UtilPB.CreateTrait(ID, Name, Description_custom,
              on_add: delegate (GameObject go)
              {
                  ChooseTarget(go);
              },
              positiveTrait: true
            );
        }

        protected override void ApplyTrait(GameObject go)
        {

            go.AddComponent<Storage>().capacityKg = 10f;
            ElementConsumer elementConsumer = (ElementConsumer)go.AddOrGet<PassiveElementConsumer>();
            elementConsumer.elementToConsume = element_input;
            elementConsumer.consumptionRate = 0.2f;
            elementConsumer.capacityKG = 10f;
            elementConsumer.consumptionRadius = (byte)3;
            elementConsumer.showInStatusPanel = true;
            elementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f, 0.0f);
            elementConsumer.isRequired = false;
            elementConsumer.storeOnConsume = true;
            elementConsumer.showDescriptor = false;
            go.AddOrGet<UpdateElementConsumerPosition>();
            //BubbleSpawner bubbleSpawner = go.AddComponent<BubbleSpawner>();
            //bubbleSpawner.element = element_input;
            //bubbleSpawner.emitMass = 2f;
            //bubbleSpawner.emitVariance = 0.5f;
            //bubbleSpawner.initialVelocity = (Vector2)new Vector2f(0, 1);

            ElementDropper elementDropper = go.AddComponent<ElementDropper>();
            elementDropper.emitMass = 10f;
            elementDropper.emitTag = element_output.CreateTag();
            elementDropper.emitOffset = new Vector3(0.0f, 0.0f, 0.0f);

            ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
            elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
            {
        new ElementConverter.ConsumedElement(element_input.CreateTag(), 0.2f)
            };
            elementConverter.outputElements = new ElementConverter.OutputElement[1]
            {
        new ElementConverter.OutputElement(0.2f, element_output, 0.0f, true, true, 0.0f, 0.5f, 1f, byte.MaxValue, 0)
            };
        }
    }
}
