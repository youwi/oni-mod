using HarmonyLib;
using JetBrains.Annotations;
using Klei.AI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using STRINGS;
using System.Collections;

namespace MeteorChangeMod
{
    internal class MeteorChangePatches
    {
        [HarmonyPatch(typeof(CustomGameSettings))]
        [HarmonyPatch("OnDeserialized")]

        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {

            public static void Postfix(CustomGameSettings __instance)
            {
                //if (!__instance.CurrentQualityLevelsBySetting.ContainsKey(CustomGameSettingConfigs.MeteorShowers.id))
                //{
                //    __instance.CurrentQualityLevelsBySetting.Add(CustomGameSettingConfigs.MeteorShowers.id, "Default");
                //}
            }
        }

        [HarmonyPatch(typeof(PauseScreen))]
        [HarmonyPatch(nameof(PauseScreen.OnKeyDown))]
        public static class CatchGoingBack
        {
            public static bool Prefix(KButtonEvent e)
            {
                if (__CustomSettingsController.Instance != null && __CustomSettingsController.Instance.CurrentlyActive)
                    return false;
                return true;
            }
        }

        private static readonly KButtonMenu.ButtonInfo toggleButtonInfo = new KButtonMenu.ButtonInfo(
                (string)"Change "+STRINGS.UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.ALL_NAME,
                Action.NumActions,
                new UnityAction(OnCustomMenuButtonPressed));

        //STRINGS.UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.ALL_NAME
        public static void setCurrWorldMeteor(string name)
        {

            toggleMyBtn(PauseScreen.Instance);
            toggleButtonInfo.text = $"=>[{name}]<=";
            PauseScreen.Instance.RefreshButtons();
           var smi= ClusterManager.Instance.activeWorld
              .GetSMI<GameplaySeasonManager.Instance>();
            // GameplaySeasonManager.Instance;
            ClusterManager.Instance.activeWorld.GetSeasonIds().Clear();
            ClusterManager.Instance.activeWorld.GetSeasonIds() .Add(name);
            //smi.activeSeasons 
            //ClusterManager.Instance.get
            //WorldContainer wc=new WorldContainer();
            //wc.GetSeasonIds();
        }
        static  IDictionary meteorShowerskeyValues = new System.Collections.Generic.Dictionary<string, string>();
        static  List<KButtonMenu.ButtonInfo> meteorShowersBtnList = new List<KButtonMenu.ButtonInfo>();

        public static  void GetDescriptors(string mskeyString)
        {

           // var s = Db.Get().GameplaySeasons.MarshyMoonletMeteorShowers.events[1];
           
          
            Db.Get().GameplaySeasons.Get("");
           
            var trans = Db.Get().GameplaySeasons.MeteorShowers.events;// [1] = s;//全事件
            List<Descriptor> list = new List<Descriptor>();
            foreach (GameplayEvent gameplayEvent in trans)
            {
               var name= gameplayEvent.title;
                //var sMI = ClusterManager.Instance.activeWorld
                //.GetSMI<GameplaySeasonManager.Instance>();
                //if (sMI != null && gameplayEvent is MeteorShowerEvent)
                //{
                //    List<MeteorShowerEvent.BombardmentInfo> meteorsInfo = (gameplayEvent as MeteorShowerEvent).GetMeteorsInfo();
                //    float num = 0f;
                //    foreach (MeteorShowerEvent.BombardmentInfo item2 in meteorsInfo)
                //    {
                //        num += item2.weight;
                //    }

                //    {
                //        foreach (MeteorShowerEvent.BombardmentInfo item3 in meteorsInfo)
                //        {
                //            GameObject prefab = Assets.GetPrefab(item3.prefab);
                //            string formattedPercent = GameUtil.GetFormattedPercent(Mathf.RoundToInt(item3.weight / num * 100f));
                //            string txt = prefab.GetProperName() + " " + formattedPercent;
                //            Descriptor item = new Descriptor(txt, UI.GAMEOBJECTEFFECTS.TOOLTIPS.METEOR_SHOWER_SINGLE_METEOR_PERCENTAGE_TOOLTIP);
                //            list.Add(item);
                //        }

                      
                //    }
                //}
            }
            // GameplayEvent gameplayEvent = Db.Get().GameplayEvents.Get(eventID);
           
            return  ;
        }
        //构建button名单.
        public static void buildButtonList()
        {
            if (meteorShowersBtnList.Count()>0)
            {
                return;
            }
            List<string> meList=new List<string>();
            List<string> preList= new List<string>() {
                "MeteorShowers",
                "TemporalTearMeteorShowers",
                "SpacedOutStyleStartMeteorShowers",
                "SpacedOutStyleRocketMeteorShowers",
                "SpacedOutStyleWarpMeteorShowers",
                "ClassicStyleStartMeteorShowers",
                "ClassicStyleWarpMeteorShowers",
                "TundraMoonletMeteorShowers",
                "MarshyMoonletMeteorShowers",
                "NiobiumMoonletMeteorShowers",
                "WaterMoonletMeteorShowers",
                "RegolithMoonMeteorShowers",
                "MiniMetallicSwampyMeteorShowers",
                "MiniForestFrozenMeteorShowers",
                "MiniBadlandsMeteorShowers",
                "MiniFlippedMeteorShowers",
                "MiniRadioactiveOceanMeteorShowers"
            };
            IDictionary eventUIK = new System.Collections.Generic.Dictionary<GameplayEvent, LocString>();
          
            eventUIK.Add(Db.Get().GameplayEvents.MeteorShowerIronEvent, UI.SPACEDESTINATIONS.COMETS.IRONCOMET.NAME);
            eventUIK.Add(Db.Get().GameplayEvents.MeteorShowerGoldEvent, UI.SPACEDESTINATIONS.COMETS.GOLDCOMET.NAME);
            eventUIK.Add(Db.Get().GameplayEvents.MeteorShowerCopperEvent, UI.SPACEDESTINATIONS.COMETS.COPPERCOMET.NAME);
            eventUIK.Add(Db.Get().GameplayEvents.MeteorShowerDustEvent ,UI.SPACEDESTINATIONS.COMETS.DUSTCOMET.NAME);
            eventUIK.Add(Db.Get().GameplayEvents.MeteorShowerFullereneEvent, UI.SPACEDESTINATIONS.COMETS.FULLERENECOMET.NAME);
            eventUIK.Add(Db.Get().GameplayEvents.GassyMooteorEvent, UI.SPACEDESTINATIONS.COMETS.GASSYMOOCOMET.NAME);
            eventUIK.Add(Db.Get().GameplayEvents.ClusterSnowShower, UI.SPACEDESTINATIONS.COMETS.SNOWBALLCOMET.NAME);
            eventUIK.Add(Db.Get().GameplayEvents.ClusterIceShower, UI.SPACEDESTINATIONS.COMETS.HARDICECOMET.NAME);
            eventUIK.Add(Db.Get().GameplayEvents.ClusterBiologicalShower, UI.SPACEDESTINATIONS.COMETS.SLIMECOMET.NAME); 
            eventUIK.Add(Db.Get().GameplayEvents.ClusterLightRegolithShower, UI.SPACEDESTINATIONS.COMETS.LIGHTDUSTCOMET.NAME);
            eventUIK.Add(Db.Get().GameplayEvents.ClusterRegolithShower, UI.SPACEDESTINATIONS.COMETS.DUSTCOMET.NAME);
            eventUIK.Add(Db.Get().GameplayEvents.ClusterGoldShower, UI.SPACEDESTINATIONS.COMETS.GOLDCOMET.NAME);
            eventUIK.Add(Db.Get().GameplayEvents.ClusterCopperShower, UI.SPACEDESTINATIONS.COMETS.COPPERCOMET.NAME);
            eventUIK.Add(Db.Get().GameplayEvents.ClusterIronShower, UI.SPACEDESTINATIONS.COMETS.IRONCOMET.NAME);
            eventUIK.Add(Db.Get().GameplayEvents.ClusterUraniumShower, UI.SPACEDESTINATIONS.COMETS.URANIUMORECOMET.NAME);
            eventUIK.Add(Db.Get().GameplayEvents.ClusterOxyliteShower, UI.SPACEDESTINATIONS.COMETS.OXYLITECOMET.NAME);
            eventUIK.Add(Db.Get().GameplayEvents.ClusterBleachStoneShower, UI.SPACEDESTINATIONS.COMETS.BLEACHSTONECOMET.NAME);
    
      
            // Db.Get().GameplaySeasons;
            // Db.Get().GameplaySeasons.MeteorShowers;
            var fieldList = typeof(Database.GameplaySeasons).GetFields();
            //var fieldList=Type.GetType("Database.GameplaySeasons").GetFields();
            //if(fieldList==null || fieldList.Length==0)
            //    fieldList=Db.Get().GameplaySeasons.GetType().GetFields(); ;

            //var s=Db.Get().GameplaySeasons.MarshyMoonletMeteorShowers.events[1];
            // s.description;
            //STRINGS.UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.COPPER.NAME;
            // ClusterMapMeteorShower.Instance
            //var sk= fieldList.Where(t =>  t.Name.EndsWith("MeteorShowers") );
            //foreach(var tmp in sk)
            //{
            //   meList.Add( tmp.Name);
            //}
          

            foreach (var field in fieldList)
            {
                // Database.GameplaySeasons;
                if (field.Name.EndsWith("MeteorShowers") && field.Name!= "MeteorShowers")
                {
                    var namei18n = "";
                    GameplaySeason curr = (GameplaySeason)field.GetValue(Db.Get().GameplaySeasons);
                   // var allMeteorShowers = Db.Get().GameplaySeasons.resources;// 使用这个不用反射了.

                    foreach (GameplayEvent gameplayEvent in curr.events)
                    {
                        // curr.Name;//
                        if (eventUIK.Contains(gameplayEvent))
                        {
                            namei18n += " " + eventUIK[gameplayEvent];
                        }
                        else
                        {
                            namei18n += "";
                        }
                     
                       // namei18n += gameplayEvent.GetType().Name;
                    }
                    if (namei18n.Trim() == "" ||namei18n.Length<2)
                    {
                        namei18n = "<"+field.Name+">";
                    }
                    meteorShowerskeyValues.Add(field.Name, namei18n);

                    KButtonMenu.ButtonInfo tmp = new KButtonMenu.ButtonInfo(
                      namei18n,
                      Action.NumActions,
                      () => { setCurrWorldMeteor(field.Name); });
                    //new UnityAction(setCurrWorldMeteor)
                    meteorShowersBtnList.Add(tmp);
                }
            }
            Debug.LogWarning("buildButtonList菜单大小:" + fieldList.Length);
            // Debug.LogWarning("buildButtonList菜单大小:" + listOut.Count);
            //var fun = Database.GameplaySeasons.class.get ;
            // Database.GameplaySeasons
            // ClusterManager.Instance.me
            // STRINGS.NAMEGEN.COLONY.NOUN.COMET
        }

        private static void OnCustomMenuButtonPressed()
        {

            toggleMyBtn(PauseScreen.Instance);
            PauseScreen.Instance.RefreshButtons();
            // CustomSettingsController.ShowWindow();
            GameScheduler.Instance.ScheduleNextFrame("OpenCustomSettings", (System.Action<object>)(_ =>
            {
                PauseScreen.Instance.RefreshButtons();
            }));
        }
        // static IList<KButtonMenu.ButtonInfo> buttonCache = null;
        static IList<KButtonMenu.ButtonInfo> buttonOri = null;//backUp

        [HarmonyPatch(typeof(PauseScreen), "OnPrefabInit")]
        private static class PauseScreen_OnPrefabInit_Patch
        {
            [UsedImplicitly]
            private static void Postfix(ref IList<KButtonMenu.ButtonInfo> ___buttons)
            {
                List<KButtonMenu.ButtonInfo> list = ___buttons.ToList<KButtonMenu.ButtonInfo>();

                toggleButtonInfo.isEnabled = true;
                // toggleButtonInfo.uibutton.hideFlags = 1;
                // toggleButtonInfo.uibutton.visi;
                // toggleButtonInfo.visualizer.
                // TwitchButtonInfo 
                list.Insert(5, toggleButtonInfo);

                buildButtonList();
                // buttonCache = ___buttons;
                buttonOri = list;

                ___buttons = (IList<KButtonMenu.ButtonInfo>)list;
            }
        }

        //  [HarmonyPatch(typeof(KButtonMenu), "RefreshButtons")]

        static bool showMoreButton = false;
        [UsedImplicitly]
        private static void toggleMyBtn(KButtonMenu __instance)
        {
            IList<KButtonMenu.ButtonInfo> buttonCache = buttonOri.ToList();
            if (showMoreButton == false)
            {
                for (int i = 0; i < meteorShowersBtnList.Count; i++)
                {
                    //list.Insert(5 + i, meteorShowersList[i]);
                    buttonCache.Add(meteorShowersBtnList[i]);
                }
                showMoreButton = true;
              //  toggleButtonInfo.text = "++++";
                __instance.SetButtons(buttonCache);
                Debug.LogWarning("---showMoreButton--->" + showMoreButton);
            }
            else
            {

                showMoreButton = false;
                __instance.SetButtons(buttonOri);
               // toggleButtonInfo.text = "---";
                Debug.LogWarning("---showMoreButton--->" + showMoreButton);

            }

        }


    }
}

