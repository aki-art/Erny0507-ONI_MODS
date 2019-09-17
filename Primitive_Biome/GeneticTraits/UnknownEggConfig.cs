using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Primitive_Biome.GeneticTraits
{
    class UnknownEggConfig : IEntityConfig
    {

       public static string ID = "UnknownEgg";
        public GameObject CreatePrefab()
        {
            //GameObject prefab = null;
            string eggId = "UnknownEgg";
            string eggName = "Unknown Egg";
            string eggDesc = "Unknown Egg";
            string egg_anim = "floral_hatch_egg_kanim";
            float egg_mass = 1f;
            string baby_id = BabyHatchConfig.ID;
            float fertility_cycles = 20.0f;
            float incubation_cycles = 1f;
            List<FertilityMonitor.BreedingChance> egg_chances = null;
            int eggSortOrder = 999;
            bool is_ranchable = false;
            bool add_fish_overcrowding_monitor = false;
            bool add_fixed_capturable_monitor = true;
            float egg_anim_scale = 1f;
            //FertilityMonitor.Def def = prefab.AddOrGetDef<FertilityMonitor.Def>();
            //def.baseFertileCycles = fertility_cycles;
            DebugUtil.DevAssert(eggSortOrder > -1, "Added a fertile creature without an egg sort order!");
            float base_incubation_rate = (float)(100.0 / (600.0 * (double)incubation_cycles));
            GameObject egg = EggConfig.CreateEgg(eggId, eggName, eggDesc, (Tag)baby_id, egg_anim, egg_mass, eggSortOrder, base_incubation_rate);
            //def.eggPrefab = new Tag(eggId);
            //def.initialBreedingWeights = egg_chances;
            if ((double)egg_anim_scale != 1.0)
            {
                KBatchedAnimController component = egg.GetComponent<KBatchedAnimController>();
                component.animWidth = egg_anim_scale;
                component.animHeight = egg_anim_scale;
            }
            KPrefabID egg_prefab_id = egg.GetComponent<KPrefabID>();
            return egg;
        }
            public void OnPrefabInit(GameObject inst)
        {
           
        }

        public void OnSpawn(GameObject inst)
        {
          
        }
    }
   
}
