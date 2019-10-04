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
        public static string ID_Type = "ElementConverter";
        public static List<SimHashes> Possible_Inputs = new List<SimHashes>(){
            SimHashes.Hydrogen,
            SimHashes.Methane,
            SimHashes.Oxygen,

            };
        public static List<SimHashes> Possible_Outputs = new List<SimHashes>(){

                SimHashes.ChlorineGas,
                SimHashes.CarbonDioxide,
                SimHashes.ContaminatedOxygen,
                SimHashes.DirtyWater,
                SimHashes.Brine,
                SimHashes.LiquidCarbonDioxide,
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


        }
        public override void SetConfiguration(GameObject to, GameObject from)
        {
            var t = to.AddComponent<ElementConverterComponent>();
            t.setConfiguration(to);
        }
    }
}
