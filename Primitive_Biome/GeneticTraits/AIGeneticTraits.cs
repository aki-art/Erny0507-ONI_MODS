﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei.AI;
using KSerialization;

namespace Primitive_Biome.GeneticTraits
{
    [SerializationConfig(MemberSerialization.OptIn)]
    class AIGeneticTraits : KMonoBehaviour, ISaveLoadable
    {
        public List<Trait> TraitList = new List<Trait>();
        [Serialize]
        private List<string> TraitIds = new List<string>();

        public List<string> GetTraitIds()
        {
            return this.TraitIds;
        }

        public void SetTraitIds(List<string> traits)
        {
            this.TraitIds = traits;
        }

        protected override void OnSpawn()
        {
            foreach (string traitId in this.TraitIds)
            {
                if (Db.Get().traits.Exists(traitId))
                    this.AddInternal(Db.Get().traits.Get(traitId));
            }
        }

        private void AddInternal(Trait trait)
        {
            if (this.HasTrait(trait))
                return;
            this.TraitList.Add(trait);


            Modifiers component = this.gameObject.GetComponent<Modifiers>();
            component.attributes = new Attributes(this.gameObject);
            trait.AddTo(this.GetAttributes());
            if (trait.OnAddTrait == null)
                return;
            trait.OnAddTrait(this.gameObject);
        }

        public void Add(Trait trait)
        {
            if (trait.ShouldSave)
                this.TraitIds.Add(trait.Id);
            this.AddInternal(trait);
        }

        public bool HasTrait(string trait_id)
        {
            bool flag = false;
            foreach (Resource trait in this.TraitList)
            {
                if (trait.Id == trait_id)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        public bool HasTrait(Trait trait)
        {
            foreach (Trait trait1 in this.TraitList)
            {
                if (trait1 == trait)
                    return true;
            }
            return false;
        }

        public void Clear()
        {
            while (this.TraitList.Count > 0)
                this.Remove(this.TraitList[0]);
        }

        public void Remove(Trait trait)
        {
            for (int index = 0; index < this.TraitList.Count; ++index)
            {
                if (this.TraitList[index] == trait)
                {
                    this.TraitList.RemoveAt(index);
                    this.TraitIds.Remove(trait.Id);
                    trait.RemoveFrom(this.GetAttributes());
                    break;
                }
            }
        }
    }
}