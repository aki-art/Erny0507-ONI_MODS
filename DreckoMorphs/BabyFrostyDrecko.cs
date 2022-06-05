using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DreckoMorphs
{
    class BabyFrostyDrecko : IEntityConfig
    {
        public GameObject CreatePrefab()
        {
            GameObject baby = FrostyDrecko.CreateCritter(
                FrostyDrecko.BabyId,
                FrostyDrecko.BabyName,
                FrostyDrecko.BabyDescription,
                "baby_snow_drecko_kanim",
                true);
            EntityTemplates.ExtendEntityToBeingABaby(baby, FrostyDrecko.ID, null);
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