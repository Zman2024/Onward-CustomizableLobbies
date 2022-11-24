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
    [HarmonyPatch]
    public class HarmonyPatches
    {
        // MenuServerSettings \\
        
        [HarmonyPatch(typeof(MenuServerSettings), nameof(MenuServerSettings.SetButtonStates))]
        public static void SetButtonStatesPatches(MenuServerSettings __instance, object[] __args, ref MenuItemPopout ___VRSpectatingButton,
            ref MenuItemPopout ___AllowObserversButton, ref MenuItemPopout ___AllowTimeoutButton, ref MenuItemPopout[] ___TimeoutLengthButtons,
            ref MenuItemPopout[] ___RoundCountButtons, ref MenuItemPopout[] ___RoundLengthButtons)
        {
            // Confusing name, but it sets buttonEnabled = !param (false = enabled, true = disabled)
            ___VRSpectatingButton.SetDisabledInvertClickable(false);
            ___AllowObserversButton.SetDisabledInvertClickable(false);
            ___AllowTimeoutButton.SetDisabledInvertClickable(false);

            var exposed = Traverse.Create(__instance);
            var fcnSetButtonsDisabled = exposed.Method("SetButtonsDisabled", new Type[] { typeof(MenuItemPopout[]), typeof(bool) });
            fcnSetButtonsDisabled.GetValue(___TimeoutLengthButtons, false);
            fcnSetButtonsDisabled.GetValue(___RoundCountButtons, false);
            fcnSetButtonsDisabled.GetValue(___RoundLengthButtons, false);

        }

        // MultiplayerCreateServerMenu \\

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MultiplayerCreateServerMenu), "GameModeSelected")]
        public static void GameModeSelectedPostfix(MultiplayerCreateServerMenu __instance)
        {
            __instance.LockToPrivate = false; // Never force rooms to be private
        }

    }
}
