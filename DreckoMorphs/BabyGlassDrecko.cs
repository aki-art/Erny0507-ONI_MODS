using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DreckoMorphs
{
    class BabyGlassDrecko : IEntityConfig
    {
        public GameObject CreatePrefab()
        {
            GameObject baby = GlassDrecko.CreateCritter(
                GlassDrecko.BabyId,
                GlassDrecko.BabyName,
                GlassDrecko.BabyDescription,
                "baby_glass_drecko_kanim",
                true);
            EntityTemplates.ExtendEntityToBeingABaby(baby, GlassDrecko.ID, null);
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