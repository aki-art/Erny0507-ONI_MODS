using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei.AI;
using UnityEngine;

namespace Primitive_Biome.GeneticTraits.Traits
{
    class ShortLived : GeneticTraitBuilder
    {
        public override string ID => "CritterShortLived";
        public override string Name => "Short-lived";
        public override string Description => "Has a 20% shorter lifespan.";

        public override Group Group => Group.LifespanGroup;

        protected override void Init()
        {
            UtilPB.CreateTrait(ID, Name, Description,
              on_add: delegate (GameObject go)
              {
                  var modifiers = go.GetComponent<Modifiers>();
                  if (modifiers != null)
                  {
                      modifiers.attributes.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, -0.20f, Description, is_multiplier: true));
                  }
              },
              positiveTrait: false
            );
        }
    }
}