using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



namespace DreckoMorphs
{
    class BabyAlgaeDrecko : IEntityConfig
    {
        public GameObject CreatePrefab()
        {
            GameObject baby = AlgaeDrecko.CreateCritter(
                AlgaeDrecko.BabyId,
                AlgaeDrecko.BabyName,
                AlgaeDrecko.BabyDescription,
                "baby_algae_drecko_kanim",
                true);
            EntityTemplates.ExtendEntityToBeingABaby(baby, AlgaeDrecko.ID, null);
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