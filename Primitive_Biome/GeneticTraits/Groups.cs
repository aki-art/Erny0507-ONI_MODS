using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Primitive_Biome.GeneticTraits
{
  public sealed class Group
  {
    //public static readonly Group SizeGroup = new Group("SizeGroup", 0.3f);
    //public static readonly Group NoiseGroup = new Group("NoiseGroup", 0.05f);
    //public static readonly Group SmellGroup = new Group("SmellGroup", 0.05f, inst => !inst.HasTag(GameTags.Creatures.Swimmer));
    //public static readonly Group GlowGroup = new Group("GlowGroup", 0.08f, inst => inst.GetComponent<Light2D>() == null);
    public static readonly Group SpeedGroup = new Group("SpeedGroup", 0.2f, inst => inst.GetComponent<Navigator>() != null);
    public static readonly Group LifespanGroup = new Group("LifespanGroup", 0.15f, inst => HasAmount(inst, Db.Get().Amounts.Age));
    public static readonly Group FertilityGroup = new Group("FertilityGroup", 0.1f, inst => HasAmount(inst, Db.Get().Amounts.Fertility));
    
    public static readonly Group ElementEmitterGroup = new Group("ElementEmitterGroup", 0.1f);
    public static readonly Group ElementAbsorberGroup = new Group("ElementAbsorberGroup", 0.1f);
    public static readonly Group ElementConverterGroup = new Group("ElementConverterGroup", 0.1f);
    public static readonly Group EggHatchingSpeedGroup = new Group("EggHatchingSpeedGroup", 0.05f);
    public static readonly Group GrowingUpSpeedGroup = new Group("GrowingUpSpeedGroup", 0.05f);
    public static readonly Group HealthGroup = new Group("HealthGroup", 0.05f);
    public Group(string id, float probability, Predicate<GameObject> requirement = null,List<string> exceptions=null )
    {
      Id = id;
      Probability = probability;
      HasRequirements = requirement ?? (_ => true);
      Exceptions=exceptions;
    }

    public string Id { get; private set; }
    public float Probability { get; private set; }
    public Predicate<GameObject> HasRequirements { get; private set; }
    public List<string> Exceptions { get; private set; }
    private static bool HasAmount(GameObject go, Amount amount)
    {
      return go.GetComponent<Modifiers>()?.amounts.Has(amount) ?? false;
    }
  }
}
