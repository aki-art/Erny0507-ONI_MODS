using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HatchMorphs
{
    class FilamentsConfig : IEntityConfig
    {
        public static string Id = "HatchFilaments";
        public static string Name = "Floral Hatch Filaments";
        public static string Description = "Produced by floral hatches";

        public GameObject CreatePrefab()
        {
            var entity = EntityTemplates.CreateLooseEntity(
                id: Id,
                name: UI.FormatAsLink(Name, Id),
                desc: Description,
                mass: 1f,
                unitMass: false,
                anim: Assets.GetAnim("filaments"),
                initialAnim: "object",
                sceneLayer: Grid.SceneLayer.Front,
                collisionShape: EntityTemplates.CollisionShape.RECTANGLE,
                width: 0.77f,
                height: 0.48f,
                isPickupable: true);

            var foodInfo = new EdiblesManager.FoodInfo(
                id: Id,
                caloriesPerUnit: 1000f,
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
