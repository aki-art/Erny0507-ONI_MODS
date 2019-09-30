using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static Diet;

namespace Primitive_Biome.GeneticTraits.Traits
{
    class Inefficient : GeneticTraitBuilder
    {
        public override string ID => "Inefficient";
        public override string Name => "Inefficient";
        public override string Description => "It produces a little less than its peers";

        public override Group Group => Group.EfficiencyGroup;
        public override bool Positive => false;
        public override bool CustomDescription => false;

        protected override void ApplyTrait(GameObject go)
        {
            var creatureCalorieMonitor = go.GetDef<CreatureCalorieMonitor.Def>();
            if (creatureCalorieMonitor != null)
            {
                var infos = creatureCalorieMonitor.diet.infos;
                foreach (var info in infos)
                {


                    Traverse.Create(info)
                        //.Property("myBar")
                        .Field("producedConversionRate").SetValue(info.producedConversionRate * 0.75f);

                }

            }
        }

        protected override void Init()
        {
            UtilPB.CreateTrait(ID, Name, Description,
              on_add: delegate (GameObject go)
              {
                  ChooseTarget(go);
              },
              positiveTrait: Positive
            );
        }
    }
}