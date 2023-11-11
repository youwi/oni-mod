using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace MeteorChangeMod
{
    // Token: 0x02000002 RID: 2
    public class BaseRenamerPatches
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public static void OnClickRenameBase(GameOptionsScreen gameOptionsScreen)
        {
            FileNameDialog fileNameDialog = (FileNameDialog)KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.FileNameDialog.gameObject, gameOptionsScreen.transform.parent.gameObject);
            fileNameDialog.onConfirm = delegate (string baseName)
            {
                SaveGame.Instance.SetBaseName(baseName.Substring(0, baseName.Length - 4));
                TopLeftControlScreen.Instance.RefreshName();
            };
            fileNameDialog.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<LocText>().SetText("Rename Base");
        }

        // Token: 0x06000002 RID: 2 RVA: 0x000020DB File Offset: 0x000002DB
        //private static IEnumerable<CodeInstruction> ReFileNameIze(IEnumerable<CodeInstruction> instructions)
        //{
        //    //MethodInfo GetFNWithoutExtensionMethodInfo = AccessTools.Method("System.IO.Path:GetFileNameWithoutExtension", null, null);
        //    //MethodInfo methodInfo = AccessTools.Method("System.IO.Path:GetFileName", null, null);
        //    //CodeInstruction callGetFileName = new CodeInstruction(OpCodes.Call, methodInfo);
        //    //foreach (CodeInstruction codeInstruction in instructions)
        //    //{
        //    //    if (codeInstruction.opcode == OpCodes.Call)
        //    //    {
        //    //        if (codeInstruction.operand != null)
        //    //        {
        //    //            global::Debug.Log(codeInstruction.operand.ToString());
        //    //        }
        //    //        if ((MethodInfo)codeInstruction.operand == GetFNWithoutExtensionMethodInfo)
        //    //        {
        //    //            yield return callGetFileName;
        //    //        }
        //    //        else
        //    //        {
        //    //            yield return codeInstruction;
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        yield return codeInstruction;
        //    //    }
        //    //}
        //    //IEnumerator<CodeInstruction> enumerator = null;
        //    //yield break;
        //    return;
        //}

        // Token: 0x02000003 RID: 3
        [HarmonyPatch(typeof(GameOptionsScreen))]
        [HarmonyPatch("OnPrefabInit")]
        public static class GameOptionsScreenOnPrefabInit
        {
            // Token: 0x06000004 RID: 4 RVA: 0x000020F4 File Offset: 0x000002F4
            public static void Postfix(GameOptionsScreen __instance)
            {
                if (Game.Instance == null)
                {
                    return;
                }
                Traverse traverse = Traverse.Create(__instance);
                IList<KButtonMenu.ButtonInfo> value = new KButtonMenu.ButtonInfo[]
                {
                    new KButtonMenu.ButtonInfo("Rename Base Demo", global::Action.CinemaZoomIn, delegate()
                    {
                        BaseRenamerPatches.OnClickRenameBase(__instance);
                    }, null, null)
                };
                traverse.Field("buttons").SetValue(value);
                traverse.Field("buttonPrefab").SetValue(PauseScreen.Instance.buttonPrefab);
                traverse.Field("keepMenuOpen").SetValue(true);
            }
        }

        // Token: 0x02000004 RID: 4
        [HarmonyPatch(typeof(GameOptionsScreen))]
        [HarmonyPatch("OnSpawn")]
        public static class GameOptionsScreenOnSpawn
        {
            // Token: 0x06000005 RID: 5 RVA: 0x00002194 File Offset: 0x00000394
            public static void Postfix(GameOptionsScreen __instance)
            {
                if (Game.Instance == null)
                {
                    return;
                }
                Traverse traverse = Traverse.Create(__instance);
                GameObject gameObject = __instance.buttonObjects[0];
                Transform transform = traverse.Field("savePanel").GetValue<GameObject>().transform;
                gameObject.transform.parent = transform;
            }
        }

        // Token: 0x02000005 RID: 5
        [HarmonyPatch(typeof(SaveScreen))]
        [HarmonyPatch("AddExistingSaveFile")]
        public static class SaveScreenAddExistingSaveFile
        {
            // Token: 0x06000006 RID: 6 RVA: 0x000021DF File Offset: 0x000003DF
            //private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            //{
            //    return BaseRenamerPatches.ReFileNameIze(instructions);
            //}
        }

        // Token: 0x02000006 RID: 6
        [HarmonyPatch(typeof(SaveScreen))]
        [HarmonyPatch("Save")]
        public static class SaveScreenSave
        {
            // Token: 0x06000007 RID: 7 RVA: 0x000021E7 File Offset: 0x000003E7
            //private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            //{
            //    return BaseRenamerPatches.ReFileNameIze(instructions);
            //}
        }

        // Token: 0x02000007 RID: 7
        [HarmonyPatch(typeof(LoadScreen))]
        [HarmonyPatch("AddExistingSaveFile")]
        public static class LoadScreenAddExistingSaveFile
        {
            // Token: 0x06000008 RID: 8 RVA: 0x000021EF File Offset: 0x000003EF
            //private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            //{
            //    return BaseRenamerPatches.ReFileNameIze(instructions);
            //}
        }

        // Token: 0x02000008 RID: 8
        [HarmonyPatch(typeof(PauseScreen))]
        [HarmonyPatch("OnSave")]
        public static class PauseScreenOnSave
        {
            // Token: 0x06000009 RID: 9 RVA: 0x000021F7 File Offset: 0x000003F7
            //private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            //{
            //    return BaseRenamerPatches.ReFileNameIze(instructions);
            //}
        }
    }
}
