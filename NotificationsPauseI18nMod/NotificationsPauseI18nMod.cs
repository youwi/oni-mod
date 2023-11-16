 
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using Newtonsoft;
using Newtonsoft.Json;
 
using UnityEngine;
using HarmonyLib;
using Klei;

namespace NotificationsPauseI18nMod
{
   
    public class InitConfig : KMod.UserMod2 
    {
        static string ModPath = null;
        public override void OnLoad(Harmony harmony)
        {
            ModPath = mod.ContentPath;
            string fileName = mod.ContentPath + "/../../NotificationsPauseI18n.yaml";
            try
            {
              var  config = YamlIO.LoadFile<NotificationsPause.SettingsFile>(fileName);

            }
            catch (System.Exception ex) {
                if(ex is FileNotFoundException)
                {   //  翻译表:
                    //  STRINGS.CREATURES.STATUSITEMS.ATTACK.NAME 战斗!
                    //  STRINGS.DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGBLADDER.NOTIFICATION_NAME 失禁
                    //  STRINGS.BUILDING.STATUSITEMS.NORESEARCHORDESTINATIONSELECTED.NOTIFICATION_NAME  未选择研究方向
                    //  
                    //  STRINGS.DUPLICANTS.MODIFIERS.REDALERT.NAME   红色警报
                    //  STRINGS.DUPLICANTS.STATUSITEMS.SUFFOCATING.NAME 窒息
                    //  STRINGS.BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NAME  顶级优先度

                    SortedDictionary<string, bool> kv= new SortedDictionary<string, bool>();
                    kv.Add(STRINGS.CREATURES.STATUSITEMS.ATTACK.NAME, false);
                    kv.Add(STRINGS.DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGBLADDER.NOTIFICATION_NAME, false);
                    kv.Add(STRINGS.BUILDING.STATUSITEMS.NORESEARCHORDESTINATIONSELECTED.NOTIFICATION_NAME, false);
                    kv.Add(STRINGS.DUPLICANTS.MODIFIERS.REDALERT.NAME, false);
                    kv.Add(STRINGS.DUPLICANTS.STATUSITEMS.SUFFOCATING.NAME, true);
                    kv.Add(STRINGS.BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NAME, false);

                    kv.Add(STRINGS.DUPLICANTS.STATUSITEMS.SUFFOCATING.NAME, true);

                    var sett = new NotificationsPause.SettingsFile
                    {
                        PauseOnNotification = kv
                    };
                    YamlIO.Save(sett, fileName);
                }
            }

            base.OnLoad(harmony);
        }
    }

    public static class NotificationsPause
    {
        public static SettingsFile settings;
        public static bool tryReadOnce = true;
        public static float lastPause = 0f;
        public class SettingsFile
        {
            public string fileversion="0.1";
            public float cooldown=10;
            public SortedDictionary<string, bool> PauseOnNotification;
        }

        [HarmonyPatch(typeof(Notification), "IsReady")]
        public static class Notification_IsReady_Patch
        {
            private static void readSettings()
            {
                FileInfo sfile = new FileInfo(Assembly.GetExecutingAssembly().Location);
                DirectoryInfo dirInfo = sfile.Directory;
                string settingpath = dirInfo.FullName + "/settings.json";

                FileInfo filechecker = new FileInfo(settingpath);
                if (!filechecker.Exists)
                    return;

                StreamReader sr = new StreamReader(settingpath);
                string settstr = sr.ReadToEnd();
                try
                {
                    settings = Newtonsoft.Json.JsonConvert.DeserializeObject<SettingsFile>(settstr);
                }
                catch (JsonReaderException exc)
                {
                    sr.Close();
                    sr.Dispose();
                    Debug.Log("Critical Notification Pauser: Error reading Json");
                }
                sr.Close();
                sr.Dispose();
            }

            public static void Postfix(ref Notification __instance)
            {
                if (tryReadOnce && settings == null)
                {
                    //First notification, read file and stuff
                    readSettings();
                    tryReadOnce = false;
                }


                if ((!(SpeedControlScreen.Instance.IsPaused)) 
                    && settings != null 
                    && settings.PauseOnNotification != null 
                    && settings.PauseOnNotification.ContainsKey(__instance.titleText))
                {
                    if (Time.time - lastPause > settings.cooldown)
                    {
                        //If the title is set in the settings, use that
                        if (settings.PauseOnNotification[__instance.titleText])
                        {
                            SpeedControlScreen.Instance.Pause();
                            lastPause = Time.time;
                        }
                    }
                }
                else
                {
                    //... otherwise use default behaviour
                    if (__instance.Type == NotificationType.Bad || __instance.Type == NotificationType.DuplicantThreatening)
                    {
                        if (!((__instance.titleText == "Combat!") 
                            || (__instance.titleText == "Missing Research Station") 
                            || (__instance.titleText == "No Researchers assigned") 
                            || (__instance.titleText == "Yellow Alert") 
                            || (__instance.titleText == "Red Alert")))
                        {
                            if (Time.time - lastPause > 1.0)
                            {
                                SpeedControlScreen.Instance.Pause();
                                lastPause = Time.time;
                            }
                        }
                    }
                }
            }
        }
    }
}