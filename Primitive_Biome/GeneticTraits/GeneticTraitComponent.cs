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
       // [SerializeField]
       // [Serialize]
        //private List<GeneticTraitBuilder> traits = new List<GeneticTraitBuilder> ();
        private static readonly EventSystem.IntraObjectHandler<GeneticTraitComponent> OnSpawnedFromDelegate =
              new EventSystem.IntraObjectHandler<GeneticTraitComponent>(OnSpawnedFrom);

        private static readonly EventSystem.IntraObjectHandler<GeneticTraitComponent> OnLayEggDelegate =
          new EventSystem.IntraObjectHandler<GeneticTraitComponent>(OnLayEgg);

        protected override void OnPrefabInit()
        {
            GeneticTraits.InitAllTraits();
            gameObject.Subscribe((int)GameHashes.SpawnedFrom, from => transferTraits(from as GameObject));
            Subscribe((int)GameHashes.SpawnedFrom, OnSpawnedFromDelegate);
            Subscribe((int)GameHashes.LayEgg, OnLayEggDelegate);
        }

        protected override void OnSpawn()
        {
           
               // var traitsToAdd = GeneticTraits.ChooseTraits(gameObject).Select(Db.Get().traits.Get);
               // addTraits(traitsToAdd);


        }

        // Transfer critter traits owned by the `from` object to this object
        private void transferTraits(GameObject from)
        {
            var fromTraits = from.GetComponent<Klei.AI.Traits>();
            if (fromTraits == null) return;

            var traitsToAdd = fromTraits.TraitList.Where(GeneticTraits.IsSupportedTrait);
            addTraits(traitsToAdd);

        }

        // Adds the provided list of traits to this object's Traits component
        public void addTraits(IEnumerable<Trait> traitsToAdd)
        {
            if (IsEgg())
            {
                var traits = gameObject.AddOrGet<AIGeneticTraits>();
                traitsToAdd.ToList().ForEach(traits.Add);
            }
            else
            {
                var traits = gameObject.AddOrGet<Klei.AI.Traits>();
                traitsToAdd.ToList().ForEach(traits.Add);
            }

        }
        //
        private static void OnSpawnedFrom(GeneticTraitComponent componentChild, object data)
        {
            var parent = (data as GameObject);
            Debug.Log("OnSpawnedFrom");
            Debug.Log(parent);
            /*if(parent.IsCritter())
            {
                    //heredar
                }
            if(parent==machine){
            //rollnewtraits
            }*/
            (data as GameObject).GetComponent<GeneticTraitComponent>()?.TransferTo(componentChild);
        }

        private static void OnLayEgg(GeneticTraitComponent component, object data)
        {
            Debug.Log("OnLayEgg");
            var egg = (data as GameObject);
            Debug.Log(egg);
            //pass traits
            Debug.Log(egg.GetComponent<GeneticTraitComponent>());
            component.TransferTo((data as GameObject).GetComponent<GeneticTraitComponent>());
        }

        public bool IsCritter()
        {
            return this.HasTag(GameTags.Creature);
        }

        public bool IsEgg()
        {
            return this.HasTag(GameTags.Egg);
        }
        public bool IsBaby()
        {
            if (IsEgg())
            {
                return false;
            }
            else
            {
                if (gameObject.GetDef<BabyMonitor.Def>() != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsAdult()
        {
            if (IsEgg())
            {
                return false;
            }
            else
            {
                if (IsBaby())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public void SetName(string newName)
        {

        }

        private void setGameObjectName(string newName)
        {

        }

        public void ApplyName()
        {

        }

        public bool HasName()
        {
            return false;
        }

        public void TransferTo(GeneticTraitComponent componentChild)
        {
            if (componentChild.IsEgg())
            {
                Debug.Log("Component to pass on is an egg");
                if (componentChild == null) return;

                var fromTraits = GetComponent<Klei.AI.Traits>();
                if (fromTraits == null) return;

                var traitsToAdd = fromTraits.TraitList.Where(GeneticTraits.IsSupportedTrait);
                Debug.Log("traitsToAdd");

                componentChild.addTraits(traitsToAdd);
            }
            else
            {
                if (componentChild.IsBaby())
                {
                    if (componentChild == null) return;

                    var fromTraits = GetComponent<AIGeneticTraits>();
                    if (fromTraits == null) return;

                    var traitsToAdd = fromTraits.TraitList.Where(GeneticTraits.IsSupportedTrait);
                    componentChild.addTraits(traitsToAdd);
                }
                else
                {
                    if (componentChild == null) return;

                    var fromTraits = GetComponent<Klei.AI.Traits>();
                    if (fromTraits == null) return;

                    var traitsToAdd = fromTraits.TraitList.Where(GeneticTraits.IsSupportedTrait);
                    componentChild.addTraits(traitsToAdd);
                }

            }

        }

     
    }
}

