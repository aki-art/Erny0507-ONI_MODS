using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUNING;
using UnityEngine;

namespace HatchMorphs
{
    class FloralAntihistamineConfig : IEntityConfig
    {
        public const string ID = "FloralAntihistamine";
        public static string Name = UI.FormatAsLink("Floral Antihistamine", ID);
        public const string Description = "Stops the symptomes of allergies for a long period of time.";
        public const string Effect_ = "FloralHistamineSuppression";
        public GameObject CreatePrefab()
        {
           
            GameObject looseEntity = EntityTemplates.CreateLooseEntity(ID, Name, Description, 1f, true, Assets.GetAnim((HashedString)"floral_antihistamine_kanim"),
                "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, (List<Tag>)null);
            MedicineInfo medicine_info = new MedicineInfo(ID.ToLower(), Effect_, MedicineInfo.MedicineType.Booster, (string[])null);
            EntityTemplates.ExtendEntityToMedicine(looseEntity, medicine_info);
            looseEntity.GetComponent<KPrefabID>().AddTag(GameTags.MedicalSupplies, false);
            ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[2]
            {
      new ComplexRecipe.RecipeElement(SimHashes.Copper.CreateTag(), 100f),
      new ComplexRecipe.RecipeElement((Tag) FilamentsConfig.Id, 50f)
            };
            ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
            {
      new ComplexRecipe.RecipeElement((Tag) ID, 1f)
            };
            
            FloralAntihistamineConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", (IList<ComplexRecipe.RecipeElement>)ingredients, (IList<ComplexRecipe.RecipeElement>)results), ingredients, results)
            {
                time = 200f,
                description = (string)Description,
                nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
                fabricators = new List<Tag>() { (Tag)"Apothecary" },
                sortOrder = 20,
                requiredTech = AdvancedCureConfig.recipe.requiredTech// "MedicineIV"
            };
            return looseEntity;
        }
        public void OnPrefabInit(GameObject inst)
        {
        }
        public void OnSpawn(GameObject inst)
        {
        }
        public static ComplexRecipe recipe;
    }
}
