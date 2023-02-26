using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Harmony;
using HarmonyLib;

namespace CustomizableLobbies
{
    public static class ConfigInfo
    {
        public static readonly string ConfigSection = "CustomizableLobbies";
        public static readonly ConfigDefinition PvPLobbyMaxKey = new ConfigDefinition(ConfigSection, "MaxPlayersPvP");
        public static readonly ConfigDefinition CoopLobbyMaxKey = new ConfigDefinition(ConfigSection, "MaxPlayersCOOP");

        public readonly static int PvPMaxDefault = 10;
        public readonly static int CoopMaxDefault = 4;

    }

    [BepInPlugin("Zman2024-CustomizableLobbies", "Customizable Lobbies", "0.1.0.0")]
    public class CustomizableLobbiesPlugin : BaseUnityPlugin
    {
        private Harmony mHarmony;

        private ConfigEntry<int> MaxPlayersPvP;
        private ConfigEntry<int> MaxPlayersCOOP;

        public void Start()
        {
            // Share logger with the patches
            HarmonyPatches.Logger = this.Logger;
            try
            {
                Logger.LogInfo("Loading config...");
                InitConfig();
                
                Logger.LogInfo("Modifying maximum players...");
                BuildSettings.maxPlayers = MaxPlayersPvP.Value;
                BuildSettings.maxCOOPPlayers = MaxPlayersCOOP.Value;

                Logger.LogInfo("Applying Harmony Patches...");
                mHarmony = Harmony.CreateAndPatchAll(typeof(HarmonyPatches));

                Logger.LogInfo("Finished");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return;
            }
        }
        
        private void InitConfig()
        {
            MaxPlayersPvP = Config.Bind(ConfigInfo.PvPLobbyMaxKey, ConfigInfo.PvPMaxDefault, new ConfigDescription("The maximum number of players for a PvP lobby"));
            MaxPlayersCOOP = Config.Bind(ConfigInfo.CoopLobbyMaxKey, ConfigInfo.CoopMaxDefault, new ConfigDescription("The maximum number of players for a PvE/COOP lobby"));
            Config.Save();
        }

    }
    
}
