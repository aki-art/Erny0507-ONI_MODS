using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Primitive_Biome.Elements
{
    static class CyanideElement
    {
        public static string ID = "Cyanide";
        public static readonly Color32 CYANIDE_FADED_YELLOW = new Color32(242, 206, 82, 255);
        public static readonly SimHashes BloodSimHash = (SimHashes)Hash.SDBMLower("Blood");
        public static readonly SimHashes FrozenBloodSimHash = (SimHashes)Hash.SDBMLower("FrozenBlood");
 
    }
}
