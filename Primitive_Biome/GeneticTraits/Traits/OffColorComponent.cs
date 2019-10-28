using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using KSerialization;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Primitive_Biome.GeneticTraits.Traits
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class OffColorComponent : KMonoBehaviour, ISaveLoadable

    {
        [Serialize]
        public bool isSet = false;
        [Serialize]
        public Color color;
        /*protected override void OnPrefabInit()
        {
           // base.OnPrefabInit();
            Debug.Log("Should be applying");
        }
        protected override void OnLoadLevel()
        {
            Debug.Log("Should be applying2222");
            ApplyColor();
        }
*/

        protected override void OnSpawn()
        {
            Debug.Log("OnSpawn");
            //base.OnSpawn();
            if (isSet)
            {
                Debug.Log("Applying colors");
                ApplyColor();

            }
            
        }
        [OnDeserialized]
        internal void OnDeserialized()
        {
            Debug.Log("OnDeserialized");
            if (isSet)
            {
                Debug.Log("Applying colors");
                ApplyColor();

            }
            
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
        public void ApplyColor()
        {
            UtilPB.ApplyTint(this.gameObject, color);
        }
    }
}
