using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Scripting;

namespace PerformanceLogMod
{

    //出异常了,报错.
   // [HarmonyPatch(typeof(System.Net.HttpWebRequest), "GetRequestStream",new Type[] { typeof(TransportContext) })]
    public class SystemHttpsPatch
    {
        public static void Postfix(TransportContext con)
        {
            Https_KleiItems_Patch.calc_count();
        }
    }
    [HarmonyPatch(typeof(ThreadedHttps<KleiAccount>), "Send")]
    public class Https_KleiAccount_Patch
    {
        public static void Postfix(System.Byte[] byteArray, System.Boolean isForce)
        {
            Https_KleiItems_Patch. calc_count();
        }
    }
    [HarmonyPatch(typeof(ThreadedHttps<KleiMetrics>), "Send")]
    public class Https_KleiMetrics_Patch
    {
 

        public static void Postfix(System.Byte[] byteArray, System.Boolean isForce)
        {
            Https_KleiItems_Patch. calc_count();
        }
    }

    [HarmonyPatch(typeof(ThreadedHttps<KleiItems>), "Send")] 
    public class Https_KleiItems_Patch
    {
        static long http_count = 0;
        static float http_last_time = 0;

        public static void Postfix(System.Byte[] byteArray, System.Boolean isForce)
        {
            calc_count();
        }
        public static void calc_count()
        {
            //ThreadedHttps<KleiAccount>
            //ThreadedHttps<KleiItems>
            //ThreadedHttps<KleiMetrics>
            http_count++;
            if (http_count > 100)
            {
                if (UnityEngine.Time.realtimeSinceStartup < http_last_time + 10)
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

