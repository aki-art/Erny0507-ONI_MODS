using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSerialization;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Primitive_Biome.GeneticTraits
{
    [SerializationConfig(MemberSerialization.OptIn)]
    class ColorHolderComponent : KMonoBehaviour, ISaveLoadable
    {
        //[Serialize]
        //public float r, g, b, a;
        [Serialize]
        public Color color;
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
           
        }
        protected override void OnSpawn()
        {
            base.OnSpawn();
        }
        protected override void OnCleanUp()
        {
            base.OnCleanUp();
        }
    }
}
