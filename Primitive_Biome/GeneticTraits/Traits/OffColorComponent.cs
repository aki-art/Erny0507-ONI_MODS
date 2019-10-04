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
        public bool isSet = false;
        [Serialize]
        public Color color;
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

        }
        protected override void OnSpawn()
        {
            base.OnSpawn();
            if (!isSet)
            {
                Debug.Log("is NO set");
                setConfiguration(gameObject);
                
            }
            Debug.Log("Applying colors");
            UtilPB.ApplyTint(gameObject, color);
        }
        protected override void OnCleanUp()
        {
            base.OnCleanUp();
        }
        public void setConfiguration(GameObject go)
        {
            Debug.Log("Configurating");
            var colors = OffColor.colors;
            Util.Shuffle(colors);
            color = colors.First();
            isSet = true;
        }
    }
}
