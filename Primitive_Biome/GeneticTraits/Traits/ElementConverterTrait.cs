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

        public override Group Group => Group.ElementEmitterGroup;

        public override bool Positive => throw new NotImplementedException();

        public SimHashes element_input = SimHashes.Oxygen;
        public SimHashes element_output = SimHashes.Oxygen;
        public static List<Tag> Solids = new List<Tag>(){
SimHashes.Lime.CreateTag(),
SimHashes.Fertilizer.CreateTag(),
SimHashes.Clay.CreateTag(),
SimHashes.IronOre.CreateTag(),
SimHashes.AluminumOre.CreateTag(),
SimHashes.Wolframite.CreateTag(),
SimHashes.Snow.CreateTag(),
};
        public static List<Tag> Liquids = new List<Tag>(){
SimHashes.Water.CreateTag(),
SimHashes.DirtyWater.CreateTag(),
SimHashes.Brine.CreateTag(),
SimHashes.LiquidCarbonDioxide.CreateTag(),
SimHashes.Ethanol.CreateTag()
};
        public static List<Tag> Gases = new List<Tag>(){
SimHashes.Hydrogen.CreateTag(),
SimHashes.Methane.CreateTag(),
SimHashes.Oxygen.CreateTag(),
SimHashes.Chlorine.CreateTag(),
SimHashes.CarbonDioxide.CreateTag(),
SimHashes.ContaminatedOxygen.CreateTag(),
};
        protected override void Init()
        {

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

            go.AddComponent<Storage>().capacityKg = 10f;
            ElementConsumer elementConsumer = (ElementConsumer)go.AddOrGet<PassiveElementConsumer>();
            elementConsumer.elementToConsume = SimHashes.DirtyWater;
            elementConsumer.consumptionRate = 0.2f;
            elementConsumer.capacityKG = 10f;
            elementConsumer.consumptionRadius = (byte)3;
            elementConsumer.showInStatusPanel = true;
            elementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f, 0.0f);
            elementConsumer.isRequired = false;
            elementConsumer.storeOnConsume = true;
            elementConsumer.showDescriptor = false;
            go.AddOrGet<UpdateElementConsumerPosition>();
            BubbleSpawner bubbleSpawner = go.AddComponent<BubbleSpawner>();
            bubbleSpawner.element = SimHashes.Water;
            bubbleSpawner.emitMass = 2f;
            bubbleSpawner.emitVariance = 0.5f;
            bubbleSpawner.initialVelocity = (Vector2)new Vector2f(0, 1);

            ElementDropper elementDropper = go.AddComponent<ElementDropper>();
            elementDropper.emitMass = 10f;
            elementDropper.emitTag = new Tag("Clay");
            elementDropper.emitOffset = new Vector3(0.0f, 0.0f, 0.0f);

            ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
            elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
            {
        new ElementConverter.ConsumedElement(SimHashes.DirtyWater.CreateTag(), 0.2f)
            };
            elementConverter.outputElements = new ElementConverter.OutputElement[1]
            {
        new ElementConverter.OutputElement(0.2f, SimHashes.Water, 0.0f, true, true, 0.0f, 0.5f, 1f, byte.MaxValue, 0)
            };
        }
    }
}
