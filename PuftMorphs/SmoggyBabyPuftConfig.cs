using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STRINGS;
using UnityEngine;

namespace PuftMorphs
{
    class SmoggyBabyPuftConfig : IEntityConfig
    {
        public GameObject CreatePrefab()
        {
            GameObject puft = SmoggyPuftConfig.CreatePuft(SmoggyPuftConfig.BABY_ID, SmoggyPuftConfig.BabyName,
                SmoggyPuftConfig.BabyDescription,
              "Smoggy_puft_baby_kanim"
                 // "baby_puft_kanim"
                , true);
            EntityTemplates.ExtendEntityToBeingABaby(puft, (Tag)SmoggyPuftConfig.ID, (string)null);
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