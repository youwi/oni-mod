using HarmonyLib;
using JetBrains.Annotations;
using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using static Klei.GenericGameSettings;

namespace PerformanceLogMod
{

    [HarmonyPatch(typeof(PauseScreen), "OnPrefabInit")]
    public static class PauseScreen_OnPrefabInit_Patch
    {
        private static readonly KButtonMenu.ButtonInfo logButtonInfo = new KButtonMenu.ButtonInfo(
            "PerformanceCapture",
            Action.NumActions,
           new UnityAction(PerformanceCapturePatch.MyPerformanceCapture));
        private static void Postfix(ref IList<KButtonMenu.ButtonInfo> ___buttons)
        {
            List<KButtonMenu.ButtonInfo> list = ___buttons.ToList<KButtonMenu.ButtonInfo>();
            logButtonInfo.isEnabled = true;
            list.Insert(1, logButtonInfo);
            ___buttons = (IList<KButtonMenu.ButtonInfo>)list;
        }
    }
    public  class PerformanceCapturePatch
    {
        public static void MyPerformanceCapture()
        {   //Game.PerformanceCapture 从复制
            if (SpeedControlScreen.Instance != null)
            {
                SpeedControlScreen.Instance.Unpause(true);
            }
            if (Time.timeSinceLevelLoad < GenericGameSettings.instance.performanceCapture.waitTime)
            {
                return;
            }
            uint num = 581979U;
            string text = System.DateTime.Now.ToShortDateString();
            string text2 = System.DateTime.Now.ToShortTimeString();
            string fileName = Path.GetFileName(GenericGameSettings.instance.performanceCapture.saveGame);
            string text3 = "Version,Date,Time,SaveGame";
            string text4 = string.Format("{0},{1},{2},{3}", new object[]
            {
                num,
                text,
                text2,
                fileName
            });
            float num2 = 0.1f;
            if (GenericGameSettings.instance.performanceCapture.gcStats)
            {
                global::Debug.Log("Begin GC profiling...");
                float realtimeSinceStartup = Time.realtimeSinceStartup;
                GC.Collect();
                num2 = Time.realtimeSinceStartup - realtimeSinceStartup;
                global::Debug.Log("\tGC.Collect() took " + num2.ToString() + " seconds");
                MemorySnapshot memorySnapshot = new MemorySnapshot();
                string format = "{0},{1},{2},{3}";
                string path = "./memory/GCTypeMetrics.csv";
                if (!File.Exists(path))
                {
                    using (StreamWriter streamWriter = new StreamWriter(path))
                    {
                        streamWriter.WriteLine(string.Format(format, new object[]
                        {
                        text3,
                        "Type",
                        "Instances",
                        "References"
                        }));
                    }
                }
                using (StreamWriter streamWriter2 = new StreamWriter(path, true))
                {
                    foreach (MemorySnapshot.TypeData typeData in memorySnapshot.types.Values)
                    {
                        streamWriter2.WriteLine(string.Format(format, new object[]
                        {
                        text4,
                        "\"" + typeData.type.ToString() + "\"",
                        typeData.instanceCount,
                        typeData.refCount
                        }));
                    }
                }
                global::Debug.Log("...end GC profiling");
            }
            float fps = Global.Instance.GetComponent<PerformanceMonitor>().FPS;
            Directory.CreateDirectory("./memory");
            string format2 = "{0},{1},{2}";
            string path2 = "./memory/GeneralMetrics.csv";
            if (!File.Exists(path2))
            {
                using (StreamWriter streamWriter3 = new StreamWriter(path2))
                {
                    streamWriter3.WriteLine(string.Format(format2, text3, "GCDuration", "FPS"));
                }
            }
            using (StreamWriter streamWriter4 = new StreamWriter(path2, true))
            {
                streamWriter4.WriteLine(string.Format(format2, text4, num2, fps));
            }
            GenericGameSettings.instance.performanceCapture.waitTime = 0f;
        }
    }
}
