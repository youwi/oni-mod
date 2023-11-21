using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitHydroponicMod
{

    [HarmonyPatch(typeof(ConduitConsumer), "Consume")]
    public class ConduitConsumerPatch
    {

        public  void Prefix()
        {
            //  ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
           // conduitConsumer.alwaysConsume = false;
           
        }
    }
}
