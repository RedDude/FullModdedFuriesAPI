using Brawler2D;
using FullModdedFuriesAPI.Framework.ModHelpers.DatabaseHelper;

namespace FullModdedFuriesAPI.Patches.Character
{
    public static class CharacterSelectionPatcherUtil
    {
        public static IModHelper Helper;

        public static ClassType GetCustomCharacterCount()
        {
            return (ClassType) (DatabaseHelper.classNames.Count - 1);
        }

        // public static ClassType GetCustomCharacterCount()
        // {
        //     return (ClassType) (Helper.Database.GetClassesNames().Count-1);
        //     // int count = Helper.Database.GetCustomClasses().Count;
        //     // if (count <= 4)
        //     //     return count switch
        //     //     {
        //     //         0 => OpCodes.Ldc_I4_4,
        //     //         1 => OpCodes.Ldc_I4_5,
        //     //         2 => OpCodes.Ldc_I4_6,
        //     //         3 => OpCodes.Ldc_I4_7,
        //     //         4 => OpCodes.Ldc_I4_8,
        //     //         _ => OpCodes.Ldc_I4_4
        //     //     };
        //     // ModEntry.monitor.LogOnce("For now, only 4 custom character are supported in this version of FMODF");
        //     // return OpCodes.Ldc_I4_8;
        // }

    }
}
