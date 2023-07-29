using System.Collections.Generic;

namespace FullModdedFuriesAPI
{
    /// <summary>Metadata about a connected player.</summary>
    public interface IMultiplayerPeer
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The player's unique ID.</summary>
        long PlayerID { get; }

        /// <summary>Whether this is a connection to the host player.</summary>
        bool IsHost { get; }

        /// <summary>Whether this a local player on the same computer in split-screen mote.</summary>
        bool IsSplitScreen { get; }

        /// <summary>Whether the player has FMODF installed.</summary>
        bool HasFmodf { get; }

        /// <summary>The player's screen ID, if applicable.</summary>
        /// <remarks>See <see cref="Context.ScreenId"/> for details. This is only visible to players in split-screen mode. A remote player won't see this value, even if the other players are in split-screen mode.</remarks>
        int? ScreenID { get; }

        /// <summary>The player's OS platform, if <see cref="HasFmodf"/> is true.</summary>
        GamePlatform? Platform { get; }

        /// <summary>The installed version of Full Metal Furies, if <see cref="HasFmodf"/> is true.</summary>
        ISemanticVersion GameVersion { get; }

        /// <summary>The installed version of FMODF, if <see cref="HasFmodf"/> is true.</summary>
        ISemanticVersion ApiVersion { get; }

        /// <summary>The installed mods, if <see cref="HasFmodf"/> is true.</summary>
        IEnumerable<IMultiplayerPeerMod> Mods { get; }


        /*********
        ** Methods
        *********/
        /// <summary>Get metadata for a mod installed by the player.</summary>
        /// <param name="id">The unique mod ID.</param>
        /// <returns>Returns the mod info, or <c>null</c> if the player doesn't have that mod.</returns>
        IMultiplayerPeerMod GetMod(string id);
    }
}
