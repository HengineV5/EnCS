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
				return ref arch.Position;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Runner.Velocity IArchType<Wall, Runner.Velocity>.Get(ref Wall arch)
			{
				return ref arch.Velocity;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Runner.MeshResourceManager.TestResource IArchType<Wall, Runner.MeshResourceManager.TestResource>.Get(ref Wall arch)
			{
				return ref arch.TestResource;
			}

			public Wall SetResourceManager(Runner.MeshResourceManager MeshResourceManager)
			{
				this.TestResource.SetResourceManager(MeshResourceManager);
				return this;
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
					this.Position = new Runner.Position.Memory(length);
					this.Velocity = new Runner.Velocity.Memory(length);
					this.TestResource = new Runner.MeshResourceManager.TestResource.Memory(length);
				}

				Wall.Vectorized IArchMemory<Memory, Wall.Vectorized, Wall>.GetVec(nint idx)
				{
					return new Wall.Vectorized
					{
						_Position = this.Position.GetVec((int)idx),
						_Velocity = this.Velocity.GetVec((int)idx),
						_TestResource = this.TestResource.GetVec((int)idx),
					};
				}

				Wall IArchMemory<Memory, Wall.Vectorized, Wall>.GetSingle(nint idx)
				{
					return new Wall
					{
						Position = this.Position.GetSingle((int)idx),
						Velocity = this.Velocity.GetSingle((int)idx),
						TestResource = this.TestResource.GetSingle((int)idx),
					};
				}

				static Memory IArchMemory<Memory, Wall.Vectorized, Wall>.Create(int length)
				{
					return new Memory(length);
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
				return ref arch.TestComp123;
			}

			public Cam SetResourceManager()
			{
				
				return this;
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
					this.TestComp123 = new Runner.TestComp123.Memory(length);
				}

				Cam.Vectorized IArchMemory<Memory, Cam.Vectorized, Cam>.GetVec(nint idx)
				{
					return new Cam.Vectorized
					{
						_TestComp123 = this.TestComp123.GetVec((int)idx),
					};
				}

				Cam IArchMemory<Memory, Cam.Vectorized, Cam>.GetSingle(nint idx)
				{
					return new Cam
					{
						TestComp123 = this.TestComp123.GetSingle((int)idx),
					};
				}

				static Memory IArchMemory<Memory, Cam.Vectorized, Cam>.Create(int length)
				{
					return new Memory(length);
				}
			}
		}
	}
}