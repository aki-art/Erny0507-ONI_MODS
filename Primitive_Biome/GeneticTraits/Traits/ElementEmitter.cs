using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Primitive_Biome.GeneticTraits.Traits
{
    class ElementEmitter : GeneticTraitBuilder
    {
        public override string ID => "ElementEmitter";
        public override string Name => "Element Emitter";
        public override string Description => "Element Emitter";

        public override Group Group => Group.ElementEmitterGroup;
        public Tag element = null;
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
SimHashes.Wolframite.CreateTag(),
SimHashes.ContaminatedOxygen.CreateTag(),
};
        protected override void Init()
        {
            UtilPB.CreateTrait(ID, Name, Description,
              on_add: delegate (GameObject go)
              {
                  var random_element = SimHashes.Hydrogen.CreateTag();
                  BubbleSpawner bubbleSpawner = go.AddComponent<BubbleSpawner>();
                  bubbleSpawner.element = SimHashes.Hydrogen;
                  bubbleSpawner.emitMass = 2f;
                  bubbleSpawner.emitVariance = 0.5f;
                  bubbleSpawner.initialVelocity = (Vector2)new Vector2f(0, 1);

              },
              positiveTrait: true
            );
        }
    }
}
