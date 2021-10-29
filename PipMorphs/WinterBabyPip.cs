using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PipMorphs
{
    class WinterBabyPip : IEntityConfig
    {
        public GameObject CreatePrefab()
        {
            GameObject baby = WinterAdultPip.CreateCritter(
                WinterAdultPip.BabyId,
                WinterAdultPip.BabyName,
                WinterAdultPip.BabyDescription,
                "baby_squirrel_winter_kanim",
                true);
            EntityTemplates.ExtendEntityToBeingABaby(baby, WinterAdultPip.ID, null);
            return baby;
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