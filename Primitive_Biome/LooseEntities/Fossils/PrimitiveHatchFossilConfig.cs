using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Primitive_Biome.LooseEntities.Fossils
{
    class PrimitiveHatchFossilConfig : IEntityConfig
    {
        public GameObject CreatePrefab()
        {
            string id = ID;
            string name = "Primitive Hatch Fossil";
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

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }

        public const string ID = "PrimitiveHatchFossil";

        public static readonly Tag TAG = TagManager.Create("BarkSkin");
    }
}