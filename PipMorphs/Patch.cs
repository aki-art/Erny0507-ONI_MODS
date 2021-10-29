using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using Klei.AI;
using TUNING;
using UnityEngine;
namespace PipMorphs
{
    class Patch : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            harmony.PatchAll();
        }
    }
}
