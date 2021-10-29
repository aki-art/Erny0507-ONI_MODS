using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HatchMorphs
{
    class MendingSerumConfig : IEntityConfig
    {
        public const string ID = "MendingSerum";
        public static string Name = UI.FormatAsLink("Mending Serum", ID);
        public const string Description = "Provides a constant healing booster.";
        public const string Effect_ = "Mending_Regeneration";
        public GameObject CreatePrefab()
        {

            GameObject looseEntity = EntityTemplates.CreateLooseEntity(ID, Name, Description, 1f, true, Assets.GetAnim((HashedString)"mending_serum_kanim"),
                "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, (List<Tag>)null);
            MedicineInfo medicine_info = new MedicineInfo(ID.ToLower(), Effect_, MedicineInfo.MedicineType.Booster, "AdvancedDoctorStation",(string[])null);
            EntityTemplates.ExtendEntityToMedicine(looseEntity, medicine_info);
            looseEntity.GetComponent<KPrefabID>().AddTag(GameTags.MedicalSupplies, false);
            ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[3]
            {
      new ComplexRecipe.RecipeElement( (Tag)RawEggConfig.ID, 3f),
      new ComplexRecipe.RecipeElement((Tag) NectarConfig.Id, 10f),
          new ComplexRecipe.RecipeElement((Tag) SwampLilyFlowerConfig.ID, 1f)
            };
            ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
            {
      new ComplexRecipe.RecipeElement((Tag) ID, 1f)
            };
            MendingSerumConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", (IList<ComplexRecipe.RecipeElement>)ingredients, (IList<ComplexRecipe.RecipeElement>)results), ingredients, results, 0)
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

        public string GetDlcId()
        {
            return DlcManager.VANILLA_ID;
        }
        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public static ComplexRecipe recipe;
    }
}
