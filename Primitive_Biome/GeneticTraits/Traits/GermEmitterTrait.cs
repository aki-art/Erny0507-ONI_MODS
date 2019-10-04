using Klei.AI;
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
        public override string Name => "Infectuos";
        public override string Description => "It produces germs";

        public override Group Group => Group.GermEmitterGroup;
        public override bool CustomDescription => true;
        public override bool Positive => false;
        public List<string> germs = new List<string>(){
        //PollenGerms.ID,
            FoodGerms.ID,ZombieSpores.ID,SlimeGerms.ID
        };
        protected override void ApplyTrait(GameObject go)
        {
           
            
            if (//go.GetComponent<GeneticTraitComponent>().IsBaby() || 
               go.GetComponent<GeneticTraitComponent>().IsAdult()
              // ||go.GetComponent<GeneticTraitComponent>().IsEgg()
               )
            { Util.Shuffle(germs);
            var reduction_factor = 4;
                var germ = Db.Get().Diseases.Get(germs.First());
                var string_holder = go.AddComponent<StringHolderComponent>();
                string_holder.text = "This critter skin produces small quantities of " + germ.Name + "";
                string_holder.id = ID;
                DiseaseDropper.Def def = go.AddOrGetDef<DiseaseDropper.Def>();
                def.diseaseIdx = Db.Get().Diseases.GetIndex(germs.First()); ;
            def.singleEmitQuantity = 0;
            def.averageEmitPerSecond = 5000/ reduction_factor;
            def.emitFrequency = 5f;
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
        public override void SetConfiguration(GameObject to, GameObject from)
        {
            var t = to.AddComponent<ElementConverterComponent>();
            t.setConfiguration(to);
        }
    }
}
