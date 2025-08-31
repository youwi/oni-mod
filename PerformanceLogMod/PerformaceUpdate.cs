using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PerformanceLogMod
{
    //TargetScreen
    //调整刷新速度,看能不能提升性能
   //  [HarmonyPatch(typeof(EnergyInfoScreen), "Refresh")]
    [HarmonyPatch(typeof(TargetScreen), "Refresh")]
    
    public class EnergyInfoScreen_Refresh_Patch
    {
       
        static float tickSecond = 0;
        static float tickRefresh = 0;
        public static bool Prefix()
        {

            if (Time.realtimeSinceStartup - tickSecond > 5)
            {
                tickSecond = Time.realtimeSinceStartup;
                return true;
            }
            tickRefresh++;
            if (tickRefresh > 10000)
            {
                Debug.Log($"---> 刷新次数:{tickRefresh} ");
                tickRefresh = 0;
            }
            return false;
        }
    }
    [HarmonyPatch(typeof(OverlayModes.Power), "Update")]
    public class OverlayModes_Power_Update_Patch
    {
        static float tickSecond = 0;
        static float tickRefresh = 0;
        public static bool Prefix()
        {

            if (Time.realtimeSinceStartup - tickSecond > 2)
            {
                tickSecond = Time.realtimeSinceStartup;
                return true;
            }
            tickRefresh++;
            if (tickRefresh > 10000)
            {
                Debug.Log($"---> OverlayModes.Power 刷新次数:{tickRefresh} ");
                tickRefresh = 0;
            }
            return false;
        }
    }
    //禁止 腐烂物的消息,可能有助于减少内存. 功能测试中,好像有点用...

    [HarmonyPatch(typeof(RotPile), "TryCreateNotification")]
    public class OplefPatch
    {
        public static bool Prefix()
        {
            return false;
        }
    }
}
