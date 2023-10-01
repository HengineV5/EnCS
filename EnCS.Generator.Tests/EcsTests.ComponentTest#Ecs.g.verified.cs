//HintName: Ecs.g.cs
using System.Runtime.CompilerServices;
using EnCS;

namespace Test
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
		public Main GetMain()
		{
			return new Main(ref containerWall, ref containerTile);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public World2 GetWorld2()
		{
			return new World2(ref containerWall);
		}
	}

	static class Ecs_Intercept
	{
		[InterceptsLocation(@"", 85, 5)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public static Ecs InterceptBuild(this EcsBuilder builder)
		{
			return new Ecs();
		}
	}
}