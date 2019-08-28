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
        // Token: 0x060060C1 RID: 24769 RVA: 0x001DADF0 File Offset: 0x001D91F0
        public GameObject CreatePrefab()
        {
            GameObject gameObject = EntityTemplates.CreateLooseEntity("FloralAntihistamine", ITEMS.PILLS.ANTIHISTAMINE.NAME, ITEMS.PILLS.ANTIHISTAMINE.DESC, 1f, true, Assets.GetAnim("pill_allergies_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
            EntityTemplates.ExtendEntityToMedicine(gameObject, MEDICINE.ANTIHISTAMINE);
            ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
            {
            new ComplexRecipe.RecipeElement("PrickleFlowerSeed", 1f),
            new ComplexRecipe.RecipeElement(SimHashes.Dirt.CreateTag(), 1f)
            };
            ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
            {
            new ComplexRecipe.RecipeElement("Antihistamine", 10f)
            };
            string id = ComplexRecipeManager.MakeRecipeID("Apothecary", array, array2);
            FloralAntihistamineConfig.recipe = new ComplexRecipe(id, array, array2)
            {
                time = 100f,
                description = ITEMS.PILLS.ANTIHISTAMINE.RECIPEDESC,
                nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
                fabricators = new List<Tag>
            {
                "Apothecary"
            },
                sortOrder = 10
            };
            return gameObject;
        }

        // Token: 0x060060C2 RID: 24770 RVA: 0x001DAF18 File Offset: 0x001D9318
        public void OnPrefabInit(GameObject inst)
        {
        }

        // Token: 0x060060C3 RID: 24771 RVA: 0x001DAF1A File Offset: 0x001D931A
        public void OnSpawn(GameObject inst)
        {
        }

        // Token: 0x04006767 RID: 26471
        public const string ID = "Antihistamine";

        // Token: 0x04006768 RID: 26472
        public static ComplexRecipe recipe;
    }
}
