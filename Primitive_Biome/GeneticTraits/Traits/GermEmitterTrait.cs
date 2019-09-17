using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Primitive_Biome.GeneticTraits.Traits
{
    class GermEmitterTrait : GeneticTraitBuilder
    {
        public override string ID => "GermEmitterTrait";
        public override string Name => "GermEmitterTrait";
        public override string Description => "It produces germs";

        public override Group Group => Group.SpeedGroup;
        public override bool Positive => true;
        public List<string> germs=new List<string>(){
        PollenGerms.ID, FoodPoison.ID,Spores.ID,SlimeSlung.ID
        };
        protected override void ApplyTrait(GameObject go)
        {
            
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
