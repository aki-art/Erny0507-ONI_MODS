using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSerialization;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Primitive_Biome.GeneticTraits.Traits
{
    [SerializationConfig(MemberSerialization.OptIn)]
    class OffColorComponent : KMonoBehaviour, ISaveLoadable

    {
        [Serialize]
        public bool isSet=false;
        [Serialize]
        public Color color;
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

        }
        protected void OnSpawn(GameObject go)
        {
            
        }
        protected override void OnCleanUp()
        {
            base.OnCleanUp();
        }
    }
}
