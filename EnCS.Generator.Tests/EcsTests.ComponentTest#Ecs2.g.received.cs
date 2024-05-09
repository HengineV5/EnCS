//HintName: Ecs2.g.cs
using System.Runtime.CompilerServices;
using EnCS;

namespace Test
{
	public partial class Ecs2
	{
		ArchTypeContainer<Tile> containerTile;
		
		

		public Ecs2()
		{
			containerTile = new ArchTypeContainer<Tile>();

			
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Main GetMain()
		{
			return new Main(ref containerTile);
		}
	}

	/*
	static class Ecs2_Intercept
	{
		[InterceptsLocation(@"", 272, 4)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public static Ecs2 InterceptBuild(this EcsBuilder builder)
		{
			return new Ecs2();
		}
	}
	*/
}