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
        if(!isSet){
        randomizeColor()
        }
               UtilPB.ApplyTint(go, color);
        }
        protected override void OnCleanUp()
        {
            base.OnCleanUp();
        }
        private void randomizeColor(){
       var colors=OffColor.colors;
        Util.Shuffle(colors);
                    color = colors.First();
        }
    }
}
