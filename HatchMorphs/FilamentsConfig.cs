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
        public static string Name = "Floral filaments";
        public static string Description = "Produced by floral hatches. Edible and useful in medicine.";
        public GameObject CreatePrefab()
        {
            var entity = EntityTemplates.CreateLooseEntity(
                id: Id,
                name: UI.FormatAsLink(Name, Id),
                desc: Description,
                mass: 1f,
                unitMass: false,
                anim: Assets.GetAnim("filaments_kanim"),
                initialAnim: "object",
                sceneLayer: Grid.SceneLayer.Front,
                collisionShape: EntityTemplates.CollisionShape.RECTANGLE,
                width: 0.70f,
                height: 0.80f,
                isPickupable: true);

            var foodInfo = new EdiblesManager.FoodInfo(
                id: Id,
                dlcId: DlcManager.VANILLA_ID,
                caloriesPerUnit: 1000f,
                quality: TUNING.FOOD.FOOD_QUALITY_MEDIOCRE,
                preserveTemperatue: 255.15f,
                rotTemperature: 277.15f,
                spoilTime: TUNING.FOOD.SPOIL_TIME.SLOW,
                can_rot: true);

            var foodEntity = EntityTemplates.ExtendEntityToFood(entity, foodInfo);



            return foodEntity;
        }

        public string GetDlcId()
        {
            return DlcManager.VANILLA_ID;
        }
        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }
        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }

    }
}
