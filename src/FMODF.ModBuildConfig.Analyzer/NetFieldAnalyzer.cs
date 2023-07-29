// using System;
// using System.Collections.Generic;
// using System.Collections.Immutable;
// using System.Linq;
// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CSharp;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
// using Microsoft.CodeAnalysis.Diagnostics;
//
// namespace FullModdedFuriesAPI.ModBuildConfig.Analyzer
// {
//     /// <summary>Detects implicit conversion from Full Metal Furies's <c>Netcode</c> types. These have very unintuitive implicit conversion rules, so mod authors should always explicitly convert the type with appropriate null checks.</summary>
//     [DiagnosticAnalyzer(LanguageNames.CSharp)]
//     public class NetFieldAnalyzer : DiagnosticAnalyzer
//     {
//         /*********
//         ** Fields
//         *********/
//         /// <summary>The namespace for Full Metal Furies's <c>Netcode</c> types.</summary>
//         private const string NetcodeNamespace = "Netcode";
//
//         /// <summary>Maps net fields to their equivalent non-net properties where available.</summary>
//         private readonly IDictionary<string, string> NetFieldWrapperProperties = new Dictionary<string, string>
//         {
//             // AnimatedSprite
//             ["Brawler2D.AnimatedSprite::currentAnimation"] = "CurrentAnimation",
//             ["Brawler2D.AnimatedSprite::currentFrame"] = "CurrentFrame",
//             ["Brawler2D.AnimatedSprite::sourceRect"] = "SourceRect",
//             ["Brawler2D.AnimatedSprite::spriteHeight"] = "SpriteHeight",
//             ["Brawler2D.AnimatedSprite::spriteWidth"] = "SpriteWidth",
//
//             // Character
//             ["Brawler2D.Character::currentLocationRef"] = "currentLocation",
//             ["Brawler2D.Character::facingDirection"] = "FacingDirection",
//             ["Brawler2D.Character::name"] = "Name",
//             ["Brawler2D.Character::position"] = "Position",
//             ["Brawler2D.Character::scale"] = "Scale",
//             ["Brawler2D.Character::speed"] = "Speed",
//             ["Brawler2D.Character::sprite"] = "Sprite",
//
//             // Chest
//             ["Brawler2D.Objects.Chest::tint"] = "Tint",
//
//             // Farmer
//             ["Brawler2D.Farmer::houseUpgradeLevel"] = "HouseUpgradeLevel",
//             ["Brawler2D.Farmer::isMale"] = "IsMale",
//             ["Brawler2D.Farmer::items"] = "Items",
//             ["Brawler2D.Farmer::magneticRadius"] = "MagneticRadius",
//             ["Brawler2D.Farmer::stamina"] = "Stamina",
//             ["Brawler2D.Farmer::uniqueMultiplayerID"] = "UniqueMultiplayerID",
//             ["Brawler2D.Farmer::usingTool"] = "UsingTool",
//
//             // Forest
//             ["Brawler2D.Locations.Forest::netTravelingMerchantDay"] = "travelingMerchantDay",
//             ["Brawler2D.Locations.Forest::netLog"] = "log",
//
//             // FruitTree
//             ["Brawler2D.TerrainFeatures.FruitTree::greenHouseTileTree"] = "GreenHouseTileTree",
//             ["Brawler2D.TerrainFeatures.FruitTree::greenHouseTree"] = "GreenHouseTree",
//
//             // GameLocation
//             ["Brawler2D.GameLocation::isFarm"] = "IsFarm",
//             ["Brawler2D.GameLocation::isOutdoors"] = "IsOutdoors",
//             ["Brawler2D.GameLocation::lightLevel"] = "LightLevel",
//             ["Brawler2D.GameLocation::name"] = "Name",
//
//             // Item
//             ["Brawler2D.Item::category"] = "Category",
//             ["Brawler2D.Item::netName"] = "Name",
//             ["Brawler2D.Item::parentSheetIndex"] = "ParentSheetIndex",
//             ["Brawler2D.Item::specialVariable"] = "SpecialVariable",
//
//             // Junimo
//             ["Brawler2D.Characters.Junimo::eventActor"] = "EventActor",
//
//             // LightSource
//             ["Brawler2D.LightSource::identifier"] = "Identifier",
//
//             // Monster
//             ["Brawler2D.Monsters.Monster::damageToFarmer"] = "DamageToFarmer",
//             ["Brawler2D.Monsters.Monster::experienceGained"] = "ExperienceGained",
//             ["Brawler2D.Monsters.Monster::health"] = "Health",
//             ["Brawler2D.Monsters.Monster::maxHealth"] = "MaxHealth",
//             ["Brawler2D.Monsters.Monster::netFocusedOnFarmers"] = "focusedOnFarmers",
//             ["Brawler2D.Monsters.Monster::netWildernessFarmMonster"] = "wildernessFarmMonster",
//             ["Brawler2D.Monsters.Monster::slipperiness"] = "Slipperiness",
//
//             // NPC
//             ["Brawler2D.NPC::age"] = "Age",
//             ["Brawler2D.NPC::birthday_Day"] = "Birthday_Day",
//             ["Brawler2D.NPC::birthday_Season"] = "Birthday_Season",
//             ["Brawler2D.NPC::breather"] = "Breather",
//             ["Brawler2D.NPC::defaultMap"] = "DefaultMap",
//             ["Brawler2D.NPC::gender"] = "Gender",
//             ["Brawler2D.NPC::hideShadow"] = "HideShadow",
//             ["Brawler2D.NPC::isInvisible"] = "IsInvisible",
//             ["Brawler2D.NPC::isWalkingTowardPlayer"] = "IsWalkingTowardPlayer",
//             ["Brawler2D.NPC::manners"] = "Manners",
//             ["Brawler2D.NPC::optimism"] = "Optimism",
//             ["Brawler2D.NPC::socialAnxiety"] = "SocialAnxiety",
//
//             // Object
//             ["Brawler2D.Object::canBeGrabbed"] = "CanBeGrabbed",
//             ["Brawler2D.Object::canBeSetDown"] = "CanBeSetDown",
//             ["Brawler2D.Object::edibility"] = "Edibility",
//             ["Brawler2D.Object::flipped"] = "Flipped",
//             ["Brawler2D.Object::fragility"] = "Fragility",
//             ["Brawler2D.Object::hasBeenPickedUpByFarmer"] = "HasBeenPickedUpByFarmer",
//             ["Brawler2D.Object::isHoedirt"] = "IsHoeDirt",
//             ["Brawler2D.Object::isOn"] = "IsOn",
//             ["Brawler2D.Object::isRecipe"] = "IsRecipe",
//             ["Brawler2D.Object::isSpawnedObject"] = "IsSpawnedObject",
//             ["Brawler2D.Object::minutesUntilReady"] = "MinutesUntilReady",
//             ["Brawler2D.Object::netName"] = "name",
//             ["Brawler2D.Object::price"] = "Price",
//             ["Brawler2D.Object::quality"] = "Quality",
//             ["Brawler2D.Object::stack"] = "Stack",
//             ["Brawler2D.Object::tileLocation"] = "TileLocation",
//             ["Brawler2D.Object::type"] = "Type",
//
//             // Projectile
//             ["Brawler2D.Projectiles.Projectile::ignoreLocationCollision"] = "IgnoreLocationCollision",
//
//             // Tool
//             ["Brawler2D.Tool::currentParentTileIndex"] = "CurrentParentTileIndex",
//             ["Brawler2D.Tool::indexOfMenuItemView"] = "IndexOfMenuItemView",
//             ["Brawler2D.Tool::initialParentTileIndex"] = "InitialParentTileIndex",
//             ["Brawler2D.Tool::instantUse"] = "InstantUse",
//             ["Brawler2D.Tool::netName"] = "BaseName",
//             ["Brawler2D.Tool::stackable"] = "Stackable",
//             ["Brawler2D.Tool::upgradeLevel"] = "UpgradeLevel"
//         };
//
//         /// <summary>The diagnostic info for an implicit net field cast.</summary>
//         private readonly DiagnosticDescriptor AvoidImplicitNetFieldCastRule = new DiagnosticDescriptor(
//             id: "AvoidImplicitNetFieldCast",
//             title: "Netcode types shouldn't be implicitly converted",
//             messageFormat: "This implicitly converts '{0}' from {1} to {2}, but {1} has unintuitive implicit conversion rules. Consider comparing against the actual value instead to avoid bugs. See https://fmodf.io/package/avoid-implicit-net-field-cast for details.",
//             category: "FMODF.CommonErrors",
//             defaultSeverity: DiagnosticSeverity.Warning,
//             isEnabledByDefault: true,
//             helpLinkUri: "https://fmodf.io/package/avoid-implicit-net-field-cast"
//         );
//
//         /// <summary>The diagnostic info for an avoidable net field access.</summary>
//         private readonly DiagnosticDescriptor AvoidNetFieldRule = new DiagnosticDescriptor(
//             id: "AvoidNetField",
//             title: "Avoid Netcode types when possible",
//             messageFormat: "'{0}' is a {1} field; consider using the {2} property instead. See https://fmodf.io/package/avoid-net-field for details.",
//             category: "FMODF.CommonErrors",
//             defaultSeverity: DiagnosticSeverity.Warning,
//             isEnabledByDefault: true,
//             helpLinkUri: "https://fmodf.io/package/avoid-net-field"
//         );
//
//
//         /*********
//         ** Accessors
//         *********/
//         /// <summary>The descriptors for the diagnostics that this analyzer is capable of producing.</summary>
//         public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
//
//
//         /*********
//         ** Public methods
//         *********/
//         /// <summary>Construct an instance.</summary>
//         public NetFieldAnalyzer()
//         {
//             this.SupportedDiagnostics = ImmutableArray.CreateRange(new[] { this.AvoidNetFieldRule, this.AvoidImplicitNetFieldCastRule });
//         }
//
//         /// <summary>Called once at session start to register actions in the analysis context.</summary>
//         /// <param name="context">The analysis context.</param>
//         public override void Initialize(AnalysisContext context)
//         {
//             context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
//             context.EnableConcurrentExecution();
//
//             context.RegisterSyntaxNodeAction(
//                 this.AnalyzeMemberAccess,
//                 SyntaxKind.SimpleMemberAccessExpression,
//                 SyntaxKind.ConditionalAccessExpression
//             );
//             context.RegisterSyntaxNodeAction(
//                 this.AnalyzeCast,
//                 SyntaxKind.CastExpression,
//                 SyntaxKind.AsExpression
//             );
//             context.RegisterSyntaxNodeAction(
//                 this.AnalyzeBinaryComparison,
//                 SyntaxKind.EqualsExpression,
//                 SyntaxKind.NotEqualsExpression,
//                 SyntaxKind.GreaterThanExpression,
//                 SyntaxKind.GreaterThanOrEqualExpression,
//                 SyntaxKind.LessThanExpression,
//                 SyntaxKind.LessThanOrEqualExpression
//             );
//         }
//
//
//         /*********
//         ** Private methods
//         *********/
//         /// <summary>Analyze a member access syntax node and add a diagnostic message if applicable.</summary>
//         /// <param name="context">The analysis context.</param>
//         /// <returns>Returns whether any warnings were added.</returns>
//         private void AnalyzeMemberAccess(SyntaxNodeAnalysisContext context)
//         {
//             this.HandleErrors(context.Node, () =>
//             {
//                 // get member access info
//                 if (!AnalyzerUtilities.TryGetMemberInfo(context.Node, context.SemanticModel, out ITypeSymbol declaringType, out TypeInfo memberType, out string memberName))
//                     return;
//                 if (!this.IsNetType(memberType.Type))
//                     return;
//
//                 // warn: use property wrapper if available
//                 foreach (ITypeSymbol type in AnalyzerUtilities.GetConcreteTypes(declaringType))
//                 {
//                     if (this.NetFieldWrapperProperties.TryGetValue($"{type}::{memberName}", out string suggestedPropertyName))
//                     {
//                         context.ReportDiagnostic(Diagnostic.Create(this.AvoidNetFieldRule, context.Node.GetLocation(), context.Node, memberType.Type.Name, suggestedPropertyName));
//                         return;
//                     }
//                 }
//
//                 // warn: implicit conversion
//                 if (this.IsInvalidConversion(memberType.Type, memberType.ConvertedType))
//                 {
//                     context.ReportDiagnostic(Diagnostic.Create(this.AvoidImplicitNetFieldCastRule, context.Node.GetLocation(), context.Node, memberType.Type.Name, memberType.ConvertedType));
//                     return;
//                 }
//             });
//         }
//
//         /// <summary>Analyze an explicit cast or 'x as y' node and add a diagnostic message if applicable.</summary>
//         /// <param name="context">The analysis context.</param>
//         /// <returns>Returns whether any warnings were added.</returns>
//         private void AnalyzeCast(SyntaxNodeAnalysisContext context)
//         {
//             // NOTE: implicit conversion within the expression is detected by the member access
//             // checks. This method is only concerned with the conversion of its final value.
//             this.HandleErrors(context.Node, () =>
//             {
//                 if (AnalyzerUtilities.TryGetCastOrAsInfo(context.Node, context.SemanticModel, out ExpressionSyntax fromExpression, out TypeInfo fromType, out TypeInfo toType))
//                 {
//                     if (this.IsInvalidConversion(fromType.ConvertedType, toType.Type))
//                         context.ReportDiagnostic(Diagnostic.Create(this.AvoidImplicitNetFieldCastRule, context.Node.GetLocation(), fromExpression, fromType.ConvertedType.Name, toType.Type));
//                 }
//             });
//         }
//
//         /// <summary>Analyze a binary comparison syntax node and add a diagnostic message if applicable.</summary>
//         /// <param name="context">The analysis context.</param>
//         /// <returns>Returns whether any warnings were added.</returns>
//         private void AnalyzeBinaryComparison(SyntaxNodeAnalysisContext context)
//         {
//             // NOTE: implicit conversion within an operand is detected by the member access checks.
//             // This method is only concerned with the conversion of each side's final value.
//             this.HandleErrors(context.Node, () =>
//             {
//                 BinaryExpressionSyntax expression = (BinaryExpressionSyntax)context.Node;
//                 foreach (var pair in new[] { Tuple.Create(expression.Left, expression.Right), Tuple.Create(expression.Right, expression.Left) })
//                 {
//                     // get node info
//                     ExpressionSyntax curExpression = pair.Item1; // the side of the comparison being examined
//                     ExpressionSyntax otherExpression = pair.Item2; // the other side
//                     TypeInfo curType = context.SemanticModel.GetTypeInfo(curExpression);
//                     TypeInfo otherType = context.SemanticModel.GetTypeInfo(otherExpression);
//                     if (!this.IsNetType(curType.ConvertedType))
//                         continue;
//
//                     // warn for comparison to null
//                     // An expression like `building.indoors != null` will sometimes convert `building.indoors` to NetFieldBase instead of object before comparison. Haven't reproduced this in unit tests yet.
//                     Optional<object> otherValue = context.SemanticModel.GetConstantValue(otherExpression);
//                     if (otherValue.HasValue && otherValue.Value == null)
//                     {
//                         context.ReportDiagnostic(Diagnostic.Create(this.AvoidImplicitNetFieldCastRule, context.Node.GetLocation(), curExpression, curType.Type.Name, "null"));
//                         break;
//                     }
//
//                     // warn for implicit conversion
//                     if (!this.IsNetType(otherType.ConvertedType))
//                     {
//                         context.ReportDiagnostic(Diagnostic.Create(this.AvoidImplicitNetFieldCastRule, context.Node.GetLocation(), curExpression, curType.Type.Name, curType.ConvertedType));
//                         break;
//                     }
//                 }
//             });
//         }
//
//         /// <summary>Handle exceptions raised while analyzing a node.</summary>
//         /// <param name="node">The node being analyzed.</param>
//         /// <param name="action">The callback to invoke.</param>
//         private void HandleErrors(SyntaxNode node, Action action)
//         {
//             try
//             {
//                 action();
//             }
//             catch (Exception ex)
//             {
//                 throw new InvalidOperationException($"Failed processing expression: '{node}'. Exception details: {ex.ToString().Replace('\r', ' ').Replace('\n', ' ')}");
//             }
//         }
//
//         /// <summary>Get whether a net field was converted in an error-prone way.</summary>
//         /// <param name="fromType">The source type.</param>
//         /// <param name="toType">The target type.</param>
//         private bool IsInvalidConversion(ITypeSymbol fromType, ITypeSymbol toType)
//         {
//             // no conversion
//             if (!this.IsNetType(fromType) || this.IsNetType(toType))
//                 return false;
//
//             // conversion to implemented interface is OK
//             if (fromType.AllInterfaces.Contains(toType))
//                 return false;
//
//             // avoid any other conversions
//             return true;
//         }
//
//         /// <summary>Get whether a type symbol references a <c>Netcode</c> type.</summary>
//         /// <param name="typeSymbol">The type symbol.</param>
//         private bool IsNetType(ITypeSymbol typeSymbol)
//         {
//             return typeSymbol?.ContainingNamespace?.Name == NetFieldAnalyzer.NetcodeNamespace;
//         }
//     }
// }
