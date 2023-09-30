//HintName: Ecs_ArchType.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class Ecs
	{
		public struct Wall : IArchType<Wall, Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>, IArchType<Wall, Runner.Velocity, Runner.Velocity.Vectorized, Runner.Velocity.Array>
		{
			public Runner.Position.Vectorized Position;
			public Runner.Velocity.Vectorized Velocity;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Runner.Position.Array IArchType<Wall, Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.GetSingle(ref Wall arch)
			{
				return ref Unsafe.As<Runner.Position.Vectorized, Runner.Position.Array>(ref arch.Position);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Runner.Position.Vectorized IArchType<Wall, Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.GetVec(ref Wall arch)
			{
				return ref arch.Position;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Runner.Velocity.Array IArchType<Wall, Runner.Velocity, Runner.Velocity.Vectorized, Runner.Velocity.Array>.GetSingle(ref Wall arch)
			{
				return ref Unsafe.As<Runner.Velocity.Vectorized, Runner.Velocity.Array>(ref arch.Velocity);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Runner.Velocity.Vectorized IArchType<Wall, Runner.Velocity, Runner.Velocity.Vectorized, Runner.Velocity.Array>.GetVec(ref Wall arch)
			{
				return ref arch.Velocity;
			}

			public ref struct Ref
			{
				public Runner.Position.Ref Position;
				public Runner.Velocity.Ref Velocity;

				public Ref(Runner.Position.Ref Position, Runner.Velocity.Ref Velocity)
				{
					this.Position = Position;
					this.Velocity = Velocity;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static Ref FromArchType(ref Wall archType, int idx)
				{
					return new Ref(
						Runner.Position.Ref.FromArray(ref Unsafe.As<Runner.Position.Vectorized, Runner.Position.Array>(ref archType.Position), idx), 
						Runner.Velocity.Ref.FromArray(ref Unsafe.As<Runner.Velocity.Vectorized, Runner.Velocity.Array>(ref archType.Velocity), idx)
					);
				}
			}
		}

		public struct Tile : IArchType<Tile, Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>
		{
			public Runner.Position.Vectorized Position;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Runner.Position.Array IArchType<Tile, Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.GetSingle(ref Tile arch)
			{
				return ref Unsafe.As<Runner.Position.Vectorized, Runner.Position.Array>(ref arch.Position);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Runner.Position.Vectorized IArchType<Tile, Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.GetVec(ref Tile arch)
			{
				return ref arch.Position;
			}

			public ref struct Ref
			{
				public Runner.Position.Ref Position;

				public Ref(Runner.Position.Ref Position)
				{
					this.Position = Position;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static Ref FromArchType(ref Tile archType, int idx)
				{
					return new Ref(
						Runner.Position.Ref.FromArray(ref Unsafe.As<Runner.Position.Vectorized, Runner.Position.Array>(ref archType.Position), idx)
					);
				}
			}
		}
	}

	public static class Ecs_ContainerExtensions
	{
		// TODO: Generate create method

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ecs.Wall.Ref Get(this ref ArchTypeContainer<Ecs.Wall> container, ArchRef<Ecs.Wall> ptr)
		{
			return Ecs.Wall.Ref.FromArchType(ref container.GetVec(ptr), (int)ptr.idx & 7);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ecs.Tile.Ref Get(this ref ArchTypeContainer<Ecs.Tile> container, ArchRef<Ecs.Tile> ptr)
		{
			return Ecs.Tile.Ref.FromArchType(ref container.GetVec(ptr), (int)ptr.idx & 7);
		}
	}
}