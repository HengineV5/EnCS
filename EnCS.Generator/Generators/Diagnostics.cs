using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnCS.Generator
{
	static class ArchTypeGeneratorDiagnostics
	{
		public static readonly DiagnosticDescriptor ArchTypeMustBeValidComponent = new("ECS003", "Archtype can only contain valid components", "Archtype member is not valid component", "ArchTypeGenerator", DiagnosticSeverity.Error, true);
	}

	static class ComponentGeneratorDiagnostics
	{
		public static readonly DiagnosticDescriptor InvalidComponentMemberType = new("ECS001", "Invalid component member type", "Component member of type '{0}' is not supported", "ComponentGenerator", DiagnosticSeverity.Error, true);

		public static readonly DiagnosticDescriptor ComponentMustBePartial = new("ECS002", "Component struct must be partial", "Component struct is not partial", "ComponentGenerator", DiagnosticSeverity.Error, true);
	}

	static class EcsGeneratorDiagnostics
	{
	}

	static class ResourceManagerGeneratorDiagnostics
	{
		public static readonly DiagnosticDescriptor InvalidResourceManagerTypeArgs = new("ECS012", "Invalid resource manager", "Resource manager may only have one or two type arguments", "ResourceManagerGenerator", DiagnosticSeverity.Error, true);
	}

	static class SystemGeneratorDiagnostics
	{
		public static readonly DiagnosticDescriptor SystemUpdateMethodsMustBeEqual = new("ECS004", "All system update methods within the group must be equal", "", "SystemGenerator", DiagnosticSeverity.Error, true);

		public static readonly DiagnosticDescriptor MethodArgumentsMustBeConcistent = new("ECS005", "All system update methods must only use Vector or Single types", "", "SystemGenerator", DiagnosticSeverity.Error, true);

		public static readonly DiagnosticDescriptor MethodArgumentMustBeComponentOrResourceOrContext = new("ECS007", "All system method arguments must be a valid component, resource or context parameter", "", "SystemGenerator", DiagnosticSeverity.Error, true);

		public static readonly DiagnosticDescriptor MethodCannotBeEmpty = new("ECS008", "System update method cannot be empty", "", "SystemGenerator", DiagnosticSeverity.Warning, true);

		public static readonly DiagnosticDescriptor MethodCannotBeInMoreThanOneGroup = new("ECS009", "System update method cannot be in more than one group", "", "SystemGenerator", DiagnosticSeverity.Error, true);

		public static readonly DiagnosticDescriptor MethodsWithinGroupMustHaveIdenticalChunk = new("ECS010", "Methods within a group must have identical chunk sizes", "", "SystemGenerator", DiagnosticSeverity.Error, true);

		public static readonly DiagnosticDescriptor PreOrPostLoopCanOnlyHaveContextArgs = new("ECS011", "Pre and post loop methods can only have system contexts as arguments", "", "SystemGenerator", DiagnosticSeverity.Error, true);
	}

	static class WorldGeneratorDiagnostics
	{
		public static readonly DiagnosticDescriptor TypeMustBeValidArchType = new("ECS006", "Type must be a valid arch type", "", "SystemGenerator", DiagnosticSeverity.Error, true);

		public static readonly DiagnosticDescriptor UnableToFindType = new("ECS013", "Unable to find type", "", "SystemGenerator", DiagnosticSeverity.Error, true);
	}
}
