using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STRINGS;
using UnityEngine;

namespace PuftMorphs
{
    class CloudyBabyPuftConfig : IEntityConfig
    {
        public GameObject CreatePrefab()
        {
            GameObject puft = CloudyPuftConfig.CreatePuft(CloudyPuftConfig.BABY_ID, CloudyPuftConfig.BabyName,
                CloudyPuftConfig.BabyDescription,
                //"cloudy_puft_baby_kanim"
                "baby_puft_kanim"
                , true);
            EntityTemplates.ExtendEntityToBeingABaby(puft, (Tag)CloudyPuftConfig.ID, (string)null);
            return puft;
        }
        public void OnPrefabInit(GameObject prefab)
        {
        }
        public void OnSpawn(GameObject inst)
        {
        }
    }
}