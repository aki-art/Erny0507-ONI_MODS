using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DreckoMorphs
{
    class BabyMetalDrecko : IEntityConfig
    {
        public GameObject CreatePrefab()
        {
            GameObject baby = MetalDrecko.CreateCritter(
                MetalDrecko.BabyId,
                MetalDrecko.BabyName,
                MetalDrecko.BabyDescription,
                "baby_metal_drecko_kanim",
                true);
            EntityTemplates.ExtendEntityToBeingABaby(baby, MetalDrecko.ID, null);
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