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
        public const string Id = "SteamedPalmeraBerry";
        public const string Name = "Steamed Palmera Berry";
        public static string Description = $"The steamed bud of a {UI.FormatAsLink(NectarConfig.Name, NectarConfig.Id)}.\n\nLong exposure to heat and exquisite cooking skills turn the toxic berry into a delicious dessert.";
        public static string RecipeDescription = $"Delicious steamed {UI.FormatAsLink(NectarConfig.Name, NectarConfig.Id)}.";

        public ComplexRecipe Recipe;

        public GameObject CreatePrefab()
        {
            var entity = EntityTemplates.CreateLooseEntity(
                id: Id,
                name: UI.FormatAsLink(Name, Id),
                desc: Description,
                mass: 1f,
                unitMass: false,
                anim: Assets.GetAnim("kukumelon_kanim"),
                initialAnim: "object",
                sceneLayer: Grid.SceneLayer.Front,
                collisionShape: EntityTemplates.CollisionShape.RECTANGLE,
                width: 0.8f,
                height: 0.7f,
                isPickupable: true);

            var foodInfo = new EdiblesManager.FoodInfo(
                id: Id,
                caloriesPerUnit:350000f,
                quality: 4,
                preserveTemperatue: 255.15f,
                rotTemperature: 277.15f,
                spoilTime: TUNING.FOOD.SPOIL_TIME.SLOW,
                can_rot: true);

            var food = EntityTemplates.ExtendEntityToFood(entity, foodInfo);
            var input = new[] { new ComplexRecipe.RecipeElement(NectarConfig.Id, 1f), new ComplexRecipe.RecipeElement(PrickleFruitConfig.ID, 1f) };
            var output = new[] { new ComplexRecipe.RecipeElement(MacedoniaConfig.Id, 1f) };
            var fabricatorId= CookingStationConfig.ID;
            var recipeId = ComplexRecipeManager.MakeRecipeID(fabricatorId, input, output);
            Recipe =  new ComplexRecipe(recipeId, input, output)
            {
                time = 100f,
                description = RecipeDescription,
                nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
                fabricators = new List<Tag>
            {
                "CookingStation"
            },
                sortOrder = 120
            };
            return food;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
