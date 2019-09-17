using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ELU_lib
{
    public class Chances
    {
        Dictionary<int, float> dict = new Dictionary<int, float>();
       public void addChance(float weight)
        {
            dict[dict.Count] = weight;

        }
        public int rollChance()
        {
            float sum = 0;
            int endValue=0;

            var random = Random.Range(0f, 1.0f);

            foreach (var key in dict.Keys)
            {
                var value = dict[key];
                sum += value; 
                if (random <= sum)
                {
                    endValue = key;
                    break;
                }
            }
            return endValue;
        }
    }
}
