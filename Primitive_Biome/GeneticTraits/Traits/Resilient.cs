using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Primitive_Biome.GeneticTraits.Traits
{
    class Resilient : GeneticTraitBuilder
    {
        public override string ID => "Resilient";
        public override string Name => "Resilient";
        public override string Description => "Can live in a wider range of temperatures";

        public override Group Group => Group.TemperatureSensitivityGroup;
        public override bool Positive => false;
        public override bool CustomDescription => false;

        protected override void ApplyTrait(GameObject go)
        {
            var temperatureVulnerable = go.GetComponent<TemperatureVulnerable>();
            if (temperatureVulnerable != null)
            {
                temperatureVulnerable.Configure(temperatureVulnerable.internalTemperatureWarning_Low * 0.75f,
                    temperatureVulnerable.internalTemperatureLethal_Low * 0.75f,
                    temperatureVulnerable.internalTemperatureLethal_High * 1.25f,
                    temperatureVulnerable.internalTemperatureWarning_High * 1.25f);
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
           
        }
    }
}