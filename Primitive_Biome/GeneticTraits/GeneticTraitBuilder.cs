using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei.AI;
using KSerialization;
using UnityEngine;

namespace Primitive_Biome.GeneticTraits
{
    abstract class GeneticTraitBuilder
    {
        public abstract string ID { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }

        public abstract Group Group { get; }

        protected abstract void Init();

        public void CreateTrait()
        {
            Init();
        }
    }
}
