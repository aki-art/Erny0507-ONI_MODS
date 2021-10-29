using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HatchMorphs
{
    class MacedoniaConfig : IEntityConfig
    {
        public const string Id = "Macedonia";
        public const string Name = "Macedonia";
        public static string Description = $"Delicious {UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT")} cut into pieces and \nmix with  {UI.FormatAsLink(NectarConfig.Name, NectarConfig.Id)}.";
        public static string RecipeDescription = $"{UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT")} cut and sweetened with {UI.FormatAsLink(NectarConfig.Name, NectarConfig.Id)}.";

        public ComplexRecipe Recipe;

        public GameObject CreatePrefab()
        {
            var entity = EntityTemplates.CreateLooseEntity(
                id: Id,
                name: UI.FormatAsLink(Name, Id),
                desc: Description,
                mass: 1f,
                unitMass: false,
                anim: Assets.GetAnim("macedonia_kanim"),
                initialAnim: "object",
                sceneLayer: Grid.SceneLayer.Front,
                collisionShape: EntityTemplates.CollisionShape.CIRCLE,
                width: 0.30f,
                height: 0.30f,
                isPickupable: true);

            var foodInfo = new EdiblesManager.FoodInfo(
                id: Id,
                dlcId: DlcManager.VANILLA_ID,
                caloriesPerUnit:1400000f,
                quality: TUNING.FOOD.FOOD_QUALITY_AMAZING,
                preserveTemperatue: 255.15f,
                rotTemperature: 277.15f,
                spoilTime: TUNING.FOOD.SPOIL_TIME.SLOW,
                can_rot: true);

            var food = EntityTemplates.ExtendEntityToFood(entity, foodInfo);
            var input = new[] { new ComplexRecipe.RecipeElement(NectarConfig.Id, 1f), new ComplexRecipe.RecipeElement(PrickleFruitConfig.ID, 1f) };
            var output = new[] { new ComplexRecipe.RecipeElement(MacedoniaConfig.Id, 1f) };
            var fabricatorId= CookingStationConfig.ID;
            var recipeId = ComplexRecipeManager.MakeRecipeID(fabricatorId, input, output);
            Recipe =  new ComplexRecipe(recipeId, input, output, 0)
            {
                time = 100f,
                description = RecipeDescription,
                nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
                fabricators = new List<Tag>
            {
                fabricatorId
            },
                sortOrder = 120
            };
            return food;
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
