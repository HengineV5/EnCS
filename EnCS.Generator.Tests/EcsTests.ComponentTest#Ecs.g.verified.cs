//HintName: Ecs.g.cs
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class Ecs
	{
		IndexedContainer<Wall.Vectorized, Wall> containerWall;
		IndexedContainer<Tile.Vectorized, Tile> containerTile;
		IndexedContainer<Cam.Vectorized, Cam> containerCam;
		
		Runner.MeshResourceManager MeshResourceManager;

		public Ecs(Runner.MeshResourceManager MeshResourceManager)
		{
			containerWall = new IndexedContainer<Wall.Vectorized, Wall>();
			containerTile = new IndexedContainer<Tile.Vectorized, Tile>();
			containerCam = new IndexedContainer<Cam.Vectorized, Cam>();

			this.MeshResourceManager = MeshResourceManager;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public MainWorld GetMainWorld()
		{
			return new MainWorld(ref containerWall, ref containerTile, ref containerCam, MeshResourceManager);
		}
	}

	/*
	static class Ecs_Intercept
	{
		[InterceptsLocation(@"", 258, 5)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public static Ecs InterceptBuild(this EcsBuilder builder)
		{
			return new Ecs();
		}
	}
	*/
}