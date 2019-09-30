using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Primitive_Biome.Fossils
{
    class PrimitiveHatchFossilConfig : IEntityConfig
    {
        public GameObject CreatePrefab()
        {
            string id = ID;
            string name = "Tooth Fossil";//"Primitive Hatch Fossil";
            string desc = "Primitive Hatch Fossil";
            float mass = 1f;
            bool unitMass = false;
            KAnimFile anim = Assets.GetAnim("bark_skin_kanim");
            string initialAnim = "object";
            Grid.SceneLayer sceneLayer = Grid.SceneLayer.Front;
            EntityTemplates.CollisionShape collisionShape = EntityTemplates.CollisionShape.CIRCLE;
            float width = 0.35f;
            float height = 0.35f;
            bool isPickupable = true;
            List<Tag> additionalTags = new List<Tag>
        {
            GameTags.IndustrialIngredient,
            GameTags.Organics,
            TagManager.Create("FossilStone")
        };
            GameObject gameObject = EntityTemplates.CreateLooseEntity(id, name, desc, mass, unitMass, anim, initialAnim, sceneLayer, collisionShape, width, height, isPickupable, 0, SimHashes.Creature, additionalTags);
            gameObject.AddOrGet<EntitySplitter>();
            gameObject.AddOrGet<SimpleMassStatusItem>();

            var input = new[] { new ComplexRecipe.RecipeElement(HatchConfig.EGG_ID, 1f), new ComplexRecipe.RecipeElement(RawEggConfig.ID, 1f) };
            var output = new[] { new ComplexRecipe.RecipeElement(SturdyHatchConfig.EGG_ID, 1f) };
            var fabricatorId = Buildings.GeneticSamplerConfig.ID;
            var recipeId = ComplexRecipeManager.MakeRecipeID(fabricatorId, input, output);
            var Recipe = new ComplexRecipe(recipeId, input, output)
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

            return gameObject;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }

        public const string ID = "PrimitiveHatchFossil";
        public static readonly Tag TAG = TagManager.Create(ID);
        public const string RecipeDescription = "PrimitiveHatchFossil";

    }
}