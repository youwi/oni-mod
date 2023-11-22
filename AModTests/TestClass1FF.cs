using HarmonyLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PerformanceLogMod;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
 

namespace ModTests
{
    [TestClass()]
    public class TestClassMore
    {

        [TestMethod()]
        public void listToFileTest()
        {
            GCPatch.cache.Add("abc");
            GCPatch.cache.Add("abc");
            GCPatch.cache.Add("abc");
            GCPatch.cache.Add("abc");
            GCPatch.cache.Add("abc");
            GCPatch.cache.Add("abc");
            PerformanceCapturePatch. DumpGCCacheList();
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
                File.Move(newFileName,fileName );
          
            File.Copy(fileName, newFileName );
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
            Debug.LogWarning("给GC打补丁... Postfix:do gc ");
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
    public  class Patch
    {
        public static void Postfix(ref string __result)
        {
            if (__result == "foo")
                __result = "bar";
        }
    }
}
