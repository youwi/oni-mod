 
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
using static NotificationsPauseI18nMod.NotificationsPause;

namespace NotificationsPauseI18nMod
{
   
    public class InitConfig : KMod.UserMod2 
    {
        static string ModPath = null;
      //  public static string ModConfigName = "";
        public static string ModConfigJsonName = "";
        public override void OnLoad(Harmony harmony)
        {
            ModPath = mod.ContentPath;
            ModConfigJsonName = mod.ContentPath + "/../../NotificationsPauseI18n.json";
          //  ModConfigName = mod.ContentPath + "/../../NotificationsPauseI18n.yaml";
            base.OnLoad(harmony);
        }
    }

    public static class NotificationsPause
    {
        public static SettingsFile settings=new SettingsFile();
        public static bool tryReadOnce = true;
        public static long tryWriteOnceTick = 0;//打算每几分钟自动刷新
        public static long lastLoadTick = 0;// 
        public static float lastPause = 0f;
        public class SettingsFile
        {
            public string fileversion="0.1";
            public float cooldown=10;
            public SortedDictionary<string, bool> PauseOnNotification;
            public  void addKeyAndSave(string keyString)
            {
                PauseOnNotification.Add(keyString, false);
                File.WriteAllText(InitConfig.ModConfigJsonName, JsonConvert.SerializeObject(PauseOnNotification, Formatting.Indented));
               // YamlIO.Save(PauseOnNotification, InitConfig.ModConfigName);
            }
            public static void loadSett()
            {
                if(!File.Exists(InitConfig.ModConfigJsonName))
               // if (!File.Exists(InitConfig.ModConfigName))
                {
                    //  翻译表:
                    //  STRINGS.CREATURES.STATUSITEMS.ATTACK.NAME 战斗!
                    //  STRINGS.DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGBLADDER.NOTIFICATION_NAME 失禁
                    //  STRINGS.BUILDING.STATUSITEMS.NORESEARCHORDESTINATIONSELECTED.NOTIFICATION_NAME  未选择研究方向
                    //  
                    //  STRINGS.DUPLICANTS.MODIFIERS.REDALERT.NAME   红色警报
                    //  STRINGS.DUPLICANTS.STATUSITEMS.SUFFOCATING.NAME 窒息
                    //  STRINGS.BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NAME  顶级优先度

                    SortedDictionary<string, bool> kv = new SortedDictionary<string, bool>();
                    kv.Add(STRINGS.CREATURES.STATUSITEMS.ATTACK.NAME, false);
                    kv.Add(STRINGS.DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGBLADDER.NOTIFICATION_NAME, false);
                    kv.Add(STRINGS.BUILDING.STATUSITEMS.NORESEARCHORDESTINATIONSELECTED.NOTIFICATION_NAME, false);
                    kv.Add(STRINGS.DUPLICANTS.MODIFIERS.REDALERT.NAME, false);
                    kv.Add(STRINGS.DUPLICANTS.STATUSITEMS.SUFFOCATING.NAME, true);
                    kv.Add(STRINGS.BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NAME, false);

                    kv.Add(STRINGS.BUILDINGS.PREFABS.STATERPILLARGENERATOR.MODIFIERS.HUNGRY, true); // 饥饿!

                    // kv.Add(STRINGS.DUPLICANTS.STATUSITEMS.SUFFOCATING.NAME, true);

                    //方案A: YAML
                    //NotificationsPause.settings.PauseOnNotification = kv;
                    //YamlIO.Save(kv, fileNamePlanB);
                    //方案B: JSON
                    File.Create(InitConfig.ModConfigJsonName).Close();
                    File.WriteAllText(InitConfig.ModConfigJsonName, 
                        JsonConvert.SerializeObject(kv, Formatting.Indented));
                };
                try
                {
                    //方案A:YAML
                    //var config = YamlIO.LoadFile<SortedDictionary<string, bool>>(fileNamePlanB);
                    //NotificationsPause.settings.PauseOnNotification = config;
                    //方案B: JSON
                    var config = JsonConvert.DeserializeObject<SortedDictionary<string, bool>>(
                        File.ReadAllText(InitConfig.ModConfigJsonName));
                    NotificationsPause.settings.PauseOnNotification = config;

                    lastLoadTick = System.DateTime.Now.Ticks;
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
            }
        }

        [HarmonyPatch(typeof(Notification), "IsReady")]
        public static class Notification_IsReady_Patch
        {
            private static void readSettings_backUp()
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
                catch (Exception e)
                {
                    sr.Close();
                    sr.Dispose();
                    Debug.Log("Critical Notification Pauser: Error reading Json :"+e.Message);
                }
                sr.Close();
                sr.Dispose();
            }

            public static bool isConkey(SortedDictionary<string, bool> kv, string searchKey)
            {
                if(kv.ContainsKey(searchKey))
                {
                    return kv[searchKey];
                }
                foreach(var key in kv.Keys)
                {
                    if(key.Contains(searchKey))
                    {
                        return kv[key];
                    }
                }
                return false;
            }
            public static void Postfix(ref Notification __instance)
            {

                if (tryReadOnce == false) //第二次
                {
                    // System.DateTime.now  /10000/1000 秒
                    var tickPoo = (System.DateTime.Now.Ticks - tryWriteOnceTick)/ 10000 / 1000 ;
                     
                    if (tickPoo  >  5) //1秒刷新一次
                    {
                        var tickOff = (File.GetLastWriteTime(InitConfig.ModConfigJsonName).Ticks - lastLoadTick)/ 10000 / 1000 ;
                        if (tickOff > 5)
                        {
                            Debug.LogWarning($"read::::{InitConfig.ModConfigJsonName}  /{tickPoo}/{tickOff}");
                            SettingsFile.loadSett();
                        }
                        tryWriteOnceTick =System.DateTime.Now.Ticks;
                    }
                }
                else
                {
                    SettingsFile.loadSett();
                    tryReadOnce = false;
                    tryWriteOnceTick = System.DateTime.Now.Ticks    ;
                }

                if (!settings.PauseOnNotification.ContainsKey(__instance.titleText))
                { //如果key没有的话就保存它
                    settings.addKeyAndSave(__instance.titleText);
                }
            
 
                if ((!(SpeedControlScreen.Instance.IsPaused)) 
                    && settings != null 
                    && settings.PauseOnNotification != null 
                    && settings.PauseOnNotification.ContainsKey(__instance.titleText))
                {
                    if (Time.time - lastPause > settings.cooldown)
                    {
                        //If the title is set in the settings, use that
                       
                        if (isConkey(settings.PauseOnNotification,__instance.titleText) )
                        {
                            SpeedControlScreen.Instance.Pause();
                          //  Debug.LogWarning($"暂停:  {__instance.titleText} ");

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
                                Debug.LogWarning($"暂停PlanB:  {__instance.titleText} ");
                                lastPause = Time.time;
                            }
                        }
                    }
                }
            }
        }
    }
}