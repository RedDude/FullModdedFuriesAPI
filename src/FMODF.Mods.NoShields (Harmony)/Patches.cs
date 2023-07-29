using System;
using Brawler2D;
using static Brawler2D.StatusEffect;

namespace FullModdedFuriesAPI.Mods.NoShields
{
    public class Patches
    {
        private static IMonitor Monitor;
        private static ModConfig Config;

        public static void Initialize(IMonitor monitor, ModConfig config)
        {
            Monitor = monitor;
            Config = config;
        }

        public static void ApplyShield_Posfix(EnemyObj enemy)
        {
            try
            {
                if (!BlitNet.Lobby.IsMaster) return;
                if (!Patches.Config.Enabled)
                {
                    return;
                }
                enemy.shieldToApply = StatusEffect.None;
                enemy.RemoveAllPlayerShields(false);
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(ApplyShield_Posfix)}:\n{ex}", LogLevel.Error);
            }
        }
    }
}
