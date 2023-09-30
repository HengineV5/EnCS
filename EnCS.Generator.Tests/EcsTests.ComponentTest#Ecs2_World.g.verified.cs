//HintName: Ecs2_World.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Test
{
	public partial class Ecs2
	{
		public ref struct Main
		{
			ref ArchTypeContainer<Tile> containerTile;

			public Main(ref ArchTypeContainer<Tile> containerTile)
			{
				this.containerTile = ref containerTile;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ArchRef<Tile> Create(in Tile data)
			{
				return containerTile.Create(data);
			}
			
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Tile.Ref Get(in ArchRef<Tile> ptr)
			{
				return containerTile.Get(ptr);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(PositionSystem system)
			{
				var enumTile = new ComponentEnumerableNew<Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>.Enumerator<Tile>(containerTile.AsSpan(), (int)containerTile.Entities);
				
				system.Update(ref enumTile);
			}
		}
	}
}