using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using STRINGS;

namespace HatchMorphs
{
    class NectarConfig : IEntityConfig
    {
        public static string Id = "SweetNectar";
        public static string Name = "Nectar";
        public static string Description = "So so sweet.";

        public GameObject CreatePrefab()
        {
            var entity = EntityTemplates.CreateLooseEntity(
                id: Id,
                name: UI.FormatAsLink(Name, Id),
                desc: Description,
                mass: 1f,
                unitMass: false,
                anim: Assets.GetAnim("nectar_kanim"),
                initialAnim: "object",
                sceneLayer: Grid.SceneLayer.Front,
                collisionShape: EntityTemplates.CollisionShape.RECTANGLE,
                width: 0.70f,//0.74
                height: 0.80f,
                isPickupable: true);

            var foodInfo = new EdiblesManager.FoodInfo(
                id: Id,
                caloriesPerUnit: 2000f,
                quality: TUNING.FOOD.FOOD_QUALITY_GOOD,
                preserveTemperatue: 255.15f,
                rotTemperature: 277.15f,
                spoilTime: TUNING.FOOD.SPOIL_TIME.SLOW,
                can_rot: true);

            var foodEntity = EntityTemplates.ExtendEntityToFood(entity, foodInfo);

  

            return foodEntity;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }

    }
}
