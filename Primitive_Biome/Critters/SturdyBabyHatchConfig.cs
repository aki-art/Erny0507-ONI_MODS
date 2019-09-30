using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Primitive_Biome
{
    public class SturdyBabyHatchConfig : IEntityConfig
    {

        public GameObject CreatePrefab()
        {
            GameObject hatch = SturdyHatchConfig.CreateHatch(
                SturdyHatchConfig.BabyId,
                SturdyHatchConfig.BabyName,
                SturdyHatchConfig.BabyDescription,
                "diamond_hatch_baby_kanim",
                true);
            EntityTemplates.ExtendEntityToBeingABaby(hatch, SturdyHatchConfig.Id, null);
            return hatch;
        }

        public void OnPrefabInit(GameObject prefab)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }

}
