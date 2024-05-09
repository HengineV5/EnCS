//HintName: Ecs.g.cs
using System.Runtime.CompilerServices;
using EnCS;

namespace Test
{
	public partial class Ecs
	{
		ArchTypeContainer<Wall> containerWall;
		ArchTypeContainer<Tile> containerTile;
		
		Project.Primitives.MeshResourceManager MeshResourceManager;
		Project.Primitives.TestResourceManager TestResourceManager;

		public Ecs(Project.Primitives.MeshResourceManager MeshResourceManager, Project.Primitives.TestResourceManager TestResourceManager)
		{
			containerWall = new ArchTypeContainer<Wall>();
			containerTile = new ArchTypeContainer<Tile>();

			this.MeshResourceManager = MeshResourceManager;
			this.TestResourceManager = TestResourceManager;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Main GetMain()
		{
			return new Main(ref containerWall, ref containerTile, MeshResourceManager, TestResourceManager);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public World2 GetWorld2()
		{
			return new World2(ref containerWall, MeshResourceManager, TestResourceManager);
		}
	}

	/*
	static class Ecs_Intercept
	{
		[InterceptsLocation(@"", 254, 4)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public static Ecs InterceptBuild(this EcsBuilder builder)
		{
			return new Ecs();
		}
	}
	*/
}