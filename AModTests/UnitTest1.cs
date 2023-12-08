using HarmonyLib;
using Klei;
using Klei.AI;
using Klei.CustomSettings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Timers;
using TranslateFixMod;
using static STRINGS.ROOMS.CRITERIA;

namespace ModTests.Tests
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
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine(DoubleMutantMod.DoubleMutantModPatch.randInList());
                Thread.Sleep(100);
                Console.WriteLine(DoubleMutantMod.DoubleMutantModPatch.randInList());
            }

        }
        [TestMethod()]
        public void TestMethodFFsssF3()
        {
            var configPlanB = YamlIO.LoadFile<SortedDictionary<string, bool>>("../../exp.yaml");

            var tflot = File.GetLastWriteTime("../../exp.yaml").Ticks;
            ;// 6383574106505 99408
            var dd = System.DateTime.Now.Ticks;
            Console.WriteLine("" + tflot + " ");
            Console.WriteLine("" + dd + " ");
            Console.WriteLine((dd - tflot) / 10000 / 1000);
            // Time.time
        }
        [TestMethod()]
        public void testRef()
        {
            var harmony = new Harmony("ModTests.Tests");
             
            //harmony.PatchAll();
            //ldftn System.Void <>c::<InitializeStates>b__6_0(StatesInstance smi)
            //RocketUsageRestriction+<>c.<InitializeStates>b__6_0 (RocketUsageRestriction+StatesInstance smi)
            var sk = AccessTools.Method("RocketUsageRestriction.<>c:<InitializeStates>b__6_0",
                     new Type[] { typeof(RocketUsageRestriction.StatesInstance)});
            Console.WriteLine("----->:"+sk );
            //作者：geleisisisi https://www.bilibili.com/read/cv22698875/ 出处：bilibili
        }
        [HarmonyPatch(typeof(RocketUsageRestriction), "InitializeStates")]
        public class test_patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
                foreach (var item in codes)
                {
                    Console.WriteLine(item);  
                }
                return codes.AsEnumerable<CodeInstruction>();
            }
        }
       

        [TestMethod()]
        public void TestMethodFFsssF2222()
        {

            AsteroidGridEntity age=new AsteroidGridEntity();
            Traverse.Create(age).Field("m_name").SetValue("delete");
            Console.Write(age.Name);
            for (int i = 0; i < 20; i++)
            {

                if (DoubleMutantMod.DoubleMutantModPatch.rand100() > 3)
                    DoubleMutantMod.DoubleMutantModPatch.randInList();
                Thread.Sleep(100);
            }

        }
        [TestMethod()]
        public void tests()
        {
            // var tt= Db.Get().GameplaySeasons;
            // var fieldList = Type.GetType("Database.GameplaySeasons").GetFields();
            var fieldList = typeof(Database.GameplaySeasons).GetFields();
            foreach (var field in fieldList)
            {
                // Database.GameplaySeasons;
                if (field.Name.EndsWith("MeteorShowers"))
                {
                    //  Debug.Log(field.Name);
                    Console.WriteLine("\"" + field.Name + "\",");
                }

            }
            Debug.Log(fieldList.Length);
            Debug.Log(fieldList);

        }

        [TestMethod()]
        public void tests2()
        {

            new MeteorShowerSeason("RegolithMoonMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, synchronizedToPeriod: false, -1f, startActive: true, -1, 0f, float.PositiveInfinity, 1, affectedByDifficultySettings: true, 6000f)
              .AddEvent(Db.Get().GameplayEvents.MeteorShowerDustEvent)
              .AddEvent(Db.Get().GameplayEvents.ClusterIronShower)
              .AddEvent(Db.Get().GameplayEvents.ClusterIceShower);

        }
        [TestMethod()]
        public void sfefawefawefaw()
        {
            Assets.GetSprite("space_race");



        }
        [TestMethod()]
        public void stesese()
        {
            var timer2 = new System.Threading.Timer(timerCallbackFun2, null, 1000, 500);
            //timer2.
            var ss = STRINGS.UI.CLUSTERMAP.POI.MASS_REMAINING;
            var st = new System.Timers.Timer(1000);
            st.AutoReset = false;
            st.Enabled = true;
            st.Elapsed += timerCallbackFun;
            st.Start();
            Thread.Sleep(2000);
        }
        public static void timerCallbackFun(object data, ElapsedEventArgs ss)
        {

            Console.WriteLine("测试");

        }
        public static void timerCallbackFun2(object data)
        {
            Console.WriteLine("测试22");
        }

        [TestMethod()]
        public void DDDDDDDDDDDDcellTest()
        {
            Grid.WidthInCells = 137;
            Grid.HeightInCells = 140;
            Grid.CellSizeInMeters = 50;
            var pos = Grid.CellToPos(8241, 0, 0, 0);
            Console.WriteLine(pos);
        }

        [TestMethod()]
        public void TestLocs()
        {

            Console.WriteLine("DFSDFSD>FSDFSDF");
            var stringDir = "G:/Steam/steamapps/common/OxygenNotIncluded/OxygenNotIncluded_Data/StreamingAssets/strings/strings_preinstalled_zh_klei.po";

            //var dic = Localization.LoadStringsFile(stringDir, true);
            var dic = ReadPoIIIIIIII.TranslatedStringsEnCn(File.ReadAllLines(stringDir, Encoding.UTF8));

            dic.TryGetValue("Stinkiness", out var loc);
            Console.WriteLine("DFSDF" + loc);
        }

        [TestMethod()]
        public void TestStringKey()
        {
       

            StringEntry entry = null;
            string name = "DECORATION";
            string text = "STRINGS.ROOMS.CRITERIA." + name.ToUpper() + ".NAME";
            var keyObj = STRINGS.ROOMS.CRITERIA.DECORATION.NAME;
            keyObj.SetKey("BAAAA");
            Console.WriteLine(keyObj);

            string keyObjstr = keyObj;
            Console.WriteLine(keyObjstr);

            LocString bbk = keyObjstr;
            Console.WriteLine(bbk);

            var tt= RoomConstraints.NO_MESS_STATION;
            var tb = RoomConstraints.NO_COTS;
            var tc = RoomConstraints.MAXIMUM_SIZE_96;

           // DlcManager.IsPureVanilla();//给打补丁禁止
        
            //CustomGameSettingConfigs.cctor();
            Strings.TryGet(new StringKey(text), out entry);

            //Console.WriteLine(entry);
            BindingFlags staticflags =
                BindingFlags.Instance
                | BindingFlags.Public 
                | BindingFlags.Static
                | BindingFlags.SetField
                | BindingFlags.NonPublic
                | BindingFlags.SetProperty;
            var harmony = new Harmony("ModTests.Tests");
            harmony.PatchAll();
            ToggleSettingConfig StressBreaks = new ToggleSettingConfig("StressBreaks", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS_BREAKS.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS_BREAKS.TOOLTIP, new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS_BREAKS.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS_BREAKS.LEVELS.DISABLED.TOOLTIP, 1L), new SettingLevel("Default", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS_BREAKS.LEVELS.DEFAULT.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS_BREAKS.LEVELS.DEFAULT.TOOLTIP, 0L), "Default", "Default", 262144L, 5L);
            ToggleSettingConfig StressBreaks2 = new ToggleSettingConfig("StressBreaks","汉字测试A", "汉字测试B",
                new SettingLevel("Disabled",
                "汉字测试C", "汉字测试D", 1L), 
                new SettingLevel("Default",
                UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS_BREAKS.LEVELS.DEFAULT.NAME,
                UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS_BREAKS.LEVELS.DEFAULT.TOOLTIP, 0L),
                "Default", "Default", 262144L, 5L);
            Traverse.Create(StressBreaks2).Field("<label>k__BackingField").SetValue("---sss---");

            Traverse.Create(StressBreaks2.on_level).Field("<label>k__BackingField").SetValue("dsdfsdfsdf");

            var ttt =Traverse.Create(StressBreaks2).Fields(); 
            var tt1 = Traverse.Create(StressBreaks2).Methods();
            var tt2 = typeof(ToggleSettingConfig ).GetProperties();
            Traverse.Create(StressBreaks2).Method("set_label", "sfsdfsdfsdf");// 失败忽略了.
           
            var met = typeof(ToggleSettingConfig).GetMethod("set_label", staticflags);//找不到.
            var ttB = Traverse.Create(StressBreaks2).Properties();
            var bbc = typeof(ToggleSettingConfig).GetFields(staticflags);
            var bbm =  typeof(ToggleSettingConfig).GetMembers();

            var mes = typeof(SettingConfig).GetMember("label")[0];//能找到,但是无法利用.

            var mmm=typeof(ToggleSettingConfig).GetProperty("label");// 可以找到,但是无法设值.SetValue(StressBreaks2,"xxxxxx");
                                                                     // mmm.SetValue(StressBreaks2, "-------", null);
                                                                     //mes.set(StressBreaks2, "--");
                                                                     // PatchSSS.SetPrivatePropertyValue<string>(StressBreaks2, "label", "ssssss");
            var field = typeof(SettingConfig).GetField("<label>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(StressBreaks2, "---xdfsdfsdf---");

            ListSettingConfig ClusterLayout = new ListSettingConfig("ClusterLayout", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CLUSTER_CHOICE.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CLUSTER_CHOICE.TOOLTIP, null, null, null, -1L, -1L, debug_only: false, triggers_custom_game: false, "", "", editor_only: true);

            var langFilename = $"G:/Steam/steamapps/common/OxygenNotIncluded/OxygenNotIncluded_Data/StreamingAssets/strings/strings_preinstalled_zh_klei.po";
             
            TestTranslatePath.translateDictionary = ReadPoIIIIIIII.TranslatedStringsEnCn(File.ReadAllLines(langFilename, Encoding.UTF8));
            TestTranslatePath.updateLabelAndTooltip(ClusterLayout);

        Console.WriteLine(StressBreaks2.label);
            // var bbt = CustomGameSettingConfigs.StressBreaks;
        }
       
        public void getBykey(string name)
        {

        }
 
    }
    [HarmonyPatch(typeof(DlcManager), nameof(DlcManager.IsPureVanilla))]
    public static class PatchSSS
    {
        static BindingFlags staticflags =
                BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.Static
                | BindingFlags.SetField
                | BindingFlags.SetProperty;
        public static void SetPrivatePropertyValue<T>(this object obj, string propName, T val)
        {
            Type t = obj.GetType();
            if (t.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
                throw new ArgumentOutOfRangeException("propName", string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
            t.InvokeMember(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, obj, new object[] { val });
        }
        public static void SetPrivateFieldValue<T>(this object obj, string propName, T val)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            Type t = obj.GetType();
            FieldInfo fi = null;
            while (fi == null && t != null)
            {
                fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                t = t.BaseType;
            }
            if (fi == null) throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
            fi.SetValue(obj, val);
        }

        public static bool Prefix(bool __result)
        {
            __result=true;
            return false;
        }
    }

}
