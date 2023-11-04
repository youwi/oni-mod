
using Epic.OnlineServices.Platform;
using HarmonyLib;
using Klei;
 
using System;
using UnityEngine;

namespace InfiniteResearch
{
    [Serializable]
    public class Options  :KMod.UserMod2
    {
        private const string min = "Minimum";
        private const string max = "Maximum";
        private const string minDesc = "The lowest level at which a dupe can operate this station";
        private const string maxDesc = "The highest level at which a dupe can operate this station";
        private const string exp = "Experience Rate";
        private const string expDesc = "How quickly duplicants gain experience while working at this station.  Vanilla defaults to 1.11.";

        public override void OnLoad(Harmony harmony)
        {
            string fileName = mod.ContentPath + "/../../InfiniteResearchMod.yaml";
            var config = new OptionsIn();
            try
            {
                config = YamlIO.LoadFile<OptionsIn>(fileName);
                Options.ResearchCenterMin = config.ResearchCenterMin;
                Options.ResearchCenterMax = config.ResearchCenterMax;
                Options.AdvancedResearchCenterExpRate = config.AdvancedResearchCenterExpRate;
                Options.AdvancedResearchCenterExpRate = config.AdvancedResearchCenterExpRate;
                Options.CosmicResearchCenterExpRate= config.CosmicResearchCenterExpRate;
                Options.ResearchCenterExpRate = config.ResearchCenterExpRate;
                Options.CosmicResearchCenterMin = config.CosmicResearchCenterMin;
                Options.TelescopeExpRate = config.TelescopeExpRate;
                Options.TelescopeMin = config.TelescopeMin;
                Options.AdvancedResearchCenterMax = config.AdvancedResearchCenterMax;
                Options.AdvancedResearchCenterMin = config.AdvancedResearchCenterMin;
                Options.ResearchCenterExpRate = config.ResearchCenterExpRate;
                Options.CosmicResearchCenterMax = config.CosmicResearchCenterMax;
              
            }
            catch (Exception ex)
            {
                 global::Debug.LogWarning(ex.Message);
                YamlIO.Save(config, fileName);

            }
            base.OnLoad(harmony);
        }
    

        [SerializeField] public static int ResearchCenterMin { get; set; } = 0;

        [SerializeField] public static int ResearchCenterMax { get; set; } = 500;

        [SerializeField] public static float ResearchCenterExpRate { get; set; } = 1;

   
        [SerializeField] public static int AdvancedResearchCenterMin { get; set; }=0;

        [SerializeField] public static int AdvancedResearchCenterMax { get; set; } = 500;
    
        [SerializeField] public static float AdvancedResearchCenterExpRate { get; set; }= 1;

 
        [SerializeField] public static int TelescopeMin { get; set; } = 0;
  
        [SerializeField] public static int TelescopeMax { get; set; } = 500;

        [SerializeField] public static float TelescopeExpRate { get; set; } = 1;

    
        [SerializeField] public static int CosmicResearchCenterMin { get; set; } = 0;
   
        [SerializeField] public static int CosmicResearchCenterMax { get; set; } = 500;

        [SerializeField] public static float CosmicResearchCenterExpRate { get; set; } = 1;
        [Serializable]
        public class OptionsIn
        {
            [SerializeField] public   int ResearchCenterMin { get; set; } = 0;

            [SerializeField] public   int ResearchCenterMax { get; set; } = 500;

            [SerializeField] public   float ResearchCenterExpRate { get; set; } = 1;


            [SerializeField] public   int AdvancedResearchCenterMin { get; set; } = 0;

            [SerializeField] public   int AdvancedResearchCenterMax { get; set; } = 500;

            [SerializeField] public   float AdvancedResearchCenterExpRate { get; set; } = 1;


            [SerializeField] public   int TelescopeMin { get; set; } = 0;

            [SerializeField] public   int TelescopeMax { get; set; } = 500;

            [SerializeField] public   float TelescopeExpRate { get; set; } = 1;


            [SerializeField] public   int CosmicResearchCenterMin { get; set; } = 0;

            [SerializeField] public   int CosmicResearchCenterMax { get; set; } = 500;

            [SerializeField] public   float CosmicResearchCenterExpRate { get; set; } = 1;
        }
  
    }
}
