//HintName: Ecs_World.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class Ecs
	{
		public ref struct MainWorld
		{
			ref ArchTypeContainer<Wall> containerWall;
			ref ArchTypeContainer<Tile> containerTile;

			public MainWorld(ref ArchTypeContainer<Wall> containerWall, ref ArchTypeContainer<Tile> containerTile)
			{
				this.containerWall = ref containerWall;
				this.containerTile = ref containerTile;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ArchRef<Wall> Create(in Wall data)
			{
				return containerWall.Create(data);
			}
			
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Wall.Ref Get(in ArchRef<Wall> ptr)
			{
				return containerWall.Get(ptr);
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
				var enumWall = new ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.Enumerator<Wall>(containerWall.AsSpan(), (int)containerWall.Entities);
				var enumTile = new ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.Enumerator<Tile>(containerTile.AsSpan(), (int)containerTile.Entities);
				
				system.Update(ref enumWall);
				system.Update(ref enumTile);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(PrintSystem system)
			{
				var enumWall = new ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.Enumerator<Wall>(containerWall.AsSpan(), (int)containerWall.Entities);
				var enumTile = new ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.Enumerator<Tile>(containerTile.AsSpan(), (int)containerTile.Entities);
				
				system.Update(ref enumWall);
				system.Update(ref enumTile);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(PerfSystem system)
			{
				var enumWall = new ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.Enumerator<Wall>(containerWall.AsSpan(), (int)containerWall.Entities);
				var enumTile = new ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.Enumerator<Tile>(containerTile.AsSpan(), (int)containerTile.Entities);
				
				system.Update(ref enumWall);
				system.Update(ref enumTile);
			}
		}
	}
}