using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Primitive_Biome.GeneticTraits.Traits
{
    class Fertile : GeneticTraitBuilder
    {
        public override string ID => "CritterFertile";
        public override string Name => "Fertile";
        public override string Description => "Is very fertile, fertility is improved by 25%.";

        public override Group Group => Group.FertilityGroup;

        protected override void Init()
        {
            UtilPB.CreateTrait(ID, Name, Description,
              on_add: delegate (GameObject go)
              {
                  var modifiers = go.GetComponent<Modifiers>();
                  if (modifiers != null)
                  {
                      modifiers.attributes.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, 0.25f, Description, is_multiplier: true));
                  }
              },
              positiveTrait: true
            );
        }
    }
}