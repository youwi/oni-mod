using HarmonyLib;
using JetBrains.Annotations;
using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;
using UnityEngine.Events;
using static Klei.GenericGameSettings;

namespace PerformanceLogMod
{

    [HarmonyPatch(typeof(PauseScreen), "OnPrefabInit")]
    public static class PauseScreen_OnPrefabInit_Patch
    {
        public static readonly KButtonMenu.ButtonInfo logButtonInfo = new KButtonMenu.ButtonInfo(
            "PerformanceCapture",
            Action.NumActions,
           new UnityAction(PerformanceCapturePatch.delayAction));
        private static void Postfix(ref IList<KButtonMenu.ButtonInfo> ___buttons)
        {
            List<KButtonMenu.ButtonInfo> list = ___buttons.ToList<KButtonMenu.ButtonInfo>();
            logButtonInfo.isEnabled = true;
            
            list.Insert(0, logButtonInfo);
            ___buttons = (IList<KButtonMenu.ButtonInfo>)list;
        }
    }
    public  class PerformanceCapturePatch
    {
        static int ondoing=0;
        static  System.Timers.Timer timer=null;
        public static void delayAction()
        {
            if (timer == null)
            {
                timer = new System.Timers.Timer(3000); //延迟
                timer.AutoReset = true;
                timer.Enabled = true;
                timer.Elapsed += (object data2, ElapsedEventArgs ss) =>
                {
                    MyPerformanceCapture();
            
                };
                
            }
            if (ondoing == 0)
            {
                timer.Start();
                PauseScreen_OnPrefabInit_Patch.logButtonInfo.text = "PerformanceCapture >ing<";
                GenericGameSettings.instance.performanceCapture.gcStats = false;

                ondoing = 1;
                PauseScreen.Instance.RefreshButtons();
            }else if (ondoing == 1)
            {
                timer.Start();
                PauseScreen_OnPrefabInit_Patch.logButtonInfo.text = "PerformanceCapture >ing+ GC<";
                GenericGameSettings.instance.performanceCapture.gcStats = true;
                ondoing = 2;
                PauseScreen.Instance.RefreshButtons();
            } else{
                timer.Stop();
                ondoing = 0;
                PauseScreen_OnPrefabInit_Patch.logButtonInfo.text = "PerformanceCapture";
                GenericGameSettings.instance.performanceCapture.gcStats = false;
                PauseScreen.Instance.RefreshButtons();
            }
        }
        public static void MyPerformanceCapture()
        {   //Game.PerformanceCapture 从复制
            if (SpeedControlScreen.Instance.IsPaused )
            {
                SpeedControlScreen.Instance.Unpause(true);
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
            float num2 = 0f;
            if (GenericGameSettings.instance.performanceCapture.gcStats)
            {
                global::Debug.Log("Begin GC profiling...");
                float realtimeSinceStartup = Time.realtimeSinceStartup;
                GC.Collect();
                num2 = Time.realtimeSinceStartup - realtimeSinceStartup;
                global::Debug.Log("\tGC.Collect() took " + num2.ToString() + " seconds");
               // MemorySnapshot memorySnapshot = new MemorySnapshot();
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
                    //foreach (MemorySnapshot.TypeData typeData in memorySnapshot.types.Values)
                    //{
                    //    streamWriter2.WriteLine(string.Format(format, new object[]
                    //    {
                    //    text4,
                    //    "\"" + typeData.type.ToString() + "\"",
                    //    typeData.instanceCount,
                    //    typeData.refCount
                    //    }));
                    //}
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
            //GenericGameSettings.instance.performanceCapture.waitTime = 0f;
        }
    }
}
