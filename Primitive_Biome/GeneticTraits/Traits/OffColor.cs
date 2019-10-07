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
        public static List<Color> colors = new List<Color>(){
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
            go.AddOrGet<OffColorComponent>();
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
            var t = to.AddOrGet<OffColorComponent>();
            var color_parent = from.GetComponent<OffColorComponent>();
            if (color_parent == null)
            {
                Debug.Log("About to config");
                t.setConfiguration(to);
            }
            else
            {
                Debug.Log("Passing on");
                t.color = color_parent.color;
                t.isSet = true;
                t.ApplyColor();
            }

        }
    }
}
