using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using HarmonyLib;
using UnityEngine;
using PeterHan.PLib.Database;
namespace Codex
{
    class CodexPatches : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);

            PUtil.InitLibrary(false);
            
            PCodexManager pCodexManager = new PCodexManager();
            pCodexManager.RegisterCreatures();
            //PCodex.RegisterCreatures();
        }
    }
}