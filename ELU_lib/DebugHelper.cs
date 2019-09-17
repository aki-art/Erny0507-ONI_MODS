using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELU_lib
{
   public static class DebugHelper
    {
        public static void LogForEach(Object obj)
        {
            foreach (object t in (IEnumerable)obj)
            {
                Debug.Log(t);
            }
        }
    }
}
