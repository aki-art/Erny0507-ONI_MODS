using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei.AI;
using KSerialization;
using UnityEngine;

namespace Primitive_Biome.GeneticTraits
{

    public class GeneticTraitComponent : KMonoBehaviour, ISaveLoadable
    {
        [SerializeField]
        [Serialize]
        private bool appliedCritterTraits = false;
private static readonly EventSystem.IntraObjectHandler<CritterName> OnSpawnedFromDelegate =
      new EventSystem.IntraObjectHandler<CritterName>(OnSpawnedFrom);

    private static readonly EventSystem.IntraObjectHandler<CritterName> OnLayEggDelegate =
      new EventSystem.IntraObjectHandler<CritterName>(OnLayEgg);
      
        protected override void OnPrefabInit()
        {
            GeneticTraits.InitAllTraits();
            gameObject.Subscribe((int)GameHashes.SpawnedFrom, from => transferTraits(from as GameObject));
             Subscribe((int)GameHashes.SpawnedFrom, OnSpawnedFromDelegate);
      Subscribe((int)GameHashes.LayEgg, OnLayEggDelegate);
        }

        protected override void OnSpawn()
        {
            /*if (!appliedCritterTraits)
            {
                var traitsToAdd = GeneticTraits.ChooseTraits(gameObject).Select(Db.Get().traits.Get);
                addTraits(traitsToAdd);

                appliedCritterTraits = true;
            }*/

        }

        // Transfer critter traits owned by the `from` object to this object
        private void transferTraits(GameObject from)
        {
            var fromTraits = from.GetComponent<Klei.AI.Traits>();
            if (fromTraits == null) return;

            var traitsToAdd = fromTraits.TraitList.Where(GeneticTraits.IsSupportedTrait);
            addTraits(traitsToAdd);

            appliedCritterTraits = true;
        }

        // Adds the provided list of traits to this object's Traits component
        private void addTraits(IEnumerable<Trait> traitsToAdd)
        {
            var traits = gameObject.AddOrGet<Klei.AI.Traits>();
            traitsToAdd.ToList().ForEach(traits.Add);
        }
         private static void OnSpawnedFrom(CritterName component, object data)
    {
      (data as GameObject).GetComponent<CritterName>()?.TransferTo(component);
    }

    private static void OnLayEgg(CritterName component, object data)
    {
      component.TransferTo((data as GameObject).GetComponent<CritterName>());
    }

    private bool IsCritter()
    {
      return this.HasTag(GameTags.Creature);
    }

    private bool IsEgg()
    {
      return this.HasTag(GameTags.Egg);
    }
     public void SetName(string newName)
    {
      generation = 1;
      if (Util.IsNullOrWhitespace(newName))
      {
        ResetToPrefabName();
        return;
      }

      critterName = newName;
      ApplyName();
    }

    private void setGameObjectName(string newName)
    {
      KSelectable selectable = GetComponent<KSelectable>();

      name = newName;
      selectable?.SetName(newName);
      gameObject.name = newName;
    }

    public void ApplyName()
    {
      if (!IsCritter()) return;

      string newName = critterName;
      if (generation == 2)
      {
        newName += " Jr.";
      }
      else if (generation > 2)
      {
        newName += " " + Util.ToRoman(generation);
      }
      setGameObjectName(newName);
    }

    public bool HasName()
    {
      return !Util.IsNullOrWhitespace(critterName);
    }

    public void TransferTo(CritterName other)
    {
      if (other == null || !HasName()) return;

      other.critterName = critterName;
      other.generation = generation;

      if (other.IsEgg())
      {
        other.generation += 1;
      }

      other.ApplyName();
    }

    public void ResetToPrefabName()
    {
      KPrefabID prefab = GetComponent<KPrefabID>();
      if (prefab != null)
      {
        critterName = "";
        setGameObjectName(TagManager.GetProperName(prefab.PrefabTag));
      }
    }
    }
}

