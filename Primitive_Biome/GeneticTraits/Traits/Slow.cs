using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei.AI;
using UnityEngine;
namespace Primitive_Biome.GeneticTraits.Traits
{
    class Slow : GeneticTraitBuilder
    {
        public override string ID => "CritterSlow";
        public override string Name => "Slow";
        public override string Description => "Moves at half the speed.";

        public override Group Group => Group.SpeedGroup;

        protected override void Init()
        {
            UtilPB.CreateTrait(ID, Name, Description,
              on_add: delegate (GameObject go)
              {
                  var navigator = go.GetComponent<Navigator>();
                  if (navigator != null)
                  {
                      navigator.defaultSpeed /= 2f;
                  }
              },
              positiveTrait: false
            );
        }
    }
}