using Database;
using HarmonyLib;
using JetBrains.Annotations;
using Klei.AI;
using Klei.CustomSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
  
 
using static STRINGS.UI.FRONTEND;

namespace MeteorChangeMod
{
    internal class Patches
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
                if ( CustomSettingsController.Instance != null && CustomSettingsController.Instance.CurrentlyActive)
                    return false;
                return true;
            }
        }

        private static readonly KButtonMenu.ButtonInfo toggleButtonInfo = new KButtonMenu.ButtonInfo(
                (string)STRINGS.UI.CUSTOMGAMESETTINGSCHANGER.BUTTONTEXT, 
                Action.NumActions, 
                new UnityAction(OnCustomMenuButtonPressed));

        static List<KButtonMenu.ButtonInfo> meteorShowersList = null;

        //构建button名单.
        public static void buildButtonList()
        {
            List<KButtonMenu.ButtonInfo> listOut= new List<KButtonMenu.ButtonInfo>();
            // Db.Get().GameplaySeasons;
            // Db.Get().GameplaySeasons.MeteorShowers;
            var fieldList = typeof(Database.GameplaySeasons).GetFields();
            //var fieldList=Type.GetType("Database.GameplaySeasons").GetFields();
            //if(fieldList==null || fieldList.Length==0)
            //    fieldList=Db.Get().GameplaySeasons.GetType().GetFields(); ;

            foreach (var field in fieldList)
            {
               // Database.GameplaySeasons;
                if (field.Name.EndsWith("MeteorShowers"))
                {
                    KButtonMenu.ButtonInfo tmp = new KButtonMenu.ButtonInfo(
                         field.Name,
                         Action.NumActions,
                         new UnityAction(OnCustomMenuButtonPressed));
                    listOut.Add(tmp);
                }
               
             }
            if(meteorShowersList==null)
            {
                meteorShowersList = listOut;
            }
            Debug.LogWarning("buildButtonList菜单大小:" + listOut.Count);
            //var fun = Database.GameplaySeasons.class.get ;
            // Database.GameplaySeasons
            // ClusterManager.Instance.me
        }

        private static void OnCustomMenuButtonPressed()
        {
            PauseScreen.Instance.RefreshButtons(); 
            // CustomSettingsController.ShowWindow();
            GameScheduler.Instance.ScheduleNextFrame("OpenCustomSettings", (System.Action<object>)(_ =>
            {
                PauseScreen.Instance.RefreshButtons();
            }));
        }

        [HarmonyPatch(typeof(PauseScreen), "OnPrefabInit")]
        private static class PauseScreen_OnPrefabInit_Patch
        {
            [UsedImplicitly]
            private static void Postfix(ref IList<KButtonMenu.ButtonInfo> ___buttons)
            {
                List<KButtonMenu.ButtonInfo> list = ___buttons.ToList<KButtonMenu.ButtonInfo>();
                toggleButtonInfo.isEnabled = true;
                // TwitchButtonInfo 
                list.Insert(5, toggleButtonInfo);

                buildButtonList();
                for(int i=0;i< meteorShowersList.Count; i++)
                {
                    list.Insert(5+i, meteorShowersList[i]);
                }

                ___buttons = (IList<KButtonMenu.ButtonInfo>)list;
            }
        }

        [HarmonyPatch(typeof(KButtonMenu), "RefreshButtons")]
        private static class PauseScreen_RefreshButtons_Patch
        {
            static bool showMoreButton=false;
            [UsedImplicitly]
            private static void Postfix(KButtonMenu __instance)
            {
               
                //buildButton
                //List<KButtonMenu.ButtonInfo> list = __instance.SetButtons.ToList<KButtonMenu.ButtonInfo>();

               
            }
        }
 
    }
}

