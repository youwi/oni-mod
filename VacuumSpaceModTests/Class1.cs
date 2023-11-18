using Klei;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ClusterGridEntity;

namespace NotificationsPauseI18nMod
{
    [TestClass()]
    public  class Class1
    {
        [TestMethod()]
        public void ssfefTest()
        {
            var fileName  = "../../test.json";
            //var configPlanB = YamlIO.LoadFile<SortedDictionary<string, bool>>(fileName);
             File.Create(fileName).Close();
            SortedDictionary<string, bool> kv = new SortedDictionary<string, bool>();
            kv.Add(STRINGS.CREATURES.STATUSITEMS.ATTACK.NAME, false);
            kv.Add(STRINGS.DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGBLADDER.NOTIFICATION_NAME, false);
            kv.Add(STRINGS.BUILDING.STATUSITEMS.NORESEARCHORDESTINATIONSELECTED.NOTIFICATION_NAME, false);
            kv.Add(STRINGS.DUPLICANTS.MODIFIERS.REDALERT.NAME, false);
            kv.Add(STRINGS.DUPLICANTS.STATUSITEMS.SUFFOCATING.NAME, true);
            kv.Add(STRINGS.BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NAME, false);

            kv.Add(STRINGS.BUILDINGS.PREFABS.STATERPILLARGENERATOR.MODIFIERS.HUNGRY, true); // 饥饿!

            // kv.Add(STRINGS.DUPLICANTS.STATUSITEMS.SUFFOCATING.NAME, true);

            var str = JsonConvert.SerializeObject(kv, Formatting.Indented);
            Console.WriteLine(str);

            File.WriteAllText(fileName, str);

            var config = JsonConvert.DeserializeObject<SortedDictionary<string, bool>>( File.ReadAllText(fileName ))
             ;

        }
        [TestMethod()]
        public void formatStringTest()
        {
           // Console.WriteLine("周期{0}的报告就绪","ssss");
            string tmp = STRINGS.UI.ENDOFDAYREPORT.NOTIFICATION_TITLE.ToString();//好像不能写死
            Console.WriteLine(tmp);
            Console.WriteLine(STRINGS.UI.ENDOFDAYREPORT.NOTIFICATION_TITLE.text);
            ;
            Console.WriteLine(STRINGS.UI.ENDOFDAYREPORT.NOTIFICATION_TITLE);
            var cyleString = String.Format(STRINGS.UI.ENDOFDAYREPORT.NOTIFICATION_TITLE, 50+"");
            Console.WriteLine(cyleString);
        }

        [TestMethod()]
        public void testHashString()
        {
            MemberInfo[] memberInfos =
            typeof(SimHashes).GetMembers(BindingFlags.Public | BindingFlags.Static);
            string alerta = "";
            for (int i = 0; i < memberInfos.Length; i++)
            {
                alerta += memberInfos[i].Name + " - ";
                alerta += memberInfos[i].GetType().Name + " ";
               // alerta +=  + "\n";
                ;
                typeof(SimHashes).GetEnumValues();
            }
            //Console.WriteLine(alerta);

            System.Type enumType = typeof(SimHashes);
            System.Type enumUnderlyingType = System.Enum.GetUnderlyingType(enumType);
            System.Array enumValues = System.Enum.GetValues(enumType);

            for (int i = 0; i < enumValues.Length; i++)
            {
                // Retrieve the value of the ith enum item.
                object value = enumValues.GetValue(i);

                // Convert the value to its underlying type (int, byte, long, ...)
                object underlyingValue = System.Convert.ChangeType(value, enumUnderlyingType);

                System.Console.Write(value);
                System.Console.WriteLine(underlyingValue);
            }
        }
    }
}
