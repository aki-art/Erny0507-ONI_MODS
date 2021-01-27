using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PipMorphs
{
    class SpringBabyPip : IEntityConfig
    {
        public GameObject CreatePrefab()
        {
            GameObject baby = SpringAdultPip.CreateCritter(
                SpringAdultPip.BabyId,
                SpringAdultPip.BabyName,
                SpringAdultPip.BabyDescription,
                "baby_squirrel_spring_kanim",
                true);
            EntityTemplates.ExtendEntityToBeingABaby(baby, SpringAdultPip.ID, null);
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