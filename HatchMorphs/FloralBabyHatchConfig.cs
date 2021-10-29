using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HatchMorphs
{
    class FloralBabyHatchConfig : IEntityConfig
    {

        public GameObject CreatePrefab()
        {
            GameObject hatch = FloralHatchConfig.CreateHatch(
                FloralHatchConfig.BabyId,
                FloralHatchConfig.BabyName,
                FloralHatchConfig.BabyDescription,
                "floral_hatch_baby_kanim",
                true);
            EntityTemplates.ExtendEntityToBeingABaby(hatch, FloralHatchConfig.Id, null);
            return hatch;
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