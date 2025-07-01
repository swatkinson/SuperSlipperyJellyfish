using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;

namespace SuperSlipperyJellyfish
{
    [BepInPlugin(ModInfo.guid, ModInfo.name, ModInfo.ver)]
    public class Base : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(ModInfo.guid);
        public static ConfigEntry<int> multiplier = null!;
        public static ConfigEntry<bool> volume = null!;
        private void Awake()
        {
            Log.Init(ModInfo.initMessage);
            BindConfig();
            harmony.PatchAll(typeof(JellyfishPatch));
        }

        private void BindConfig()
        {
            multiplier = Config.Bind("Super Slippery Jellyfish", "Multiplier", 2, "The multiplier the jellyfish slipperiness is multiplied by. Is 2 by default.");
            volume = Config.Bind("Super Slippery Jellyfish", "Multiply volume?", true, "Is the SFX's volume also multiplied by the multiplier? Is true by default.");
        }
    }
}
