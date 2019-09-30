using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Primitive_Biome
{
   public static class DebugHelper
    {
        public static void LogForEach(Object obj)
        {
            foreach (object t in (IEnumerable)obj)
            {
                Debug.Log(t.ToString());
            }
        }
        public static void LogForEach(string title,Object obj)
        {
            Debug.Log(title);
            foreach (object t in (IEnumerable)obj)
            {
                Debug.Log(t.ToString());
            }
        }
        public static void Separator()
        {
            
                Debug.Log("----------");
            
        }
        public static void LogVar(Object obj)
        {
            Debug.Log(nameof(obj));
            Debug.Log(obj);
        }
        public static void LogVar(string title,Object obj)
        {
            Debug.Log(title);
            Debug.Log(obj);
        }
        public static void Log(Object obj)
        {
            Debug.Log(obj);
        }
    }
}
