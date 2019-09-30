using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Klei.AI;
using Primitive_Biome.GeneticTraits;

namespace Primitive_Biome
{
    static class UtilPB
    {
        public static Texture2D DuplicateTexture(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
        public static void CreateTrait(string id, string name, string desc, Action<GameObject> on_add, bool positiveTrait)
        {
            Trait trait = Db.Get().CreateTrait(
              id: id,
              name: name,
              description: desc,
              group_name: null,
              should_save: true,
              disabled_chore_groups: null,
              positive_trait: positiveTrait,
              is_valid_starter_trait: false);

            trait.OnAddTrait = on_add;
        }

        /**
         * Creates a trait that modifies the object's scale (including max health and calories).
         */
        public static void CreateScaleTrait(string id, string name, string desc, float scale)
        {
            CreateTrait(id, name, desc,
              on_add: delegate (GameObject go)
              {
                  SetObjectScale(go, scale, desc);
              },
              positiveTrait: scale >= 1.0f
            );
        }
        public static void SetObjectScale(GameObject go, float scale, string description = null)
        {
            // Graphic
            var animController = go.GetComponent<KBatchedAnimController>();
            if (animController != null)
            {
                animController.animScale *= scale;
            }

            // Collision
            var boxCollider = go.GetComponent<KBoxCollider2D>();
            if (boxCollider != null)
            {
                boxCollider.size *= scale;
            }

            // HP and Calories
            var modifiers = go.GetComponent<Modifiers>();
            if (modifiers != null)
            {
                // We need to update the health here or max health will be altered without changing the current health
                var health = go.GetComponent<Health>();
                if (health != null && health.hitPoints == health.maxHitPoints)
                {
                    health.hitPoints *= scale;
                }

                modifiers.attributes.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, scale - 1.0f, description, is_multiplier: true));
                modifiers.attributes.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, scale - 1.0f, description, is_multiplier: true));
            }

            // Mass
            var primaryElement = go.GetComponent<PrimaryElement>();
            if (primaryElement != null)
            {
                primaryElement.MassPerUnit *= scale;
                primaryElement.Units = 1;
            }

            // Drops
            var butcherable = go.GetComponent<Butcherable>();
            if (butcherable != null)
            {
                // If a mod uses non-meat drops then we should take that into account
                var drops = butcherable.Drops;
                var dropId = drops.FirstOrDefault() ?? MeatConfig.ID;

                if (scale < 1.0f)
                {
                    int numDropsToRemove = (int)((1.0f - scale) / 0.25f);
                    drops = drops.Take(drops.Length - numDropsToRemove).ToArray();
                }
                else if (scale > 1.0f)
                {
                    int numDropsToAdd = (int)((scale - 1.0f) / 0.25f);
                    for (int i = 0; i < numDropsToAdd; ++i)
                    {
                        drops = drops.Append(dropId);
                    }
                }
                butcherable.SetDrops(drops);
            }
        }

        /**
         * Adds a lightBug light to the provided object with the given colour, range, and lux.
         */
        public static void AddObjectLight(GameObject go, Color color, float range, int lux)
        {
            Light2D light = go.GetComponent<Light2D>();
            if (light == null)
            {
                light = go.AddComponent<Light2D>();
                light.Color = color;
                light.overlayColour = TUNING.LIGHT2D.LIGHTBUG_OVERLAYCOLOR;
                light.Range = range;
                light.Angle = 0f;
                light.Direction = TUNING.LIGHT2D.LIGHTBUG_DIRECTION;
                light.Offset = TUNING.LIGHT2D.LIGHTBUG_OFFSET;
                light.shape = LightShape.Circle;
                light.drawOverlay = true;
                light.Lux = lux;
            }
        }
        public static void ApplyTint(GameObject go, Color color)
        {
            var kAnimBase = go.GetComponent<KAnimControllerBase>();

            kAnimBase.TintColour = color;

        }
        public static void ApplyTint(Capturable go, Color color)
        {
            var kAnimBase = go.GetComponent<KAnimControllerBase>();

            kAnimBase.TintColour = color;
            

        }
       /* public static Color recoverColor(ColorHolderComponent colorHolder)
        {
            return new Color(colorHolder.r, colorHolder.g, colorHolder.b, colorHolder.a);
        }
        public static void setColor(ColorHolderComponent colorHolder, Color color)
        {
            colorHolder.r = color.r; colorHolder.g = color.g; colorHolder.b = color.b; colorHolder.a = color.a;
        }*/
    }
}
