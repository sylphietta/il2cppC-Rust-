using System.Runtime.InteropServices;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
//created by sylphietta | write me in telegram: t.me/sylphydev
namespace IL2cppHack
{
    [BepInPlugin("com.boosterfps", "FpsBooster", "1.0.0")]
    public class Loader : BasePlugin
    {
        public override void Load()
        {
          InitializeMemory();
          UnityEngine.Object.DontDestroyOnLoad(base.AddComponent<Manager>().gameObject);
        }

        private void InitializeMemory()
        {
            Harmony harmony = new Harmony("com.boosterfps");
            harmony.PatchAll();
        }
    }
}
