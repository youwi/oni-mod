using HarmonyLib;
using KMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

namespace ForceEruptionMod
{
    // Token: 0x02000002 RID: 2
    [HarmonyPatch(typeof(Geyser))]
    [HarmonyPatch("OnSpawn")]
    public class ForceEruptionPatch : UserMod2
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        private static void Postfix(Geyser __instance)
        {
            __instance.Subscribe<Geyser>(493375141, ForceEruptionPatch.OnRefreshUserMenuDelegate);
        }
        public static void setEmittingState(Geyser gb,bool emitting )
        {
            //,KIconButtonMenu.ButtonInfo cbtn
            Traverse.Create(gb)
             .Field("emitter")
             .GetValue<ElementEmitter>()
             .SetEmitting(emitting);
        }
        public static void timerCallbackFun(object data)
        {
            if(data is Geyser  )
            {
                setEmittingState((Geyser)data,false); return;
            }
        }

        //目标正喷发:STRINGS.BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.NAME_ERUPTING

        // Token: 0x04000001 RID: 1
        private static readonly EventSystem.IntraObjectHandler<Geyser> OnRefreshUserMenuDelegate = 
            new EventSystem.IntraObjectHandler<Geyser>(
                delegate (Geyser component, object data)
        {
            KIconButtonMenu.ButtonInfo button = new KIconButtonMenu.ButtonInfo("status_item_toilet_needs_emptying", 
                "Force Eruption",null , global::Action.SwitchActiveWorld8, null, null, null, "Start the eruption immediately in (10 seconds)", true);
           
            button.onClick = delegate ()
            {
                setEmittingState(component.smi.master, true);
       
                if (button.text == "Force Eruption")
                    button.text = "Stop Eruption";
             
                var st = new System.Timers.Timer(10000);
                st.AutoReset = false;
                st.Enabled = true;
                st.Elapsed += (object data2, ElapsedEventArgs ss) => {
                    setEmittingState(component, false);
                    button.text = "Force Eruption";
                };
                Game.Instance.userMenu.Refresh(component.gameObject);
                
                //button.ref
                st.Start();
                //component.AddTag(component);//定时器搞不定呀...
                //var timer=new System.Threading.Timer(callback: new TimerCallback()    ,component.smi.master,10000,1);
                // var timer2 = new System.Threading.Timer(timerCallbackFun, component.smi.master, 10000, 1);
                //component.smi.master.Set
                // this.text = "Stop Eruption";
                // Timer.
                //component.smi.master.Invoke(() => { }, 10f);
            };
            Game.Instance.userMenu.AddButton(component.gameObject, button, 1f);
           // ButtonMenuSideScreen;
             
           // Game.Instance.userMenu.up.

            //KIconButtonMenu.ButtonInfo button2 = new KIconButtonMenu.ButtonInfo("status_item_toilet_needs_emptying", 
            //      "Stop Eruption", delegate ()
            //  {

            //      setEmittingState(component.smi.master, false);
            //  }, global::Action.SwitchActiveWorld8, null, null, null, "Stop the eruption immediately", true);

            //  Game.Instance.userMenu.AddButton(component.gameObject, button2, 1f);
        });
    }
}
