using HarmonyLib;
using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Timers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;
using UnityEngine.Scripting;

namespace PerformanceLogMod
{

    [HarmonyPatch(typeof(PauseScreen), "OnPrefabInit")]
    public static class PauseScreen_OnPrefabInit_Patch
    {
        public static readonly KButtonMenu.ButtonInfo logButtonInfo = new KButtonMenu.ButtonInfo(
            "PerformanceCapture",
            Action.NumActions,
           new UnityAction(PerformanceCapturePatch.delayAction));
        public static readonly KButtonMenu.ButtonInfo gcButton = new KButtonMenu.ButtonInfo(
           "Clean Memery",
           Action.NumActions,
          new UnityAction(PerformanceCapturePatch.doCollect));
        private static void Postfix(ref IList<KButtonMenu.ButtonInfo> ___buttons)
        {
            List<KButtonMenu.ButtonInfo> list = ___buttons.ToList<KButtonMenu.ButtonInfo>();
            logButtonInfo.isEnabled = true;

            list.Insert(0, logButtonInfo);
            list.Insert(0, gcButton);
            ___buttons = (IList<KButtonMenu.ButtonInfo>)list;
        }
    }
    public class PerformanceCapturePatch
    {
        static List<Notification> notifications = null;
        static List<Notification> pendingNotifications = null;
        public static void clearMessage()
        {
            msgCount();
            Debug.Log($"-- Memory allocation High.{lastMemSizeM}M >>>>remvoe messages:{notifications.Count} {pendingNotifications.Count}");
            notifications.Clear();
            pendingNotifications.Clear();
            return;
        }
        public static int msgCount()
        {
            if (notifications == null)
            {
                notifications = (List<Notification>)Traverse.Create(NotificationManager.Instance)
                 .Field("notifications").GetValue();
                pendingNotifications = (List<Notification>)Traverse.Create(NotificationManager.Instance)
                   .Field("pendingNotifications").GetValue();
            }
            int count = notifications.Count + pendingNotifications.Count;
            return count;
        }
        public static void doCollect()
        {
            // PauseScreen.Instance.FindOrAdd<GCManualControlS>();
            SpeedControlScreen.Instance.gameObject.AddOrGet<GCManualControlS>();//初始化.

            string gcMode = "...";
            if (GarbageCollector.GCMode == GarbageCollector.Mode.Enabled)
            {
                GarbageCollector.GCMode = GarbageCollector.Mode.Manual;
                gcMode = "Large";
            }
            else
            {
                GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
                gcMode = "Original";
            }

            // Process proc = Process.GetCurrentProcess();
            //var mem=proc.PrivateMemorySize64 / 1024 / 1024;
            var mem = GC.GetTotalMemory(false) / 1024 / 1024;
            float realtimeSinceStartup = Time.realtimeSinceStartup;
            GC.Collect();
            var gcTime = Time.realtimeSinceStartup - realtimeSinceStartup;
            var mem2 = GC.GetTotalMemory(true) / 1024 / 1024;

            //long memP = Profiler.GetMonoUsedSizeLong();
            var mem3 = Profiler.GetMonoUsedSizeLong() / 1024 / 1024;
            var mem4 = Profiler.GetTotalAllocatedMemoryLong() / 1024 / 1024;
            //腐烂物
            PauseScreen_OnPrefabInit_Patch.gcButton.text = $"Clean Memery ({gcTime:0.0}s {mem2 - mem}M,ALL:{mem3}M GC:{gcMode})";
            PauseScreen.Instance.RefreshButtons();
            clearMessage();

            //翻译测试:
            Debug.Log("---->" + STRINGS.DUPLICANTS.TRAITS.GREENTHUMB.NAME);
        }
        static int ondoing = 0;
        static System.Timers.Timer timer = null;
        static long lastMemSizeM = 0;
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
            }
            else if (ondoing == 1)
            {
                timer.Start();
                PauseScreen_OnPrefabInit_Patch.logButtonInfo.text = "PerformanceCapture with GC >ing<";
                GenericGameSettings.instance.performanceCapture.gcStats = true;
                ondoing = 2;
                PauseScreen.Instance.RefreshButtons();
            }
            else if (ondoing == 2)
            {
                timer.Stop();
                GenericGameSettings.instance.performanceCapture.gcStats = false;
                PauseScreen_OnPrefabInit_Patch.logButtonInfo.text = " Dump GC  >ing<";
                ondoing = 3;
                PauseScreen.Instance.RefreshButtons();
                DumpGCCacheList();
                PauseScreen_OnPrefabInit_Patch.logButtonInfo.text = " Dump GcTicks >ing<";
                PauseScreen.Instance.RefreshButtons();
            }
            else
            {

                timer.Stop();
                ondoing = 0;
                PauseScreen_OnPrefabInit_Patch.logButtonInfo.text = "PerformanceCapture";
                GenericGameSettings.instance.performanceCapture.gcStats = false;
                PauseScreen.Instance.RefreshButtons();
            }
        }
        public static void MyPerformanceCapture()
        {   //Game.PerformanceCapture 从复制
            if (SpeedControlScreen.Instance.IsPaused)
            {
                return;//暂停时不记录.
                       //  SpeedControlScreen.Instance.Unpause(true);
            }
            if (Global.Instance.GetComponent<PerformanceMonitor>().FPS < 3)
            {
                return;//太卡了也不记录.
            }

            uint versionNum = 581979U;

            string timeText = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff");
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
                    File.Move(csvFileName, csvFileName + "." + (long)System.DateTime.Now.Ticks / 10000);
                }
            }

            if (!File.Exists(csvFileName))
            {
                using (StreamWriter streamWriter3 = new StreamWriter(csvFileName))
                {
                    streamWriter3.WriteLine("Version,DateTime,Memory,GCDuration,msg_count,GCCount,FPS");
                }
            }
            int msgCt = msgCount();


            using (StreamWriter streamWriter4 = new StreamWriter(csvFileName, true))
            {
                var memSizeM = GC.GetTotalMemory(false) / 1024 / 1024;
                // GarbageCollector.CollectIncremental
                // GarbageCollector.incrementalTimeSliceNanoseconds
                if (memSizeM > lastMemSizeM + 128)
                {
                    //内存突然升高
                    //NotificationManager.Instance.
                    //Unity.Profiling.ProfilerRecorder
                    clearMessage();

                }

                streamWriter4.WriteLine($"{versionNum},{timeText},{memSizeM},{gcTime:0.00},{msgCt},{GCAllMyPatches.cache.Count},{fps:0.0}");
                lastMemSizeM = (memSizeM + lastMemSizeM) / 2;//累计平均.
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
                //  GCPatch.cache.ToArray;
                //  MemorySetup.
                streamWriter4.WriteLine(String.Join("\n", GCAllMyPatches.cache));
            }
        }
    }

    [HarmonyPatch] // make sure Harmony inspects the class
    public class GCAllMyPatches
    {
        public static List<string> cache = new System.Collections.Generic.List<string>();

        static IEnumerable<MethodBase> TargetMethods()
        {
            return typeof(System.GC).GetMethods()
                .Where(method => method.Name.StartsWith("Collect"))
                .Cast<MethodBase>();
        }
        public static void Postfix()
        {
            if (cache.Count > 100000)
            {  //消耗太大,重置.
                cache = new System.Collections.Generic.List<string>();
            }
            cache.Add(System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
            // 
            Debug.Log("PerformanceLogMod.GCPatch:>>>>>>给GC打补丁: ....... "
                + GC.GetTotalMemory(true) / 1024 / 1024 + "M /"
                + GarbageCollector.incrementalTimeSliceNanoseconds
                );
        }
    }
    //[HarmonyPatch(typeof(GarbageCollector), "CollectIncremental")]
    //public class GCPatch
    //{
    //    public static void Postfix()
    //    {
    //        //GarbageCollector.CollectIncremental();
    //        GCAllMyPatches.Postfix();
    //    }
    //}




    //[HarmonyPatch(typeof(System.GC), "Collect", new Type[] { })]
    // public class  GCPatch
    // {

    // }
    //[HarmonyPatch(typeof(System.GC), "Collect", new Type[] { typeof(int) })]
    //public class GCIntPatch
    //{
    //    public static List<string> cache = new System.Collections.Generic.List<string>();
    //    public static void Postfix()
    //    {
    //        if (cache.Count > 100000)
    //        {  //消耗太大,重置.
    //            cache = new System.Collections.Generic.List<string>();
    //        }

    //        cache.Add(System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
    //        // 
    //        Debug.Log("PerformanceLogMod.GCPatch:>>>>>>给GC打补丁: ....... "
    //            + GC.GetTotalMemory(true) / 1024 / 1024 + "M /"
    //            + GarbageCollector.incrementalTimeSliceNanoseconds
    //            );
    //    }
    //}

    //调整刷新速度,看能不能提升性能
    [HarmonyPatch(typeof(EnergyInfoScreen), "Refresh")]
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
            if (tickRefresh > 100)
            {
                Debug.Log($"---> 刷新次数:{tickRefresh} ");
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
