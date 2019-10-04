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
        public abstract bool Positive { get; }
        public abstract bool CustomDescription { get; }
        protected abstract void Init();

        public void CreateTrait()
        {
            Init();
        }
        protected abstract void ApplyTrait(GameObject go);
        protected void ChooseTarget(GameObject go)
        {
            if (Group.OnlyEgg)
            {
                if (go.GetComponent<GeneticTraitComponent>().IsEgg() )
                {
                    ApplyTrait(go);
                }
            }
            else
            {
                ApplyTrait(go);
            }
            
        }
        public abstract void SetConfiguration(GameObject to, GameObject from);
        public new string ToString => ID;
    }
}
