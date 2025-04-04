using System;
using UnityEngine;
using HarmonyLib;
using static ConsoleSystem;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using IL2cppHack.Main.Misc;

namespace IL2cppHack
{
    public class Manager : MonoBehaviour
    {
        [HarmonyPatch(typeof(BasePlayer), "ClientInput")]
        internal static class ClientInputHook
        {
            [HarmonyPostfix]
            internal static void PostFix(BasePlayer __instance, InputState state)
            {
                try
                {
                    if (__instance == null || state == null)
                        return;

                    if (Settings.localPlayer == null)
                    {
                        Settings.localPlayer = LocalPlayer.Entity;
                        if (Settings.localPlayer == null)
                            return;
                    }

                    if (!Settings.localPlayer.IsValid())
                        return;

                    if (!Settings.localPlayer.HasPlayerFlag(BasePlayer.PlayerFlags.Connected))
                        return;

                    if (Settings.FakeAdmin)
                        Settings.localPlayer.playerFlags |= BasePlayer.PlayerFlags.IsAdmin;

                }
                catch (Exception ex)
                {
                    Debug.LogError($"Exception in PostFix: {ex.ToString()}");
                }
            }
        }


        [HarmonyPatch(typeof(ModelState), "set_flying")]
        internal static class SetFlyingHook
        {
            [HarmonyPrefix]
            internal static bool Prefix(ModelState __instance, bool value)
            {
                if (__instance == null) return true;
                if (value == true) return false;
                return true;
            }
        }
        [HarmonyPatch(typeof(ConsoleSystem), "Run", new Type[] { typeof(Option), typeof(string), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
        public static class RunHook
        {
            public static void Prefix(Option options, string strCommand, Il2CppReferenceArray<Il2CppSystem.Object> args)
            {

                if (options.IsFromServer)
                {
                    if (strCommand.Equals("noclip") || strCommand.Equals("debugcamera") || strCommand.Equals("camlerp")
                        || strCommand.Equals("camspeed") || strCommand.Equals("admintime"))

                    {
                        strCommand = string.Empty;

                    }
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.N))
                Settings.FakeAdmin = !Settings.FakeAdmin;
        }
    }
}
