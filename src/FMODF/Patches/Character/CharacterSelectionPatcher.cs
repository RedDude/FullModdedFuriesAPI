// using Brawler2D;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
// using System.Reflection.Emit;
// using CDGEngine;
// using FullModdedFuriesAPI.Framework.ModHelpers.DatabaseHelper;
// using FullModdedFuriesAPI.Internal.Patching;
// using FullModdedFuriesAPI.Patches.Character;
// using HarmonyLib;
//
// namespace FullModdedFuriesAPI.Patches
// {
//   class CharacterSelectionPatcher : BasePatcher
//     {
//         private GameController game;
//         private Assembly Assembly;
//         private static Type CharacterSelectMenuType;
//
//         // private static int currentCharacter = -1;
//
//         private static bool IsCustomCharacterSelected = false;
//         private static bool IsLastCustomCharacterSelected = false;
//
//         private static int nextCharacter;
//         private static int previousCharacter;
//         private static FieldInfo classTypeSelectorField;
//         private static FieldInfo leftArrowField;
//         private static FieldInfo rightArrowField;
//
//         public CharacterSelectionPatcher()
//         {
//             this.Assembly = typeof(GameController).Assembly;
//             CharacterSelectMenuType = this.Assembly.GetType("Brawler2D.CharacterSelectMenuObj");
//             classTypeSelectorField = CharacterSelectMenuType.GetField("m_classTypeSelector", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
//         }
//
//
//         public override void Apply(Harmony harmony, IMonitor monitor)
//         {
//             this.Patch();
//         }
//
//         public void Patch()
//         {
//             var harmony = new Harmony("CharacterSelectionPatcher");
//
//             //Handle Character Selection
//             harmony.Patch(
//                 original: AccessTools.Method(CharacterSelectMenuType, "SwapClass"),
//                 prefix: this.GetHarmonyMethod(nameof(this.Before_SwapClass))
//             );
//
//             harmony.Patch(
//                 original: AccessTools.Method(CharacterSelectMenuType, "HandleInput"),
//                 transpiler: this.GetHarmonyMethod(nameof(this.HandleInput_Patch))
//             );
//
//             harmony.Patch(
//                 original: AccessTools.Method(CharacterSelectMenuType, "ForceSwapLeft"),
//                 transpiler: this.GetHarmonyMethod(nameof(this.HandleInput_Patch))
//             );
//             // harmony.Patch(
//                 // original: AccessTools.Method(CharacterSelectMenuType, "SwapClassTransition"),
//                 // prefix: this.GetHarmonyMethod(nameof(this.Before_SwapClassTransition))
//             // );
//             // harmony.Patch(
//             //     original: AccessTools.Method(CharacterSelectMenuType, "ForceSwapLeft"),
//             //     prefix: this.GetHarmonyMethod(nameof(this.Before_ForceSwapLeft))
//             // );
//         }
//
//         static IEnumerable<CodeInstruction> HandleInput_Patch(IEnumerable<CodeInstruction> instructions)
//         {
//             bool foundResetMethod = false;
//             int startIndex = -1;
//
//
//             var codes = new List<CodeInstruction>(instructions);
//             for (int i = 0; i < codes.Count; i++)
//             {
//                 if (codes[i].opcode != OpCodes.Call)
//                     continue;
//
//                 string strOperand = codes[i].operand.ToString();
//                 if (strOperand.Contains("ResetPositions"))
//                 {
//                     startIndex = i-1;
//                     foundResetMethod = true;
//                     break;
//                 }
//             }
//
//             if (foundResetMethod)
//                 for (int i = startIndex; i > 0; i--)
//                 {
//                     // if (codes[i].opcode == OpCodes.Ldc_I4_4)
//                         // codes[i].opcode = OpCodes.Ldc_I4_5;
//
//                         if (codes[i].opcode == OpCodes.Ldc_I4_4)
//                         {
//
//                             codes[i].opcode = OpCodes.Call;
//                             codes[i].operand = AccessTools.Method(typeof(CharacterSelectionPatcherUtil),
//                                 "GetCustomCharacterCount");
//                         }
//
//                     if (codes[i].opcode == OpCodes.Pop)
//                         break;
//                 }
//
//             return codes.AsEnumerable();
//         }
//
//         static bool Before_SwapClass(object __instance, ClassType classType)
//         {
//             string className = "";
//             foreach (var customClassPair in DatabaseHelper.CustomCharacter)
//             {
//                 if (customClassPair.Value != (int) classType)
//                     continue;
//
//                 className = DatabaseHelper.classColor.ContainsKey((int) classType) ? DatabaseHelper.classNames[(int) classType] : null;
//                 // className = DatabaseHelper..Database.GetClassTypeName(classType);
//             }
//
//             if (string.IsNullOrWhiteSpace(className))
//                 return true;
//
//             classTypeSelectorField.SetValue(__instance, classType);
//             int stars = 5; //TODO: Solve stars, get it from databaseHelper
//
//             var classTextField = CharacterSelectMenuType.GetField("m_classText", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
//             var classText = (BrawlerTextObj)classTextField.GetValue(__instance);
//
//             var starContainerField = CharacterSelectMenuType.GetField("m_starContainer", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
//             var starContainer = (ContainerObj)starContainerField.GetValue(__instance);
//
//             var portraitField = CharacterSelectMenuType.GetField("m_portrait", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
//             var portrait = (ClickableSpriteObj)portraitField.GetValue(__instance);
//
//             GameController.soundManager.PlayEvent("event:/SFX/Front End/Character Select/sfx_fe_char_change_select", (IPositionalObj) null, false, false);
//             string localeKey = className.ToLower() + ".class.name";
//             classText.Text = Helper.Translation.Get(localeKey).ToString().ToUpper();//className.ToUpper();
//
//             for (int index = 0; index < starContainer.NumChildren; ++index)
//             {
//                 SpriteObj childAt = starContainer.GetChildAt(index) as SpriteObj;
//                 (starContainer.GetChildAt(index) as SpriteObj)?.ChangeSprite(childAt.spriteName);
//                 childAt.ChangeSprite(index <= stars - 1 ? "Difficulty_Star_Filled" : "Difficulty_Star_Empty");
//             }
//             portrait.ChangeSprite("CharSelect_" + className + "_Portrait");
//             var updateText = Helper.Reflection.GetMethod(__instance, "UpdateText");
//             updateText.Invoke();
//
//
//             return false;
//         }
//
//
//         /// <summary>Get a Harmony patch method on the current patcher instance.</summary>
//         /// <param name="name">The method name.</param>
//         /// <param name="priority">The patch priority to apply, usually specified using Harmony's <see cref="Priority"/> enum, or <c>null</c> to keep the default value.</param>
//         protected HarmonyMethod GetHarmonyMethod(string name, int? priority = null)
//         {
//             var method = new HarmonyMethod(
//                 AccessTools.Method(this.GetType(), name)
//                 ?? throw new InvalidOperationException($"Can't find patcher method {name}.")
//             );
//
//             if (priority.HasValue)
//                 method.priority = priority.Value;
//
//             return method;
//         }
//     }
// }
