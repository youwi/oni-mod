﻿using HarmonyLib;
using KMod;
using System;
using System.Collections.Generic;
 

namespace MeteorChangeMod
{
    public class Mod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            ModAssets.LoadAssets();
            base.OnLoad(harmony);
          //  SgtLogger.LogVersion(this);
        }
        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
           // CompatibilityNotifications.FlagLoggingPrevention(mods);

        }
    }
}
