using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace CustomizableLobbies
{
    // I never really liked harmony but man this looks ugly
    public class HarmonyPatches
    {
        // MenuServerSettings \\
        
        [HarmonyPatch(typeof(MenuServerSettings), "SetButtonStates")]
        public static void SetButtonStatesPatches(MenuServerSettings __instance, object[] __args)
        {
            bool disabled = false;
            var exposed = Traverse.Create(__instance);
            exposed.Field<MenuItemPopout>("VRSpectatingButton").Value.SetDisabledInvertClickable(disabled);
            exposed.Field<MenuItemPopout>("AllowObserversButton").Value.SetDisabledInvertClickable(disabled);
            exposed.Field<MenuItemPopout>("AllowTimeoutButton").Value.SetDisabledInvertClickable(disabled);

            var fcnSetButtonsDisabled = exposed.Method("SetButtonsDisabled", new Type[] { typeof(MenuItemPopout[]), typeof(bool) });
            fcnSetButtonsDisabled.GetValue(exposed.Field<MenuItemPopout[]>("TimeoutLengthButtons").Value, disabled);
            fcnSetButtonsDisabled.GetValue(exposed.Field<MenuItemPopout[]>("RoundCountButtons").Value, disabled);
            fcnSetButtonsDisabled.GetValue(exposed.Field<MenuItemPopout[]>("RoundLengthButtons").Value, disabled);

        }

        // MultiplayerCreateServerMenu \\

        [HarmonyPatch(typeof(MultiplayerCreateServerMenu), "GameModeSelected")]
        public static void GameModeSelectedPatched(MultiplayerCreateServerMenu __instance, object[] __args)
        {
            __instance.GameSettings.Gamemode = (OnwardGameMode)__args[0];
            __instance.LockToPrivate = false; // Never force rooms to be private

            var exposed = Traverse.Create(__instance);
            exposed.Field<UnityEngine.GameObject>("ModeSelectMenu").Value.SetActive(false);
            exposed.Field<UnityEngine.GameObject>("MapSelectMenu").Value.SetActive(true);
        }

    }
}
