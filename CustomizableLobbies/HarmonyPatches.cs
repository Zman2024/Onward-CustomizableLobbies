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
        public static bool SetButtonStates_Prefix(MenuServerSettings __instance, object[] __args)
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

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MenuServerSettings), nameof(MenuServerSettings.SetRoundLengthAndChallengeBtnStateAndInfo))]
        public static void SetRoundLengthAndChallengeBtnStateAndInfo_Postfix(MenuServerSettings __instance)
        {
            // Always allow round changing
            MenuItemPopout[] RoundLengthButtons = __instance.GetPrivateField<MenuItemPopout[]>("RoundLengthButtons");
            __instance.InvokePrivateMethod("SetButtonsDisabled", RoundLengthButtons, false);
        }

        // MultiplayerCreateServerMenu \\

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MultiplayerCreateServerMenu), "IsGameModeForcedPrivate")]
        public static bool IsGameModeForcedPrivate_Prefix(MultiplayerCreateServerMenu __instance, ref bool __result)
        {
            // We dont want the original one to run
            return __result = false;
        }

    }
}
