using Klei;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

    }
}
