// <copyright file="GlobalSuppressions.cs" company="Kaden Metzger Id: 11817362">
// Copyright (c) Kaden Metzger Id: 11817362. All rights reserved.
// </copyright>
// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<Operator Node class will have many types of inheriting classes. Separating them into different files is over kill>", Scope = "type", Target = "~T:SpreadsheetEngine.AdditionOperatorNode")]
[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<Operator Node class will have many types of inheriting classes. Separating them into different files is over kill>", Scope = "type", Target = "~T:SpreadsheetEngine.SubtractionOperatorNode")]
[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<Operator Node class will have many types of inheriting classes. Separating them into different files is over kill>", Scope = "type", Target = "~T:SpreadsheetEngine.MultiplicationOperatorNode")]
[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<Operator Node class will have many types of inheriting classes. Separating them into different files is over kill>", Scope = "type", Target = "~T:SpreadsheetEngine.DivisionOperatorNode")]
