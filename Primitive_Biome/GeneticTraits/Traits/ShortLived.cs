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
        public override bool Positive => false;
        public override bool CustomDescription => false;
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
        protected override void ApplyTrait(GameObject go)
        {
            var modifiers = go.GetComponent<Modifiers>();
            if (modifiers != null)
            {
                modifiers.attributes.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, -0.20f, Description, is_multiplier: true));
            }
        }
        public override void SetConfiguration(GameObject to, GameObject from)
        {
           
        }
    }
}