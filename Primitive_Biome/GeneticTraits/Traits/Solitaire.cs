using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei.AI;
using UnityEngine;

namespace Primitive_Biome.GeneticTraits.Traits
{
    class Solitaire : GeneticTraitBuilder
    {
        public override string ID => "Solitaire";
        public override string Name => "Solitaire";
        public override string Description => "Needs more space for itself";

        public override Group Group => Group.HerdingGroup;
        public override bool Positive => false;
        public override bool CustomDescription => false;
        protected override void ApplyTrait(GameObject go)
        {
            var overcrowdingMonitor = go.GetDef<OvercrowdingMonitor.Def>();
            if (overcrowdingMonitor != null)
            {
                overcrowdingMonitor.spaceRequiredPerCreature = (int)(overcrowdingMonitor.spaceRequiredPerCreature *1.25f);
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