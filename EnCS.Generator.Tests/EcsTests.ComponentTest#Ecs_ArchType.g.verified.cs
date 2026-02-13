//HintName: Ecs_ArchType.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class Ecs
	{
		public ref struct Wall : IArchType<Wall, Runner.Position>, IArchType<Wall, Runner.Velocity>, IArchType<Wall, Runner.MeshResourceManager.TestResource>
		{
			public Runner.Position Position;
			public Runner.Velocity Velocity;
			public Runner.MeshResourceManager.TestResource TestResource;

			public Wall(Runner.Position Position, Runner.Velocity Velocity, Runner.MeshResourceManager.TestResource TestResource)
			{
				this.Position = Position;
				this.Velocity = Velocity;
				this.TestResource = TestResource;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Runner.Position IArchType<Wall, Runner.Position>.Get(ref Wall arch)
			{
				return ref arch._Position;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Runner.Velocity IArchType<Wall, Runner.Velocity>.Get(ref Wall arch)
			{
				return ref arch._Velocity;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Runner.MeshResourceManager.TestResource IArchType<Wall, Runner.MeshResourceManager.TestResource>.Get(ref Wall arch)
			{
				return ref arch._TestResource;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Wall FromArchType(ref Wall.Vectorized archType, int idx, Runner.MeshResourceManager MeshResourceManager)
			{
				return new Wall(Runner.Position.FromArray(ref Unsafe.As<Runner.Position.Vectorized, Runner.Position.Array>(ref archType._Position), idx), 
					Runner.Velocity.FromArray(ref Unsafe.As<Runner.Velocity.Vectorized, Runner.Velocity.Array>(ref archType._Velocity), idx), 
					Runner.MeshResourceManager.TestResource.FromArray(ref Unsafe.As<Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array>(ref archType._TestResource), idx, MeshResourceManager));
			}

			public ref struct Vectorized : IArchType<Wall.Vectorized, Runner.Position.Vectorized>, IArchType<Wall.Vectorized, Runner.Velocity.Vectorized>, IArchType<Wall.Vectorized, Runner.MeshResourceManager.TestResource.Vectorized>
			{
				public Runner.Position.Vectorized _Position;
				public Runner.Velocity.Vectorized _Velocity;
				public Runner.MeshResourceManager.TestResource.Vectorized _TestResource;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				static ref Runner.Position.Vectorized IArchType<Wall.Vectorized, Runner.Position.Vectorized>.Get(ref Wall.Vectorized arch)
				{
					return ref arch._Position;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				static ref Runner.Velocity.Vectorized IArchType<Wall.Vectorized, Runner.Velocity.Vectorized>.Get(ref Wall.Vectorized arch)
				{
					return ref arch._Velocity;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				static ref Runner.MeshResourceManager.TestResource.Vectorized IArchType<Wall.Vectorized, Runner.MeshResourceManager.TestResource.Vectorized>.Get(ref Wall.Vectorized arch)
				{
					return ref arch._TestResource;
				}
			}

			public struct Memory : IArchMemory<Memory, Wall.Vectorized, Wall>
			{
				public Runner.Position.Memory Position;
				public Runner.Velocity.Memory Velocity;
				public Runner.MeshResourceManager.TestResource.Memory TestResource;

				public Memory(int length)
				{
					this.Position = new Runner.Position(length);
					this.Velocity = new Runner.Velocity(length);
					this.TestResource = new Runner.MeshResourceManager.TestResource(length);
				}

				Wall.Vectorized IArchMemory<Memory, Wall.Vectorized, Wall>.GetVec(nint idx)
				{
					return new Wall.Vectorized
					{
						_Position = this.Position.GetVec(idx),
						_Velocity = this.Velocity.GetVec(idx),
						_TestResource = this.TestResource.GetVec(idx),
					};
				}

				Wall IArchMemory<Memory, Wall.Vectorized, Wall>.GetSingle(nint idx)
				{
					return new Wall
					(
						Position = this.Position.GetSingle(idx),
						Velocity = this.Velocity.GetSingle(idx),
						TestResource = this.TestResource.GetSingle(idx),
					);
				}

				UtilLib.Memory.FixedRefBuffer8<Wall> IArchMemory<Memory, Wall.Vectorized, Wall>.GetSingleArray(nint idx)
				{
					UtilLib.Memory.FixedRefBuffer8<Wall> buffer = new();
					int start = idx / 8;

					buffer[0] = GetSingle(start + 0);
					buffer[1] = GetSingle(start + 1);
					buffer[2] = GetSingle(start + 2);
					buffer[3] = GetSingle(start + 3);
					buffer[4] = GetSingle(start + 4);
					buffer[5] = GetSingle(start + 5);
					buffer[6] = GetSingle(start + 6);
					buffer[7] = GetSingle(start + 7);

					return buffer;
				}

				public Wall.Span AsSpan()
				{
					return new Wall.Span(in this);
				}

				static Memory IArchMemory<Memory, Wall.Vectorized, Wall>.Create(int length)
				{
					return new Memory(length);
				}
			}

			public ref struct Span
			{
				public Runner.Position.Span Position;
				public Runner.Velocity.Span Velocity;
				public Runner.MeshResourceManager.TestResource.Span TestResource;

				public Span(ref readonly Wall.Memory memory)
				{
					this.Position = new(memory.Position);
					this.Velocity = new(memory.Velocity);
					this.TestResource = new(memory.TestResource);
				}
			}
		}

		public ref struct Cam : IArchType<Cam, Runner.TestComp123>
		{
			public Runner.TestComp123 TestComp123;

			public Cam(Runner.TestComp123 TestComp123)
			{
				this.TestComp123 = TestComp123;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Runner.TestComp123 IArchType<Cam, Runner.TestComp123>.Get(ref Cam arch)
			{
				return ref arch._TestComp123;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Cam FromArchType(ref Cam.Vectorized archType, int idx)
			{
				return new Cam(Runner.TestComp123.FromArray(ref Unsafe.As<Runner.TestComp123.Vectorized, Runner.TestComp123.Array>(ref archType._TestComp123), idx));
			}

			public ref struct Vectorized : IArchType<Cam.Vectorized, Runner.TestComp123.Vectorized>
			{
				public Runner.TestComp123.Vectorized _TestComp123;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				static ref Runner.TestComp123.Vectorized IArchType<Cam.Vectorized, Runner.TestComp123.Vectorized>.Get(ref Cam.Vectorized arch)
				{
					return ref arch._TestComp123;
				}
			}

			public struct Memory : IArchMemory<Memory, Cam.Vectorized, Cam>
			{
				public Runner.TestComp123.Memory TestComp123;

				public Memory(int length)
				{
					this.TestComp123 = new Runner.TestComp123(length);
				}

				Cam.Vectorized IArchMemory<Memory, Cam.Vectorized, Cam>.GetVec(nint idx)
				{
					return new Cam.Vectorized
					{
						_TestComp123 = this.TestComp123.GetVec(idx),
					};
				}

				Cam IArchMemory<Memory, Cam.Vectorized, Cam>.GetSingle(nint idx)
				{
					return new Cam
					(
						TestComp123 = this.TestComp123.GetSingle(idx),
					);
				}

				UtilLib.Memory.FixedRefBuffer8<Cam> IArchMemory<Memory, Cam.Vectorized, Cam>.GetSingleArray(nint idx)
				{
					UtilLib.Memory.FixedRefBuffer8<Cam> buffer = new();
					int start = idx / 8;

					buffer[0] = GetSingle(start + 0);
					buffer[1] = GetSingle(start + 1);
					buffer[2] = GetSingle(start + 2);
					buffer[3] = GetSingle(start + 3);
					buffer[4] = GetSingle(start + 4);
					buffer[5] = GetSingle(start + 5);
					buffer[6] = GetSingle(start + 6);
					buffer[7] = GetSingle(start + 7);

					return buffer;
				}

				public Cam.Span AsSpan()
				{
					return new Cam.Span(in this);
				}

				static Memory IArchMemory<Memory, Cam.Vectorized, Cam>.Create(int length)
				{
					return new Memory(length);
				}
			}

			public ref struct Span
			{
				public Runner.TestComp123.Span TestComp123;

				public Span(ref readonly Cam.Memory memory)
				{
					this.TestComp123 = new(memory.TestComp123);
				}
			}
		}
	}

	/*
	public static class Ecs_ContainerExtensions
	{
		// TODO: Generate create method

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ecs.Wall Get(this ref IndexedContainer<Ecs.Wall.Vectorized, Ecs.Wall> container, ArchRef<Ecs.Wall> ptr, Runner.MeshResourceManager MeshResourceManager)
		{
			return Ecs.Wall.FromArchType(ref container.GetValue(ptr);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ecs.Cam Get(this ref IndexedContainer<Ecs.Cam.Vectorized, Ecs.Cam> container, ArchRef<Ecs.Cam> ptr)
		{
			return Ecs.Cam.FromArchType(ref container.GetValue(ptr);
		}
	}
	*/
}