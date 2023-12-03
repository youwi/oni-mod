using HarmonyLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PerformanceLogMod;
using System;
using System.Diagnostics;
using System.IO;


namespace ModTests
{



    [TestClass()]
    public class TestClassMore
    {

        [TestMethod()]
        public void haskEnumTest()
        {
            var t = STRINGS.DUPLICANTS.TRAITS.GREENTHUMB.NAME;
            Console.WriteLine(SimHashes.Katairite.ToString());
            var tmp = Traverse.CreateWithType("SimHashes")
                 .Field("Katairite")
                 .GetValue<SimHashes>();

            ;
            //  Console.WriteLine(KMod.Manager.GetDirectory());
            Console.WriteLine(SimHashes.Katairite.ToString());
            File.WriteAllText("test标杆e.json", "SDFSDFSDFSD");
        }

        [TestMethod()]
        public void lprocTest()
        {
            Process proc = Process.GetCurrentProcess();
            Console.WriteLine(proc.PrivateMemorySize64 / 1024 / 1024);
            Console.WriteLine(proc.WorkingSet64 / 1024 / 1024);

            float sss = 1223.455123f;
            Console.WriteLine(sss.ToString("0.00"));
            Console.WriteLine($"sfasfsdfsdf{sss:0.00}");
        }

        [TestMethod()]
        public void listToFileTest()
        {
            GCAllMyPatches.cache.Add("abc");
            GCAllMyPatches.cache.Add("abc");
            GCAllMyPatches.cache.Add("abc");
            GCAllMyPatches.cache.Add("abc");
            GCAllMyPatches.cache.Add("abc");
            GCAllMyPatches.cache.Add("abc");
            PerformanceCapturePatch.DumpGCCacheList();
        }

        [TestMethod()]
        public void fileMoveTest()
        {

            //  string csvFileName = Application.dataPath + "/abc.test";
            // File.Move(csvFileName, (long)System.DateTime.Now.Ticks / 10000 + csvFileName);
            Console.WriteLine((long)System.DateTime.Now.Ticks / 10000);

            long length = new System.IO.FileInfo("../../app.config").Length;
            string fileName = "../../test.json";
            Console.WriteLine(length); //字节 Byte
                                       // if (length > 10 * 1000 * 1024)

            string newFileName = fileName + "." + (long)System.DateTime.Now.Ticks / 10000;
            File.Move(fileName, newFileName);
            File.Move(newFileName, fileName);

            File.Copy(fileName, newFileName);
            // System.GC.CancelFullGCNotification();

            //var assembly = Assembly.GetExecutingAssembly();
            //harmony.PatchAll(assembly);

            //// or implying current assembly:
            var harmony = new Harmony("ModTests");
            harmony.PatchAll();
            GC.Collect();
            Console.WriteLine("ddddddd"); //字节 Byte
            var s = new OriginalCode();
            Console.WriteLine(s.GetName());
        }

    }

    [HarmonyPatch(typeof(System.GC), "Collect", new Type[] { })]
    public class PatchTest
    {
        public static void Postfix()
        {
            Debug.Log("给GC打补丁... Postfix:do gc ");
            Console.WriteLine("Postfix B"); //字节 Byte
        }
    }

    public class OriginalCode
    {
        string name = "foo";
        public string GetName()
        {
            return name; // ...
        }
    }

    [HarmonyPatch(typeof(OriginalCode), nameof(OriginalCode.GetName))]
    public class Patch
    {
        public static void Postfix(ref string __result)
        {
            if (__result == "foo")
                __result = "bar";
        }
    }
}
