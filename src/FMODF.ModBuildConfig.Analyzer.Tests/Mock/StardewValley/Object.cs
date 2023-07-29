// ReSharper disable CheckNamespace, InconsistentNaming -- matches Stardew Valley's code
using Netcode;

namespace Brawler2D
{
    /// <summary>A simplified version of Stardew Valley's <c>Brawler2D.Object</c> class for unit testing.</summary>
    public class Object : Item
    {
        /// <summary>A net int field with an equivalent non-net property.</summary>
        public NetInt type = new NetInt { Value = 42 };
    }
}
