using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei.AI;
using KSerialization;
using UnityEngine;

namespace Primitive_Biome.GeneticTraits
{
    [SerializationConfig(MemberSerialization.OptIn)]

    class StringHolderComponent : KMonoBehaviour, ISaveLoadable
    {

        [Serialize]
        public String text = null;

        [Serialize]
        public String id = null;
 
    }
}