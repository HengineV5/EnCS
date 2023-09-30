//HintName: Ecs.g.cs
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class Ecs
	{
		ArchTypeContainer<Wall> containerWall;
		ArchTypeContainer<Tile> containerTile;

		public Ecs()
		{
			containerWall = new ArchTypeContainer<Wall>();
			containerTile = new ArchTypeContainer<Tile>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public MainWorld GetMainWorld()
		{
			return new MainWorld(ref containerWall, ref containerTile);
		}
	}

	static class Ecs_Intercept
	{
		[InterceptsLocation(@"", 121, 6)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public static Ecs InterceptBuild(this EcsBuilder builder)
		{
			return new Ecs();
		}
	}
}