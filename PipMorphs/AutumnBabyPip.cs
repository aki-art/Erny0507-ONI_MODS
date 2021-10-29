using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PipMorphs
{
    class AutumnBabyPip : IEntityConfig
    {
        public GameObject CreatePrefab()
        {
            GameObject baby = AutumnAdultPip.CreateCritter(
                AutumnAdultPip.BabyId,
                AutumnAdultPip.BabyName,
                AutumnAdultPip.BabyDescription,
                "baby_squirrel_autumn_kanim",
                true);
            EntityTemplates.ExtendEntityToBeingABaby(baby, AutumnAdultPip.ID, null);
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