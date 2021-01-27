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

        public void OnPrefabInit(GameObject prefab)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}