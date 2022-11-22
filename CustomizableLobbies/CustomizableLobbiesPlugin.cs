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

        public void Start()
        {
            Logger.LogInfo("Starting...");

            Logger.LogInfo("Loading config...");
            VerifyConfig();

            Logger.LogInfo("Modifying maximum players...");
            try
            {
                // yeah i should just use tryget but i already typed this awful code so oh well
                BuildSettings.maxPlayers = ((ConfigEntry<int>)Config[ConfigInfo.PvPLobbyMaxKey]).Value;
                BuildSettings.maxCOOPPlayers = ((ConfigEntry<int>)Config[ConfigInfo.CoopLobbyMaxKey]).Value;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return;
            }
            Logger.LogInfo("Done!");

            Logger.LogInfo("Applying Harmony Patches...");
            mHarmony = Harmony.CreateAndPatchAll(typeof(HarmonyPatches));
            Logger.LogInfo("Done!");
        }
        
        private void VerifyConfig()
        {
            if (Config.Count == 0)
            {
                CreateConfig();
                return;
            }

            // yeah it's hideous, but it works for now
            if (!Config.ContainsKey(ConfigInfo.PvPLobbyMaxKey))
            {
                Config.Bind(ConfigInfo.PvPLobbyMaxKey, ConfigInfo.PvPMaxDefault, new ConfigDescription("The maximum number of players for a PvP lobby"));
            }
            if (!Config.ContainsKey(ConfigInfo.CoopLobbyMaxKey))
            {
                Config.Bind(ConfigInfo.CoopLobbyMaxKey, ConfigInfo.CoopMaxDefault, new ConfigDescription("The maximum number of players for a PvE lobby"));
            }
        }

        private void CreateConfig()
        {
            Config.Bind(ConfigInfo.PvPLobbyMaxKey, ConfigInfo.PvPMaxDefault, new ConfigDescription("The maximum number of players for a PvP lobby"));
            Config.Bind(ConfigInfo.CoopLobbyMaxKey, ConfigInfo.CoopMaxDefault, new ConfigDescription("The maximum number of players for a PvE lobby"));
            Config.Save();
        }

    }
    
}
