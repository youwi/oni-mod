﻿using HarmonyLib;
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
using UnityEngine.Scripting;
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
                GarbageCollector.GCMode = GarbageCollector.Mode.Manual;
                timer.Start();
                PauseScreen_OnPrefabInit_Patch.logButtonInfo.text = "PerformanceCapture >ing<";
                GenericGameSettings.instance.performanceCapture.gcStats = false;
                ondoing = 1;
                PauseScreen.Instance.RefreshButtons();
            }else if (ondoing == 1)
            {
                timer.Start();
                PauseScreen_OnPrefabInit_Patch.logButtonInfo.text = " GC.Collect() >ing<";
                GenericGameSettings.instance.performanceCapture.gcStats = true;
                ondoing = 2;
                PauseScreen.Instance.RefreshButtons();
            }else if (ondoing==2) {
                timer.Stop();
                GenericGameSettings.instance.performanceCapture.gcStats = false;
                PauseScreen_OnPrefabInit_Patch.logButtonInfo.text = " Dump GC  >ing<";
                ondoing = 3;
                PauseScreen.Instance.RefreshButtons();
                DumpGCCacheList();
                PauseScreen_OnPrefabInit_Patch.logButtonInfo.text = " Dump GcTicks >ing<";
                PauseScreen.Instance.RefreshButtons();
            }
            else {
                GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
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
       
            uint versionNum = 581979U;
            string dateText = System.DateTime.Now.ToShortDateString();
            string timeText = System.DateTime.Now.ToShortTimeString();
            string savefileName = Path.GetFileName(GenericGameSettings.instance.performanceCapture.saveGame);
            float gcTime = 0f;

            if (GenericGameSettings.instance.performanceCapture.gcStats)
            {
               //  string gcHeadText = "Version,Date,Time,SaveGame";
               // global::Debug.Log("Begin GC profiling...");
                float realtimeSinceStartup = Time.realtimeSinceStartup;
                GC.Collect();
                gcTime = Time.realtimeSinceStartup - realtimeSinceStartup;
                global::Debug.Log("\tGC.Collect() took " + gcTime.ToString() + " seconds");
               // MemorySnapshot memorySnapshot = new MemorySnapshot();
                //string format = "{0},{1},{2},{3}";
                //string gcCsvFilepath = "./memory/GCTypeMetrics.csv";
                //if (!File.Exists(gcCsvFilepath))
                //{
                //    using (StreamWriter streamWriter = new StreamWriter(gcCsvFilepath))
                //    {
                //        streamWriter.WriteLine(string.Format(format, new object[]
                //        {
                //        gcHeadText,
                //        "Type",
                //        "Instances",
                //        "References"
                //        }));
                //    }
                //}
                //using (StreamWriter streamWriter2 = new StreamWriter(gcCsvFilepath, true))
                //{
                //    //foreach (MemorySnapshot.TypeData typeData in memorySnapshot.types.Values)
                //    //{
                //    //    streamWriter2.WriteLine(string.Format(format, new object[]
                //    //    {
                //    //    text4,
                //    //    "\"" + typeData.type.ToString() + "\"",
                //    //    typeData.instanceCount,
                //    //    typeData.refCount
                //    //    }));
                //    //}
                //}
               // global::Debug.Log("...end GC profiling");
            }
            float fps = Global.Instance.GetComponent<PerformanceMonitor>().FPS;
            Directory.CreateDirectory("./memory");
        
            string csvFileName = "./memory/GeneralMetrics.csv";
            if (File.Exists(csvFileName))
            {
                long length = new System.IO.FileInfo(csvFileName).Length;
                if (length > 10 * 1024 * 1024)  //大于10M ,1024K
                {
                    File.Move(csvFileName, csvFileName+"."+(long)System.DateTime.Now.Ticks / 10000 );
                }
            }
           
            if (!File.Exists(csvFileName))
            {
                using (StreamWriter streamWriter3 = new StreamWriter(csvFileName))
                {
                    streamWriter3.WriteLine( "Version,Date,Time,SaveGame,GCDuration,GCCount,FPS");
                }
            }
            
            using (StreamWriter streamWriter4 = new StreamWriter(csvFileName, true))
            {
                ; // GarbageCollector.CollectIncremental
                  // GarbageCollector.incrementalTimeSliceNanoseconds
                streamWriter4.WriteLine($"{versionNum},{dateText},{timeText},{savefileName},{gcTime},{GCPatch.cache.Count},{fps}");
            }
            //GenericGameSettings.instance.performanceCapture.waitTime = 0f;
        }
        public static void DumpGCCacheList()
        {
            Directory.CreateDirectory("./memory");
            //if (!File.Exists("./memory/GcTick.txt"))
            //{
            //    File.Create("./memory/GcTick.txt").Close();
            //}
            using (StreamWriter streamWriter4 = new StreamWriter("./memory/GcTick.txt", true))
            {
                //GCPatch.cache.ToArray;
                streamWriter4.WriteLine(String.Join("\n", GCPatch.cache));
            }
        }
    }

    [HarmonyPatch(typeof(System.GC), "Collect", new Type[] { })]
    public class  GCPatch
    {
        public static List<string> cache = new System.Collections.Generic.List<string>();
        public static void Postfix()
        {
            if(cache.Count > 100000)
            {  //消耗太大,重置.
                cache = new System.Collections.Generic.List<string>();
            }
            cache.Add(System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
           // 
            Debug.Log("PerformanceLogMod.GCPatch:>>>>>>给GC打补丁: ....... " + GC.GetTotalMemory(true)/1000/1000 +"M /"+ GarbageCollector.incrementalTimeSliceNanoseconds);
        }
    }
}
