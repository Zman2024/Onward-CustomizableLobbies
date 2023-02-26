using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Reflection;

namespace CustomizableLobbies
{
    // I never really liked harmony but man this looks ugly
    [HarmonyPatch]
    public class HarmonyPatches
    {
        public static BepInEx.Logging.ManualLogSource Logger;

        // MenuServerSettings \\
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MenuServerSettings), nameof(MenuServerSettings.SetButtonStates))]
        public static bool SetButtonStatesPatches(MenuServerSettings __instance, object[] __args)
        {
            MenuItemPopout VRSpectatingButton = __instance.GetPrivateField<MenuItemPopout>("VRSpectatingButton");
            MenuItemPopout AllowObserversButton = __instance.GetPrivateField<MenuItemPopout>("AllowObserversButton");
            MenuItemPopout AllowTimeoutButton = __instance.GetPrivateField<MenuItemPopout>("AllowTimeoutButton");

            MenuItemPopout[] TimeoutLengthButtons = __instance.GetPrivateField<MenuItemPopout[]>("TimeoutLengthButtons");
            MenuItemPopout[] RoundCountButtons = __instance.GetPrivateField<MenuItemPopout[]>("RoundCountButtons");
            MenuItemPopout[] RoundLengthButtons = __instance.GetPrivateField<MenuItemPopout[]>("RoundLengthButtons");

            // Confusing name, but it sets buttonEnabled = !param (false = enabled, true = disabled)
            VRSpectatingButton.SetDisabledInvertClickable(false);
            AllowObserversButton.SetDisabledInvertClickable(false);
            AllowTimeoutButton.SetDisabledInvertClickable(false);

            __instance.InvokePrivateMethod("SetButtonsDisabled", TimeoutLengthButtons, false);
            __instance.InvokePrivateMethod("SetButtonsDisabled", RoundCountButtons, false);
            __instance.InvokePrivateMethod("SetButtonsDisabled", RoundLengthButtons, false);

            return false;
        }

        // MultiplayerCreateServerMenu \\

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MultiplayerCreateServerMenu), nameof(MultiplayerCreateServerMenu.GamemodeSelected))]
        public static void GameModeSelectedPostfix(MultiplayerCreateServerMenu __instance)
        {
            __instance.LockToPrivate = false; // Never force rooms to be private
        }

    }
}
