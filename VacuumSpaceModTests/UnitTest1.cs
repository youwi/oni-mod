using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ModTests
{
    /// <summary>
    /// UnitTest1 的摘要说明
    /// </summary>
    [TestClass()]
    public class UnitTest1
    {


        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod()]
        public void TestMethodFFF1()
        {
            //这个运行时才能显示.
            // moderatelyLoose",
            //-- moderatelyTight",
            // extremelyTight",
            //-- bonusLice",
            // sunnySpeed",
            //-- slowBurn",
            //-- blooms",
            // loadedWithFruit",
            // heavyFruit",
            // rottenHeaps",
            string[] ss =
            {
                "moderatelyLoose",
                "moderatelyTight",
                "extremelyTight",
                "bonusLice",
                "sunnySpeed",
                "slowBurn",
                "blooms",
                "loadedWithFruit",
                "heavyFruit",
                "rottenHeaps"
            };
            List<string> stou = new List<string>();
            foreach (var s in ss)
            {
                foreach (var v in ss)
                {
                    if (s != v)
                    {
                        var tmpa = s + "_" + v;
                        var tmpb = v + "_" + s;
                        // Console.WriteLine(tmp) ;
                        // stou.Append(tmp);
                        if (!stou.Contains(tmpb))
                            stou.Add(tmpa);
                    }
                }
            }
            Console.WriteLine(String.Join("\",\n\"", stou));
            Console.WriteLine(stou.Count);
            // Console.WriteLine(stou);
            // 
            // TODO:  在此处添加测试逻辑
            //
        }

        [TestMethod()]
        public void TestMethodFFsssF2()
        {
            for(int i = 0; i < 20; i++)
            {
                Console.WriteLine(DoubleMutantMod.DoubleMutantModPatch.randInList());
                Thread.Sleep(100);
                Console.WriteLine(DoubleMutantMod.DoubleMutantModPatch.randInList());
            }
        
        }

        [TestMethod()]
        public void TestMethodFFsssF2222()
        {
            for (int i = 0; i < 20; i++)
            {

               if(DoubleMutantMod.DoubleMutantModPatch.rand100()>3)
                        DoubleMutantMod.DoubleMutantModPatch.randInList();
                Thread.Sleep(100);
            }

        }
        
    }
}
