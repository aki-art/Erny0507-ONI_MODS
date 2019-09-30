using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei.AI;
using UnityEngine;

namespace Primitive_Biome.GeneticTraits.Traits
{
    class Infertile : GeneticTraitBuilder
    {
        public override string ID => "CritterInfertile";
        public override string Name => "Infertile";
        public override string Description => "Is very infertile, fertility is decreased by 25%.";

        public override Group Group => Group.FertilityGroup;
        public override bool Positive => false;
        public override bool CustomDescription => false;
        protected override void ApplyTrait(GameObject go)
        {
            var modifiers = go.GetComponent<Modifiers>();
            if (modifiers != null)
            {
                modifiers.attributes.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -0.25f, Description, is_multiplier: true));
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
            ); ;
        }
    }
}