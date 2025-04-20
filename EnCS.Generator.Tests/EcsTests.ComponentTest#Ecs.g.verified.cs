//HintName: Ecs.g.cs
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class Ecs
	{
		ArchTypeContainer<Wall.Vectorized, Wall> containerWall;
		ArchTypeContainer<Tile.Vectorized, Tile> containerTile;
		ArchTypeContainer<Cam.Vectorized, Cam> containerCam;
		
		Runner.MeshResourceManager MeshResourceManager;

		public Ecs(Runner.MeshResourceManager MeshResourceManager)
		{
			containerWall = new ArchTypeContainer<Wall.Vectorized, Wall>();
			containerTile = new ArchTypeContainer<Tile.Vectorized, Tile>();
			containerCam = new ArchTypeContainer<Cam.Vectorized, Cam>();

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
		[InterceptsLocation(@"", 197, 5)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public static Ecs InterceptBuild(this EcsBuilder builder)
		{
			return new Ecs();
		}
	}
	*/
}