namespace FullModdedFuriesAPI.Mods.ConsoleCommands.Framework.ItemData
{
    /// <summary>An item type that can be searched and added to the player through the console.</summary>
    internal enum ItemType
    {
        /// <summary>A big craftable object in <see cref="Brawler2D.Game1.bigCraftablesInformation"/></summary>
        BigCraftable,

        /// <summary>A <see cref="Brawler2D.Objects.Boots"/> item.</summary>
        Boots,

        /// <summary>A <see cref="Brawler2D.Objects.Clothing"/> item.</summary>
        Clothing,

        /// <summary>A <see cref="Brawler2D.Objects.Wallpaper"/> flooring item.</summary>
        Flooring,

        /// <summary>A <see cref="Brawler2D.Objects.Furniture"/> item.</summary>
        Furniture,

        /// <summary>A <see cref="Brawler2D.Objects.Hat"/> item.</summary>
        Hat,

        /// <summary>Any object in <see cref="Brawler2D.Game1.objectInformation"/> (except rings).</summary>
        Object,

        /// <summary>A <see cref="Brawler2D.Objects.Ring"/> item.</summary>
        Ring,

        /// <summary>A <see cref="Brawler2D.Tool"/> tool.</summary>
        Tool,

        /// <summary>A <see cref="Brawler2D.Objects.Wallpaper"/> wall item.</summary>
        Wallpaper,

        /// <summary>A <see cref="Brawler2D.Tools.MeleeWeapon"/> or <see cref="Brawler2D.Tools.Slingshot"/> item.</summary>
        Weapon
    }
}
