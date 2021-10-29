using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace HatchMorphs
{
    public class DiamondBabyHatchConfig : IEntityConfig
    {

        public GameObject CreatePrefab()
        {
            GameObject hatch = DiamondHatchConfig.CreateHatch(
                DiamondHatchConfig.BabyId,
                DiamondHatchConfig.BabyName,
                DiamondHatchConfig.BabyDescription,
                "diamond_hatch_baby_kanim",
                true);
            EntityTemplates.ExtendEntityToBeingABaby(hatch, DiamondHatchConfig.Id, null);
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