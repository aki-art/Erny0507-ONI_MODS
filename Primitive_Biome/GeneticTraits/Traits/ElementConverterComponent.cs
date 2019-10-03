using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSerialization;
using UnityEngine;
namespace Primitive_Biome.GeneticTraits.Traits
{
    [SerializationConfig(MemberSerialization.OptIn)]
    class ElementConverterComponent : KMonoBehaviour, ISaveLoadable

    {
    [Serialize]
        public bool isSet=false;
        [Serialize]
        public bool isGood=true;
         [Serialize]
    public SimHashes element_input;
     [Serialize]
        public SimHashes element_output;
  protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

        }
        protected void OnSpawn(GameObject go)
        {
        if(!isSet){
        randomizeColor()
        isSet=true;
        }
               UtilPB.ApplyTint(go, color);
        }
        protected override void OnCleanUp()
        {
            base.OnCleanUp();
        }
        private void setConfiguration(){
        List<SimHashes>  Possible_Inputs;
        List<SimHashes> Possible_Outputs ;
        if(isGood){
        Possible_Inputs
        }
       var colors=OffColor.colors;
        Util.Shuffle(colors);
                    color = colors.First();
        }
    }
}
