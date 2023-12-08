using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Scripting;

namespace PerformanceLogMod
{
    [HarmonyPatch("ThreadedHttps", "Send")] 
    public class Https_Patch
    {
        static long http_count = 0;
        static float http_last_time = 0;
        public static void Postfix()
        {
            http_count++;
            if(http_count > 100)
            {
              
                if(UnityEngine.Time.realtimeSinceStartup < http_last_time + 10)
                {
                    Debug.LogWarning("---> ThreadedHttps.Send Count: 100 Reset in 10s");
                }
                else
                {
                    Debug.Log("---> ThreadedHttps.Send Count: 100 Reset");
                }
                http_count = 0;
                http_last_time = UnityEngine.Time.realtimeSinceStartup;
            }
        }
    }
}

