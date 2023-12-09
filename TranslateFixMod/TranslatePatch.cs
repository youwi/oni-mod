

using HarmonyLib;
using Klei.CustomSettings;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace TranslateFixMod
{
    public class ForceFirstLoad  : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {

            base.OnLoad(harmony);
            Debug.Log("---> ForceFirstLoad translate ");
            //原生调用翻译
            try
            {   //有部分异常
                Localization.Initialize();
            }catch(System.Exception e)
            {
                Debug.LogWarning(e);
            }
            //var dic = Localization.LoadStringsFile(stringDir, false);
            // ModManager
        }
    }
     [HarmonyPatch(typeof(PressureDoorConfig), "CreateBuildingDef")] //创建游戏时就加载
    //[HarmonyPatch(typeof(Game), nameof(Game.Load))] //游戏开始时加载
    public class TestTranslatePath
    {
        public static bool inited = false;
        public static Dictionary<string, string> translateDictionary = null;
        public static BindingFlags staticflags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

        public static void updateLabelAndTooltip<T>(T obj)
        {
            //SettingLevel  都有这2个属性.
            //SettingConfig
            var labelField = Traverse.Create(obj).Field("<label>k__BackingField"); 
            var tooltipField = Traverse.Create(obj).Field("<tooltip>k__BackingField");
         
            if (translateDictionary.ContainsKey((string)labelField.GetValue())
                && translateDictionary.ContainsKey((string)tooltipField.GetValue()))
            {
                translateDictionary.TryGetValue((string)labelField.GetValue(), out var str);
                translateDictionary.TryGetValue((string)tooltipField.GetValue(), out var str2);

                if (str != null && str2 != null)
                {   //这里有特殊语法: 没有setter的反射很麻烦.
                    labelField.SetValue(str);
                    tooltipField.SetValue(str2);
                }
                //Debug.Log($"--->    {obj} {labelField} {str} {str2}  ---- ");
            }
            else
            {
                Debug.LogWarning($"--->   无翻译 {obj} {labelField}  ");
            }

        }
        public static void fixGameSettingTranslate()
        {
            var fieldsOrigal = typeof(CustomGameSettingConfigs).GetFields(staticflags);
            int count = 0;
            int scCount = 0;
            foreach (var fieldOri in fieldsOrigal)
            {
                count++;
                //方案A:
                try
                {
                    var obj = fieldOri.GetValue(null);
                    if (obj == null) continue;
                    if (obj is SettingConfig)
                    {
                        scCount++;
                        var objb = (SettingConfig)obj;
                        updateLabelAndTooltip(objb);
                    }
                    if (obj is ToggleSettingConfig)
                    {
                        var objc = (ToggleSettingConfig)obj;
                        updateLabelAndTooltip(objc.on_level);
                        updateLabelAndTooltip(objc.off_level);
                    }
                    if (obj is ListSettingConfig)
                    {
                        var objd = (ListSettingConfig)obj;
                        var list = objd.GetLevels();
                        if(list == null) continue;
                        foreach (var tmp in list)
                        {
                            if(tmp==null) continue;
                            updateLabelAndTooltip(tmp);
                        }
                    }
                }catch(Exception ex)
                {
                    Debug.LogWarning("----->--出了一异常 --<"+ fieldOri);
                    Debug.LogWarning(ex);
                }
                //方案B:
                //var name = fieldOri.Name;
                //var tmp = typeof(MockCustomGameSettingConfigs).GetField(name, staticflags);
                //if (tmp != null)
                //{
                //    fieldOri.SetValue(null, tmp.GetValue(null));
                //}
            
            }
            Debug.Log($"--->统计翻译次数 GameSetting :{count} {scCount}");

        }

        public static void fixRoomTranslate()
        {
            //var s=  STRINGS.UI.OVERLAYS.ROOMS.NOROOM.TOO_BIG;
            //var tt = RoomConstraints.NO_MESS_STATION;
            var fieldsOrigal = typeof(RoomConstraints).GetFields(staticflags);
            int count = 0;
            int countUn= 0;
            int countNull = 0;
            foreach (var fieldOri in fieldsOrigal)
            {
                var obj=fieldOri.GetValue(null);
                if(obj is RoomConstraints.Constraint)
                {
                    RoomConstraints.Constraint obja = (RoomConstraints.Constraint)obj;
                     ;
                    if(translateDictionary.TryGetValue(obja.name,out var nameZh))
                    {
                        obja.name = nameZh;
                    }
                    else
                    {
                        //未找到的翻译 {0} 包含了这些
                        var tmp = typeof(MockRoomConstraints).GetField(fieldOri.Name, staticflags);
                        if(tmp != null)
                        {

                            // ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION
                            // ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION
                            // fieldOri.SetValue(null, tmp.GetValue(null));
                            RoomConstraints.Constraint objb = (RoomConstraints.Constraint)tmp.GetValue(null);
                            //Debug.Log($" ---  {fieldOri.Name}  {objb.name}---");
                            obja.name = objb.name;
                            obja.description = objb.description;
                        }
                        if (tmp == null)
                            countNull++;
                        countUn++;
                    }
                    if (translateDictionary.TryGetValue(obja.description, out var descZh))
                    {
                        obja.description = descZh;
                    }
                    if (obja.name.Contains("Cots"))
                    {
                        obja.name += " (床铺)";
                        //指定翻译
                    }
                    count++;
                }
            }
            Debug.Log($"--->统计翻译次数 RoomConstraints :{count}  {countUn} {countNull}");

            //   RoomConstraints.
            //   STRINGS.ROOMS.
            //  SelectToolHoverTextCard.UpdateHoverElements();//
        }
        public static void fixTraitTranslate()
        {

            //var dic = Localization.LoadStringsFile(stringDir, false);
            //STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.CUSTOMIZE
            var traits = Db.Get().traits;
            int count = 0;
         
            foreach (var trait in traits.resources)
            {
                //LocString name = trait.Name;//无效.这里是功能阉割了. Stinkiness S: [] H: [0] Value: [MISSING.]
                //if (name.text == name.key.String) //怎么判断没有翻译? 

                // dic.
                var found = translateDictionary.TryGetValue(trait.Name, out var ss);
                if (found)
                {
                    //Debug.LogWarning("--->修正翻译 :" + trait.Name );
                    trait.Name = ss + "(" + trait.Name + ")";
                    trait.description = translateDictionary[trait.description] + " " + trait.description;
                    count++;
                }
                //  trait.Name=dic[trait.Name];
                //  ->翻译对比: Skilled: < link = "MINING1" > Hard Digging </ link > S: [] H: [0] Value: [MISSING.]
                //  outString +=$"--->翻译对比:{name.text} {name.key}\n";
            }
            //翻译
            //Strings.Get(key).String;
            var sk = STRINGS.CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.GERM_COLLECTION_PROGRESS.TOOLTIP;
            Debug.Log("--->统计翻译次数 Trait :" + count);
        }
        public static void Postfix()
        {
            if (inited) { return; }
            inited = true;
            var langKey = Localization.GetCurrentLanguageCode();
            var langFilename = $"{Application.dataPath}/StreamingAssets/strings/strings_preinstalled_{langKey}.po";
            if (!File.Exists(langFilename)) { return; }
            translateDictionary = ReadPoIIIIIIII.TranslatedStringsEnCn(File.ReadAllLines(langFilename, Encoding.UTF8));


            var example = STRINGS.DUPLICANTS.TRAITS.IRONGUT.NAME;
            //铁石胃肠 S: [STRINGS.DUPLICANTS.TRAITS.IRONGUT.NAME] H: [-1000142653] Value: [铁石胃肠]
            Debug.Log($"---> 验证翻译 {Localization.GetCurrentLanguageCode()} {example.text}/{example.key}");

            fixGameSettingTranslate();

            fixTraitTranslate();

            fixRoomTranslate();
            // Localization.OverloadStrings(dic);

            //G:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\StreamingAssets\strings
            //G:/Steam/steamapps/common/OxygenNotIncluded/OxygenNotIncluded_Data/StreamingAssets/strings/strings_preinstalled_zh_klei.po
            // 设置字体:
            //Localization.sFontAsset = Localization.GetFont(Localization.GetDefaultLocale().FontName);

        }
        public void testExample()
        {
            //Localization.Locale locale = Localization.GetLocale(lines);
            //Dictionary<string, string> translated_strings = Localization.ExtractTranslatedStrings(lines, is_template);
            //TMP_FontAsset font = Localization.GetFont(locale.FontName);
            //Localization.SetFont<ConfirmDialogScreen>(screen, font, locale.IsRightToLeft, excluded_members);

            //如何设置字体 ?
            //  ConfirmDialogScreen screen = this.GetConfirmDialog();


            var ttt = STRINGS.DUPLICANTS.TRAITS.IRONGUT.NAME;
            var traitsActions = TUNING.TRAITS.TRAIT_CREATORS;

            foreach (var trAction in traitsActions)
            {
                // trAction.
                // trait.name;
            }
            //ModifierSet.LoadTraits(); 核心代码是直接加载  //二次加载会怎样?
            TUNING.TRAITS.TRAIT_CREATORS.ForEach(delegate (System.Action action)
            {
                action();
            });
            //回调执行完成以后,直接在数据库中存在.

            //示例代码: 这里明显翻译没到位.
            Db.Get().CreateTrait("IronGut", STRINGS.DUPLICANTS.TRAITS.IRONGUT.NAME, STRINGS.DUPLICANTS.TRAITS.IRONGUT.DESC, null, true, null, false, true);
            // Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, disabled_chore_groups, positiveTrait, true);
            // var traits= ModifierSet.traits; //Db.Get().traits
            // var traitGroups = ModifierSet.traitGroups;Db.Get().traitGroups

            //直接调用:
        }
    }

    public class ReadPoIIIIIIII
    {
        public struct Entry
        {
            public string msgctxt;
            public string msgstr;
            public string msgid;

            public bool IsPopulated
            {
                get
                {
                    if (msgctxt != null && msgstr != null)
                    {
                        return msgstr.Length > 0;
                    }

                    return false;
                }
            }
        }
        public static string FixupString(string result)
        {
            result = result.Replace("\\n", "\n");
            result = result.Replace("\\\"", "\"");
            result = result.Replace("<style=“", "<style=\"");
            result = result.Replace("”>", "\">");
            result = result.Replace("<color=^p", "<color=#");
            return result;
        }

        public static string GetParameter(string key, int idx, string[] all_lines)
        {
            if (!all_lines[idx].StartsWith(key))
            {
                return null;
            }

            List<string> list = new List<string>();
            string text = all_lines[idx];
            text = text.Substring(key.Length + 1, text.Length - key.Length - 1);
            list.Add(text);
            for (int i = idx + 1; i < all_lines.Length; i++)
            {
                string text2 = all_lines[i];
                if (!text2.StartsWith("\""))
                {
                    break;
                }

                list.Add(text2);
            }

            string text3 = "";
            foreach (string item in list)
            {
                string text4 = item;
                if (text4.EndsWith("\r"))
                {
                    text4 = text4.Substring(0, text4.Length - 1);
                }

                text4 = text4.Substring(1, text4.Length - 2);
                text4 = FixupString(text4);
                text3 += text4;
            }

            return text3;
        }
        public static Dictionary<string, string> TranslatedStringsEnCn(string[] lines)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Entry entry = default(Entry);

            for (int i = 0; i < lines.Length; i++)
            {
                string text = lines[i];
                if (text == null || text.Length == 0)
                {
                    entry = default(Entry);
                }
                else
                {
                    string parameter = GetParameter("msgctxt", i, lines);
                    if (parameter != null) entry.msgctxt = parameter;

                    parameter = GetParameter("msgid", i, lines);
                    if (parameter != null) entry.msgid = parameter;

                    parameter = GetParameter("msgstr", i, lines);
                    if (parameter != null) entry.msgstr = parameter;

                }

                if (entry.IsPopulated)
                {
                    dictionary[entry.msgid] = entry.msgstr;
                    entry = default(Entry);
                }
            }

            return dictionary;
        }
    }

    public class MockRoomConstraints
    {
        public static RoomConstraints.Constraint CEILING_HEIGHT_4 = new RoomConstraints.Constraint(null, (Room room) => 1 + room.cavity.maxY - room.cavity.minY >= 4, 1, string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.NAME, "4"), string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION, "4"), null);
 
        public static RoomConstraints.Constraint CEILING_HEIGHT_6 = new RoomConstraints.Constraint(null, (Room room) => 1 + room.cavity.maxY - room.cavity.minY >= 6, 1, string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.NAME, "6"), string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION, "6"), null);
 
        public static RoomConstraints.Constraint MINIMUM_SIZE_12 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells >= 12, 1, string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.NAME, "12"), string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, "12"), null);
 
        public static RoomConstraints.Constraint MINIMUM_SIZE_24 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells >= 24, 1, string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.NAME, "24"), string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, "24"), null);
 
        public static RoomConstraints.Constraint MINIMUM_SIZE_32 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells >= 32, 1, string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.NAME, "32"), string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, "32"), null);
 
        public static RoomConstraints.Constraint MAXIMUM_SIZE_64 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells <= 64, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "64"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "64"), null);
 
        public static RoomConstraints.Constraint MAXIMUM_SIZE_96 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells <= 96, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "96"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "96"), null);
 
        public static RoomConstraints.Constraint MAXIMUM_SIZE_120 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells <= 120, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "120"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "120"), null);
        public static RoomConstraints.Constraint DECORATIVE_ITEM = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(GameTags.Decoration), null, 1, string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.NAME, 1), string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.DESCRIPTION, 1), null);
 
        public static RoomConstraints.Constraint DECORATIVE_ITEM_2 = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(GameTags.Decoration), null, 2, string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.NAME, 2), string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.DESCRIPTION, 2), null);
 
        public static RoomConstraints.Constraint DECORATIVE_ITEM_SCORE_20 = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(GameTags.Decoration) && bc.HasTag(RoomConstraints.ConstraintTags.Decor20), null, 1, string.Format(ROOMS.CRITERIA.DECOR20.NAME, "20"), string.Format(ROOMS.CRITERIA.DECOR20.DESCRIPTION, "20"), null);

    }
    //直接复制源代码来操作...
    public class MockCustomGameSettingConfigs
    {
        // Token: 0x040052E6 RID: 21222
        public static SeedSettingConfig WorldgenSeed = new SeedSettingConfig("WorldgenSeed", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLDGEN_SEED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLDGEN_SEED.TOOLTIP, false, false);

        // Token: 0x040052E7 RID: 21223
        public static ListSettingConfig ClusterLayout = new ListSettingConfig("ClusterLayout", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CLUSTER_CHOICE.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CLUSTER_CHOICE.TOOLTIP, null, null, null, -1L, -1L, false, false, "", "", true);

        // Token: 0x040052E8 RID: 21224
        public static SettingConfig SandboxMode = new ToggleSettingConfig("SandboxMode", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SANDBOXMODE.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SANDBOXMODE.TOOLTIP, new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SANDBOXMODE.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SANDBOXMODE.LEVELS.DISABLED.TOOLTIP, 0L, null), new SettingLevel("Enabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SANDBOXMODE.LEVELS.ENABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SANDBOXMODE.LEVELS.ENABLED.TOOLTIP, 0L, null), "Disabled", "Disabled", -1L, -1L, false, true, "", "");

        // Token: 0x040052E9 RID: 21225
        public static SettingConfig FastWorkersMode = new ToggleSettingConfig("FastWorkersMode", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.FASTWORKERSMODE.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.FASTWORKERSMODE.TOOLTIP, new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.FASTWORKERSMODE.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.FASTWORKERSMODE.LEVELS.DISABLED.TOOLTIP, 0L, null), new SettingLevel("Enabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.FASTWORKERSMODE.LEVELS.ENABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.FASTWORKERSMODE.LEVELS.ENABLED.TOOLTIP, 0L, null), "Disabled", "Disabled", -1L, -1L, true, true, "", "");

        // Token: 0x040052EA RID: 21226
        public static SettingConfig SaveToCloud = new ToggleSettingConfig("SaveToCloud", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP, new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.LEVELS.DISABLED.TOOLTIP, 0L, null), new SettingLevel("Enabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.LEVELS.ENABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.LEVELS.ENABLED.TOOLTIP, 0L, null), "Enabled", "Enabled", -1L, -1L, false, false, "", "");

        // Token: 0x040052EB RID: 21227
        public static SettingConfig CalorieBurn = new ListSettingConfig("CalorieBurn", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.TOOLTIP, new List<SettingLevel>
        {
            new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.DISABLED.TOOLTIP, 2L, null),
            new SettingLevel("Easy", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.EASY.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.EASY.TOOLTIP, 1L, null),
            new SettingLevel("Default", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.DEFAULT.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.DEFAULT.TOOLTIP, 0L, null),
            new SettingLevel("Hard", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.HARD.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.HARD.TOOLTIP, 3L, null),
            new SettingLevel("VeryHard", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.VERYHARD.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.VERYHARD.TOOLTIP, 4L, null)
        }, "Default", "Easy", 1L, 8L, false, true, "", "", false);

        // Token: 0x040052EC RID: 21228
        public static SettingConfig ImmuneSystem = new ListSettingConfig("ImmuneSystem", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.TOOLTIP, new List<SettingLevel>
        {
            new SettingLevel("Invincible", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.INVINCIBLE.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.INVINCIBLE.TOOLTIP, 2L, null),
            new SettingLevel("Strong", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.STRONG.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.STRONG.TOOLTIP, 1L, null),
            new SettingLevel("Default", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.DEFAULT.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.DEFAULT.TOOLTIP, 0L, null),
            new SettingLevel("Weak", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.WEAK.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.WEAK.TOOLTIP, 3L, null),
            new SettingLevel("Compromised", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.COMPROMISED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.COMPROMISED.TOOLTIP, 4L, null)
        }, "Default", "Strong", 8L, 8L, false, true, "", "", false);

        // Token: 0x040052ED RID: 21229
        public static SettingConfig Morale = new ListSettingConfig("Morale", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.MORALE.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.MORALE.TOOLTIP, new List<SettingLevel>
        {
            new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.MORALE.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.MORALE.LEVELS.DISABLED.TOOLTIP, 2L, null),
            new SettingLevel("Easy", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.MORALE.LEVELS.EASY.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.MORALE.LEVELS.EASY.TOOLTIP, 1L, null),
            new SettingLevel("Default", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.MORALE.LEVELS.DEFAULT.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.MORALE.LEVELS.DEFAULT.TOOLTIP, 0L, null),
            new SettingLevel("Hard", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.MORALE.LEVELS.HARD.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.MORALE.LEVELS.HARD.TOOLTIP, 3L, null),
            new SettingLevel("VeryHard", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.MORALE.LEVELS.VERYHARD.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.MORALE.LEVELS.VERYHARD.TOOLTIP, 4L, null)
        }, "Default", "Easy", 64L, 8L, false, true, "", "", false);

        // Token: 0x040052EE RID: 21230
        public static SettingConfig Durability = new ListSettingConfig("Durability", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DURABILITY.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DURABILITY.TOOLTIP, new List<SettingLevel>
        {
            new SettingLevel("Indestructible", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DURABILITY.LEVELS.INDESTRUCTIBLE.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DURABILITY.LEVELS.INDESTRUCTIBLE.TOOLTIP, DlcManager.IsPureVanilla() ? 0L : 2L, null),
            new SettingLevel("Reinforced", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DURABILITY.LEVELS.REINFORCED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DURABILITY.LEVELS.REINFORCED.TOOLTIP, 1L, null),
            new SettingLevel("Default", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DURABILITY.LEVELS.DEFAULT.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DURABILITY.LEVELS.DEFAULT.TOOLTIP, DlcManager.IsPureVanilla() ? 2L : 0L, null),
            new SettingLevel("Flimsy", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DURABILITY.LEVELS.FLIMSY.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DURABILITY.LEVELS.FLIMSY.TOOLTIP, 3L, null),
            new SettingLevel("Threadbare", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DURABILITY.LEVELS.THREADBARE.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DURABILITY.LEVELS.THREADBARE.TOOLTIP, 4L, null)
        }, DlcManager.IsPureVanilla() ? "Indestructible" : "Default", DlcManager.IsPureVanilla() ? "Indestructible" : "Reinforced", 512L, 8L, false, true, "", "", false);

        // Token: 0x040052EF RID: 21231
        public static SettingConfig Radiation = new ListSettingConfig("Radiation", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.RADIATION.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.RADIATION.TOOLTIP, new List<SettingLevel>
        {
            new SettingLevel("Easiest", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.RADIATION.LEVELS.EASIEST.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.RADIATION.LEVELS.EASIEST.TOOLTIP, 2L, null),
            new SettingLevel("Easier", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.RADIATION.LEVELS.EASIER.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.RADIATION.LEVELS.EASIER.TOOLTIP, 1L, null),
            new SettingLevel("Default", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.RADIATION.LEVELS.DEFAULT.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.RADIATION.LEVELS.DEFAULT.TOOLTIP, 0L, null),
            new SettingLevel("Harder", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.RADIATION.LEVELS.HARDER.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.RADIATION.LEVELS.HARDER.TOOLTIP, 3L, null),
            new SettingLevel("Hardest", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.RADIATION.LEVELS.HARDEST.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.RADIATION.LEVELS.HARDEST.TOOLTIP, 4L, null)
        }, "Default", "Easier", 4096L, 8L, false, true, "", "", false);

        // Token: 0x040052F0 RID: 21232
        public static SettingConfig Stress = new ListSettingConfig("Stress", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.TOOLTIP, new List<SettingLevel>
        {
            new SettingLevel("Indomitable", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.INDOMITABLE.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.INDOMITABLE.TOOLTIP, 2L, null),
            new SettingLevel("Optimistic", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.OPTIMISTIC.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.OPTIMISTIC.TOOLTIP, 1L, null),
            new SettingLevel("Default", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.DEFAULT.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.DEFAULT.TOOLTIP, 0L, null),
            new SettingLevel("Pessimistic", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.PESSIMISTIC.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.PESSIMISTIC.TOOLTIP, 3L, null),
            new SettingLevel("Doomed", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.DOOMED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.DOOMED.TOOLTIP, 4L, null)
        }, "Default", "Optimistic", 32768L, 8L, false, true, "", "", false);

        // Token: 0x040052F1 RID: 21233
        public static SettingConfig StressBreaks = new ToggleSettingConfig("StressBreaks", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS_BREAKS.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS_BREAKS.TOOLTIP, new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS_BREAKS.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS_BREAKS.LEVELS.DISABLED.TOOLTIP, 1L, null), new SettingLevel("Default", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS_BREAKS.LEVELS.DEFAULT.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS_BREAKS.LEVELS.DEFAULT.TOOLTIP, 0L, null), "Default", "Default", 262144L, 5L, false, true, "", "");

        // Token: 0x040052F2 RID: 21234
        public static SettingConfig CarePackages = new ToggleSettingConfig("CarePackages", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CAREPACKAGES.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CAREPACKAGES.TOOLTIP, new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CAREPACKAGES.LEVELS.DUPLICANTS_ONLY.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CAREPACKAGES.LEVELS.DUPLICANTS_ONLY.TOOLTIP, 1L, null), new SettingLevel("Enabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CAREPACKAGES.LEVELS.NORMAL.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CAREPACKAGES.LEVELS.NORMAL.TOOLTIP, 0L, null), "Enabled", "Enabled", 1310720L, 5L, false, true, "", "");

        // Token: 0x040052F3 RID: 21235
        public static SettingConfig Teleporters = new ToggleSettingConfig("Teleporters", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.TELEPORTERS.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.TELEPORTERS.TOOLTIP, new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.TELEPORTERS.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.TELEPORTERS.LEVELS.DISABLED.TOOLTIP, 1L, null), new SettingLevel("Enabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.TELEPORTERS.LEVELS.ENABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.TELEPORTERS.LEVELS.ENABLED.TOOLTIP, 0L, null), "Enabled", "Enabled", 6553600L, 5L, false, true, "", "");

        // Token: 0x040052F4 RID: 21236
        public static SettingConfig MeteorShowers = new ListSettingConfig("MeteorShowers", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.METEORSHOWERS.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.METEORSHOWERS.TOOLTIP, new List<SettingLevel>
        {
            new SettingLevel("ClearSkies", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.METEORSHOWERS.LEVELS.CLEAR_SKIES.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.METEORSHOWERS.LEVELS.CLEAR_SKIES.TOOLTIP, 2L, null),
            new SettingLevel("Infrequent", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.METEORSHOWERS.LEVELS.INFREQUENT.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.METEORSHOWERS.LEVELS.INFREQUENT.TOOLTIP, 1L, null),
            new SettingLevel("Default", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.METEORSHOWERS.LEVELS.DEFAULT.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.METEORSHOWERS.LEVELS.DEFAULT.TOOLTIP, 0L, null),
            new SettingLevel("Intense", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.METEORSHOWERS.LEVELS.INTENSE.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.METEORSHOWERS.LEVELS.INTENSE.TOOLTIP, 3L, null),
            new SettingLevel("Doomed", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.METEORSHOWERS.LEVELS.DOOMED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.METEORSHOWERS.LEVELS.DOOMED.TOOLTIP, 4L, null)
        }, "Default", "Infrequent", 32768000L, 8L, false, true, "", "", false);
    }

}
