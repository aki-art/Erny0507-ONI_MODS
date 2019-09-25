using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HatchMorphs
{
    class BarkSkinConfig : IEntityConfig
    {
        public GameObject CreatePrefab()
        {
            string id = "BarkSkin";
            string name = "Bark Skin";
            string desc = "Are produced by wooden hatch";
            float mass = 1f;//too litle, add more later
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
            GameTags.Organics
        };
            GameObject gameObject = EntityTemplates.CreateLooseEntity(id, name, desc, mass, unitMass, anim, initialAnim, sceneLayer, collisionShape, width, height, isPickupable, 0, SimHashes.Creature, additionalTags);
            gameObject.AddOrGet<EntitySplitter>();
            gameObject.AddOrGet<SimpleMassStatusItem>();
            return gameObject;
        }

        // Token: 0x06005F74 RID: 24436 RVA: 0x001D4924 File Offset: 0x001D2D24
        public void OnPrefabInit(GameObject inst)
        {
        }

        // Token: 0x06005F75 RID: 24437 RVA: 0x001D4926 File Offset: 0x001D2D26
        public void OnSpawn(GameObject inst)
        {
        }

        // Token: 0x040066F3 RID: 26355
        public const string ID = "BarkSkin";

        // Token: 0x040066F4 RID: 26356
        public static readonly Tag TAG = TagManager.Create("BarkSkin");

    }
}
