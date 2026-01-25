using EnCS;
using EnCS.Attributes;
using Engine.Components;
using Engine.Components.Graphics;
using Silk.NET.GLFW;
using Silk.NET.Windowing;
using Silk.NET.Vulkan;
using Silk.NET.OpenGL;
using System.Runtime.InteropServices;
using Engine.Graphics;
using Engine.Parsing;
using Engine.Translation;
using Microsoft.Extensions.Logging;

namespace Engine.Components
{
	[Component]
	public ref partial struct Scale
	{
		public ref float x;
		public ref float y;
		public ref float z;

		public static implicit operator Vector3f(Scale v) => new(v.x, v.y, v.z);
		public static implicit operator Vector2f(Scale v) => new(v.x, v.y);
	}

	static partial class Comp_Extensions
	{
		public static void Set(this ref Scale position, Vector3f value)
		{
			position.x = value.x;
			position.y = value.y;
			position.z = value.z;
		}
		public static void Set(this ref Scale position, Vector2f value)
		{
			position.x = value.x;
			position.y = value.y;
			position.z = 0;
		}
	}
}


namespace Engine.Components
{
	[Component]
	public ref partial struct Rotation
	{
		public ref float x;
		public ref float y;
		public ref float z;
		public ref float w;

		public static implicit operator Quaternionf(Rotation v) => new(v.x, v.y, v.z, v.w);
		public static implicit operator Vector4f(Rotation v) => new(v.x, v.y, v.z, v.w);
		public static implicit operator Vector3f(Rotation v) => new(v.x, v.y, v.z);
		public static implicit operator Vector2f(Rotation v) => new(v.x, v.y);
	}

	static partial class Comp_Extensions
	{
		public static void Set(this ref Rotation position, Quaternionf value)
		{
			position.x = value.x;
			position.y = value.y;
			position.z = value.z;
			position.w = value.w;
		}

		public static void Set(this ref Rotation position, Vector4f value)
		{
			position.x = value.x;
			position.y = value.y;
			position.z = value.z;
			position.w = value.w;
		}

		public static void Set(this ref Rotation position, Vector3f value)
		{
			position.x = value.x;
			position.y = value.y;
			position.z = value.z;
			position.w = 0;
		}

		public static void Set(this ref Rotation position, Vector2f value)
		{
			position.x = value.x;
			position.y = value.y;
			position.z = 0;
			position.w = 0;
		}
	}
}


namespace Engine.Components
{
	[Component]
	public ref partial struct Position
	{
		public ref float x;
		public ref float y;
		public ref float z;

		public static implicit operator Vector3f(Position v) => new(v.x, v.y, v.z);
		public static implicit operator Vector2f(Position v) => new(v.x, v.y);
	}

	static partial class Comp_Extensions
	{
		public static void Set(this ref Position position, Vector3f value)
		{
			position.x = value.x;
			position.y = value.y;
			position.z = value.z;
		}
		public static void Set(this ref Position position, Vector2f value)
		{
			position.x = value.x;
			position.y = value.y;
			position.z = 0;
		}
	}
}


namespace Engine
{
	public class EngineConfig
	{
		public string appName;
		public Version appVersion;

		public string engineName;
		public Version engineVersion;

		public int idx;
	}

	public struct EngineContext
	{
		public float dt;
	}

	[System]
	public partial class PositionSystem
	{
		[SystemUpdate]
		public void Update(ref Position position)
		{
		}

		[SystemUpdate]
		public void Update(ref Position.Vectorized position)
		{
		}
	}

	public partial class HengineEcs
	{

	}

	public partial class Hengine
	{
		bool ShouldExit()
		{
			return argIWindow.IsClosing;
		}
		
		public static void Ecs()
		{
			new EcsBuilder()
				.ArchType(x =>
				{
					//x.ArchType<Position, Rotation, Scale, Mesh, ETexture>("Entity");
					//x.ArchType<Position, Rotation, Scale, Mesh, PbrMaterial, Networked>("NEntity");
					//x.ArchType<Position, Rotation, Scale, HexCell, Mesh, PbrMaterial, Networked>("Hex");
					//x.ArchType<Position, Rotation, Camera, Skybox, Networked>("Cam");
					//
					//x.ArchType<Position, Rotation, Scale, GizmoComp>("Gizmo");
					//x.ArchType<GizmoLine>("GizmoLine1");
					//
					//x.ArchType<GuiProperties, GuiPosition, GuiSize, GuiState, GuiButton, GuiDraggable, TextureAtlas>("GuiButton1");
					//x.ArchType<GuiProperties, GuiPosition, TextureAtlas, GuiText>("TextElement");
				})
				.System(x =>
				{
					//x.System<PositionSystem>();
					//x.System<RotateSystem>();
					//x.System<MoveSystem>();
					//
					//x.System<GuiButtonSystem>();
					//x.System<GuiDraggableSystem>();
					//
					//x.System<OpenGLRenderSystem>();
					//
					//x.System<VulkanCameraSystem>();
					//x.System<VulkanPbrRenderSystem>();
					//x.System<VulkanWireframeRenderSystem>();
					//x.System<VulkanPresentSystem>();
					//x.System<VulkanGuiRenderSystem>();
					//x.System<VulkanTextRenderingSystem>();
					//x.System<VulkanGizmoRenderSystem>();
					//x.System<VulkanGizmoLineRenderSystem>();
					//
					//x.System<ClientSendSystem>();
					//x.System<ClientReceiveSystem>();
				})
				.World(x =>
				{
					//x.World<HengineEcs.NEntity, HengineEcs.Cam, HengineEcs.Hex, HengineEcs.Gizmo, HengineEcs.GizmoLine1>("Main");
					//x.World<HengineEcs.GuiButton1, HengineEcs.TextElement>("Overlay");
				})
				.Resource(x =>
				{
					//x.ResourceManager<VulkanMeshResourceManager>();
					//x.ResourceManager<VulkanTextureResourceManager>();
					//x.ResourceManager<VulkanTextResourceManager>();
					//x.ResourceManager<VulkanMaterialResourceManager>();
					//x.ResourceManager<VulkanSkyboxResourceManager>();
					//x.ResourceManager<VulkanTextureAtlasResourceManager>();

					//x.ResourceManager<OpenGLMeshResourceManager>();
					//x.ResourceManager<OpenGLTextureResourceManager>();
				})
				.Build<HengineEcs>();
		}

	public partial class HengineServerEcs
	{

	}
}
