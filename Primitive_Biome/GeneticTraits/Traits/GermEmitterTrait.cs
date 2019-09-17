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

        public override Group Group => Group.GermEmitterGroup;
        public override bool Positive => true;
        public List<string> germs=new List<string>(){
        PollenGerms.ID, FoodPoison.ID,Spores.ID,SlimeSlung.ID
        };
        protected override void ApplyTrait(GameObject go)
        {
        Util.shuffle(germs);
             DiseaseDropper.Def def = go.AddOrGetDef<DiseaseDropper.Def>();
            def.diseaseIdx = Db.Get().Diseases.GetIndex(germs.first());
            def.emitFrequency = 1f;
            def.averageEmitPerSecond = 1000;
            def.singleEmitQuantity = 100000;
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
