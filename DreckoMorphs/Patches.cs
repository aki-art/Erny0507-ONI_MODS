using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Klei.AI;
using TUNING;
using UnityEngine;
namespace DreckoMorphs
{
    class Patches : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            harmony.PatchAll();
        }
            
    }
}
