using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STRINGS;
using UnityEngine;
namespace PuftMorphs
{
    class DevilBabyPuftConfig : IEntityConfig
    {
        public GameObject CreatePrefab()
        {
            GameObject puft = DevilPuftConfig.CreatePuft(DevilPuftConfig.BABY_ID, DevilPuftConfig.BabyName,
                DevilPuftConfig.BabyDescription,
                "devil_puft_baby_kanim", true);
            EntityTemplates.ExtendEntityToBeingABaby(puft, (Tag)DevilPuftConfig.ID, (string)null);
            return puft;
        }

        public string GetDlcId()
        {
            return DlcManager.VANILLA_ID;
        }
        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }
        public void OnPrefabInit(GameObject prefab)
        {
        }
        public void OnSpawn(GameObject inst)
        {
        }
    }
}