using Klei.AI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Primitive_Biome.GeneticTraits.Traits
{
    class OffColor : GeneticTraitBuilder
    {
        public override string ID => "OffColor";
        public override string Name => "Off Color";
        public override string Description => "It has an unusual color";

        public override Group Group => Group.GermEmitterGroup;
        public override bool CustomDescription => false;
        public override bool Positive => true;
        public List<Color> colors = new List<Color>(){
            new Color (130/255f,255/255f,130/255f, 1f), //Color.green
            new Color (130/255f,130/255f,255/255f, 1f),//Color.blue,
          new Color (255/255f,128/255f,128/255f, 1f), //Color.red
            new Color (255/255f,255/255f,130/255f, 1f),//Color.yellow,
           new Color (130/255f,255/255f,255/255f, 1f),// Color.cyan,
           new Color (255/255f,130/255f,255/255f, 1f),// Color.magenta

        };
        public Color color = Color.white;
        public static string id_color = "ColorTint";
        protected override void ApplyTrait(GameObject go)
        {


            if (go.GetComponent<GeneticTraitComponent>().IsBaby() ||
               go.GetComponent<GeneticTraitComponent>().IsAdult()
               || go.GetComponent<GeneticTraitComponent>().IsEgg()
               )
            {
                DebugHelper.Separator();
                var text_holders = go.GetComponents<ColorHolderComponent>();
                DebugHelper.LogVar(text_holders);
                var flag = true;
                if (text_holders.Length > 0)
                {
                    DebugHelper.LogVar(text_holders);
                    DebugHelper.LogForEach(text_holders);
                    var text_holder = text_holders.First();
                    if (text_holder != null)
                    {

                        Color color = text_holder.color;//UtilPB.recoverColor(text_holder);
                        DebugHelper.LogVar(color);
                        UtilPB.ApplyTint(go, color);
                        flag = false;
                    }
                }

                if(flag)
                {
                    Util.Shuffle(colors);
                    color = colors.First();
                    //color.a = color.a / 2;
                    UtilPB.ApplyTint(go, color);
                    /*var string_holder = go.AddComponent<StringHolderComponent>();

                    string_holder.text = "This critter skin has soft tint of " + color.ToString()+ "";
                    string_holder.id = ID;*/

                    var color_holder = go.AddOrGet<ColorHolderComponent>();
                    color_holder.color = color;
                    //UtilPB.setColor(color_holder, color);


                }

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
