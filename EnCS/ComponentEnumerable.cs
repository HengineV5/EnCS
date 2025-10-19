using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace EnCS
{
    public ref struct ComponentEnumerable<T1Comp, T1Vec, T1Single>
        where T1Comp : IComponent<T1Comp, T1Vec, T1Single>, allows ref struct
        where T1Vec : unmanaged
        where T1Single : unmanaged
    {
        public ref struct Enumerator<T1Arch>
            where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>
        {
            public ArchTypeSlice<T1Vec, T1Single> Current
            {
                get
                {
                    return slice;
                }
            }

            public int Remaining
            {
                get
                {
                    return Math.Min(8, remaining);
                }
            }

            ArchTypeSlice<T1Vec, T1Single> slice;

            Span<T1Arch>.Enumerator e1;
            Span<T1Arch> span1;
            int count;
            int remaining;

            public Enumerator(Span<T1Arch> span1, int count)
            {
                this.span1 = span1;
                this.count = count;

                Reset();
            }

            public bool MoveNext()
            {
                if (e1.MoveNext())
                {
                    slice = new(ref T1Comp.GetVec(ref e1.Current), ref T1Comp.GetSingle(ref e1.Current));
                    remaining -= 8;
                }
                else
                {
                    return false;
                }

                return true;
            }

            public void Reset()
            {
                e1 = span1.GetEnumerator();
                remaining = count + 8;
            }
        }

        public Enumerator<T1Arch> GetEnumerator<T1Arch>(Span<T1Arch> span1, int count)
            where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>
        {
            return new(span1, count);
        }
    }

    public ref struct ComponentEnumerable<T1Comp, T1Vec, T1Single, T2Comp, T2Vec, T2Single>
        where T1Comp : IComponent<T1Comp, T1Vec, T1Single>, allows ref struct
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Comp : IComponent<T2Comp, T2Vec, T2Single>, allows ref struct
		where T2Vec : unmanaged
		where T2Single : unmanaged
	{
		public ref struct Enumerator<T1Arch>
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>
		{
			public ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single> Current
			{
				get
				{
					return slice;
				}
			}

			public int Remaining
			{
				get
				{
					return Math.Min(8, remaining);
				}
			}

			ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single> slice;

			Span<T1Arch>.Enumerator e1;
			Span<T1Arch> span1;
			int count;
			int remaining;

			public Enumerator(Span<T1Arch> span1, int count)
			{
				this.span1 = span1;
				this.count = count;

				Reset();
			}

			public bool MoveNext()
			{
				if (e1.MoveNext())
				{
					slice = new(ref T1Comp.GetVec(ref e1.Current), ref T1Comp.GetSingle(ref e1.Current), ref T2Comp.GetVec(ref e1.Current), ref T2Comp.GetSingle(ref e1.Current));
					remaining -= 8;
				}
				else
				{
					return false;
				}

				return true;
			}

			public void Reset()
			{
				e1 = span1.GetEnumerator();
				remaining = count + 8;
			}
		}

		public Enumerator<T1Arch> GetEnumerator<T1Arch>(Span<T1Arch> span1, int count)
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>
		{
			return new(span1, count);
		}
	}

	public ref struct ComponentEnumerable<T1Comp, T1Vec, T1Single, T2Comp, T2Vec, T2Single, T3Comp, T3Vec, T3Single>
		where T1Comp : IComponent<T1Comp, T1Vec, T1Single>, allows ref struct
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Comp : IComponent<T2Comp, T2Vec, T2Single>, allows ref struct
		where T2Vec : unmanaged
		where T2Single : unmanaged
		where T3Comp : IComponent<T3Comp, T3Vec, T3Single>, allows ref struct
		where T3Vec : unmanaged
		where T3Single : unmanaged
	{
		public ref struct Enumerator<T1Arch>
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>, IArchType<T1Arch, T3Comp, T3Vec, T3Single>
		{
			public ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single> Current
			{
				get
				{
					return slice;
				}
			}

			public int Remaining
			{
				get
				{
					return Math.Min(8, remaining);
				}
			}

			ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single> slice;

			Span<T1Arch>.Enumerator e1;
			Span<T1Arch> span1;
			int count;
			int remaining;

			public Enumerator(Span<T1Arch> span1, int count)
			{
				this.span1 = span1;
				this.count = count;

				Reset();
			}

			public bool MoveNext()
			{
				if (e1.MoveNext())
				{
					slice = new(ref T1Comp.GetVec(ref e1.Current), ref T1Comp.GetSingle(ref e1.Current), ref T2Comp.GetVec(ref e1.Current), ref T2Comp.GetSingle(ref e1.Current), ref T3Comp.GetVec(ref e1.Current), ref T3Comp.GetSingle(ref e1.Current));
					remaining -= 8;
				}
				else
				{
					return false;
				}

				return true;
			}

			public void Reset()
			{
				e1 = span1.GetEnumerator();
				remaining = count + 8;
			}
		}

		public Enumerator<T1Arch> GetEnumerator<T1Arch>(Span<T1Arch> span1, int count)
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>, IArchType<T1Arch, T3Comp, T3Vec, T3Single>
		{
			return new(span1, count);
		}
	}

	public ref struct ComponentEnumerable<T1Comp, T1Vec, T1Single, T2Comp, T2Vec, T2Single, T3Comp, T3Vec, T3Single, T4Comp, T4Vec, T4Single>
		where T1Comp : IComponent<T1Comp, T1Vec, T1Single>, allows ref struct
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Comp : IComponent<T2Comp, T2Vec, T2Single>, allows ref struct
		where T2Vec : unmanaged
		where T2Single : unmanaged
		where T3Comp : IComponent<T3Comp, T3Vec, T3Single>, allows ref struct
		where T3Vec : unmanaged
		where T3Single : unmanaged
		where T4Comp : IComponent<T4Comp, T4Vec, T4Single>, allows ref struct
		where T4Vec : unmanaged
		where T4Single : unmanaged
	{
		public ref struct Enumerator<T1Arch>
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>, IArchType<T1Arch, T3Comp, T3Vec, T3Single>, IArchType<T1Arch, T4Comp, T4Vec, T4Single>
		{
			public ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single> Current
			{
				get
				{
					return slice;
				}
			}

			public int Remaining
			{
				get
				{
					return Math.Min(8, remaining);
				}
			}

			ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single> slice;

			Span<T1Arch>.Enumerator e1;
			Span<T1Arch> span1;
			int count;
			int remaining;

			public Enumerator(Span<T1Arch> span1, int count)
			{
				this.span1 = span1;
				this.count = count;

				Reset();
			}

			public bool MoveNext()
			{
				if (e1.MoveNext())
				{
					slice = new(ref T1Comp.GetVec(ref e1.Current), ref T1Comp.GetSingle(ref e1.Current), ref T2Comp.GetVec(ref e1.Current), ref T2Comp.GetSingle(ref e1.Current), ref T3Comp.GetVec(ref e1.Current), ref T3Comp.GetSingle(ref e1.Current), ref T4Comp.GetVec(ref e1.Current), ref T4Comp.GetSingle(ref e1.Current));
					remaining -= 8;
				}
				else
				{
					return false;
				}

				return true;
			}

			public void Reset()
			{
				e1 = span1.GetEnumerator();
				remaining = count + 8;
			}
		}

		public Enumerator<T1Arch> GetEnumerator<T1Arch>(Span<T1Arch> span1, int count)
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>, IArchType<T1Arch, T3Comp, T3Vec, T3Single>, IArchType<T1Arch, T4Comp, T4Vec, T4Single>
		{
			return new(span1, count);
		}
	}

	public ref struct ComponentEnumerable<T1Comp, T1Vec, T1Single, T2Comp, T2Vec, T2Single, T3Comp, T3Vec, T3Single, T4Comp, T4Vec, T4Single, T5Comp, T5Vec, T5Single>
		where T1Comp : IComponent<T1Comp, T1Vec, T1Single>, allows ref struct
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Comp : IComponent<T2Comp, T2Vec, T2Single>, allows ref struct
		where T2Vec : unmanaged
		where T2Single : unmanaged
		where T3Comp : IComponent<T3Comp, T3Vec, T3Single>, allows ref struct
		where T3Vec : unmanaged
		where T3Single : unmanaged
		where T4Comp : IComponent<T4Comp, T4Vec, T4Single>, allows ref struct
		where T4Vec : unmanaged
		where T4Single : unmanaged
		where T5Comp : IComponent<T5Comp, T5Vec, T5Single>, allows ref struct
		where T5Vec : unmanaged
		where T5Single : unmanaged
	{
		public ref struct Enumerator<T1Arch>
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>, IArchType<T1Arch, T3Comp, T3Vec, T3Single>, IArchType<T1Arch, T4Comp, T4Vec, T4Single>, IArchType<T1Arch, T5Comp, T5Vec, T5Single>
		{
			public ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single> Current
			{
				get
				{
					return slice;
				}
			}

			public int Remaining
			{
				get
				{
					return Math.Min(8, remaining);
				}
			}

			ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single> slice;

			Span<T1Arch>.Enumerator e1;
			Span<T1Arch> span1;
			int count;
			int remaining;

			public Enumerator(Span<T1Arch> span1, int count)
			{
				this.span1 = span1;
				this.count = count;

				Reset();
			}

			public bool MoveNext()
			{
				if (e1.MoveNext())
				{
					slice = new(ref T1Comp.GetVec(ref e1.Current), ref T1Comp.GetSingle(ref e1.Current), ref T2Comp.GetVec(ref e1.Current), ref T2Comp.GetSingle(ref e1.Current), ref T3Comp.GetVec(ref e1.Current), ref T3Comp.GetSingle(ref e1.Current), ref T4Comp.GetVec(ref e1.Current), ref T4Comp.GetSingle(ref e1.Current), ref T5Comp.GetVec(ref e1.Current), ref T5Comp.GetSingle(ref e1.Current));
					remaining -= 8;
				}
				else
				{
					return false;
				}

				return true;
			}

			public void Reset()
			{
				e1 = span1.GetEnumerator();
				remaining = count + 8;
			}
		}

		public Enumerator<T1Arch> GetEnumerator<T1Arch>(Span<T1Arch> span1, int count)
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>, IArchType<T1Arch, T3Comp, T3Vec, T3Single>, IArchType<T1Arch, T4Comp, T4Vec, T4Single>, IArchType<T1Arch, T5Comp, T5Vec, T5Single>
		{
			return new(span1, count);
		}
	}

	public ref struct ComponentEnumerable<T1Comp, T1Vec, T1Single, T2Comp, T2Vec, T2Single, T3Comp, T3Vec, T3Single, T4Comp, T4Vec, T4Single, T5Comp, T5Vec, T5Single, T6Comp, T6Vec, T6Single>
		where T1Comp : IComponent<T1Comp, T1Vec, T1Single>, allows ref struct
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Comp : IComponent<T2Comp, T2Vec, T2Single>, allows ref struct
		where T2Vec : unmanaged
		where T2Single : unmanaged
		where T3Comp : IComponent<T3Comp, T3Vec, T3Single>, allows ref struct
		where T3Vec : unmanaged
		where T3Single : unmanaged
		where T4Comp : IComponent<T4Comp, T4Vec, T4Single>, allows ref struct
		where T4Vec : unmanaged
		where T4Single : unmanaged
		where T5Comp : IComponent<T5Comp, T5Vec, T5Single>, allows ref struct
		where T5Vec : unmanaged
		where T5Single : unmanaged
		where T6Comp : IComponent<T6Comp, T6Vec, T6Single>, allows ref struct
		where T6Vec : unmanaged
		where T6Single : unmanaged
	{
		public ref struct Enumerator<T1Arch>
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>, IArchType<T1Arch, T3Comp, T3Vec, T3Single>, IArchType<T1Arch, T4Comp, T4Vec, T4Single>, IArchType<T1Arch, T5Comp, T5Vec, T5Single>, IArchType<T1Arch, T6Comp, T6Vec, T6Single>
		{
			public ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single, T6Vec, T6Single> Current
			{
				get
				{
					return slice;
				}
			}

			public int Remaining
			{
				get
				{
					return Math.Min(8, remaining);
				}
			}

			ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single, T6Vec, T6Single> slice;

			Span<T1Arch>.Enumerator e1;
			Span<T1Arch> span1;
			int count;
			int remaining;

			public Enumerator(Span<T1Arch> span1, int count)
			{
				this.span1 = span1;
				this.count = count;

				Reset();
			}

			public bool MoveNext()
			{
				if (e1.MoveNext())
				{
					slice = new(ref T1Comp.GetVec(ref e1.Current), ref T1Comp.GetSingle(ref e1.Current), ref T2Comp.GetVec(ref e1.Current), ref T2Comp.GetSingle(ref e1.Current), ref T3Comp.GetVec(ref e1.Current), ref T3Comp.GetSingle(ref e1.Current), ref T4Comp.GetVec(ref e1.Current), ref T4Comp.GetSingle(ref e1.Current), ref T5Comp.GetVec(ref e1.Current), ref T5Comp.GetSingle(ref e1.Current), ref T6Comp.GetVec(ref e1.Current), ref T6Comp.GetSingle(ref e1.Current));
					remaining -= 8;
				}
				else
				{
					return false;
				}

				return true;
			}

			public void Reset()
			{
				e1 = span1.GetEnumerator();
				remaining = count + 8;
			}
		}

		public Enumerator<T1Arch> GetEnumerator<T1Arch>(Span<T1Arch> span1, int count)
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>, IArchType<T1Arch, T3Comp, T3Vec, T3Single>, IArchType<T1Arch, T4Comp, T4Vec, T4Single>, IArchType<T1Arch, T5Comp, T5Vec, T5Single>, IArchType<T1Arch, T6Comp, T6Vec, T6Single>
		{
			return new(span1, count);
		}
	}

	public ref struct ComponentEnumerable<T1Comp, T1Vec, T1Single, T2Comp, T2Vec, T2Single, T3Comp, T3Vec, T3Single, T4Comp, T4Vec, T4Single, T5Comp, T5Vec, T5Single, T6Comp, T6Vec, T6Single, T7Comp, T7Vec, T7Single>
		where T1Comp : IComponent<T1Comp, T1Vec, T1Single>, allows ref struct
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Comp : IComponent<T2Comp, T2Vec, T2Single>, allows ref struct
		where T2Vec : unmanaged
		where T2Single : unmanaged
		where T3Comp : IComponent<T3Comp, T3Vec, T3Single>, allows ref struct
		where T3Vec : unmanaged
		where T3Single : unmanaged
		where T4Comp : IComponent<T4Comp, T4Vec, T4Single>, allows ref struct
		where T4Vec : unmanaged
		where T4Single : unmanaged
		where T5Comp : IComponent<T5Comp, T5Vec, T5Single>, allows ref struct
		where T5Vec : unmanaged
		where T5Single : unmanaged
		where T6Comp : IComponent<T6Comp, T6Vec, T6Single>, allows ref struct
		where T6Vec : unmanaged
		where T6Single : unmanaged
		where T7Comp : IComponent<T7Comp, T7Vec, T7Single>, allows ref struct
		where T7Vec : unmanaged
		where T7Single : unmanaged
	{
		public ref struct Enumerator<T1Arch>
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>, IArchType<T1Arch, T3Comp, T3Vec, T3Single>, IArchType<T1Arch, T4Comp, T4Vec, T4Single>, IArchType<T1Arch, T5Comp, T5Vec, T5Single>, IArchType<T1Arch, T6Comp, T6Vec, T6Single>, IArchType<T1Arch, T7Comp, T7Vec, T7Single>
		{
			public ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single, T6Vec, T6Single, T7Vec, T7Single> Current
			{
				get
				{
					return slice;
				}
			}

			public int Remaining
			{
				get
				{
					return Math.Min(8, remaining);
				}
			}

			ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single, T6Vec, T6Single, T7Vec, T7Single> slice;

			Span<T1Arch>.Enumerator e1;
			Span<T1Arch> span1;
			int count;
			int remaining;

			public Enumerator(Span<T1Arch> span1, int count)
			{
				this.span1 = span1;
				this.count = count;

				Reset();
			}

			public bool MoveNext()
			{
				if (e1.MoveNext())
				{
					slice = new(ref T1Comp.GetVec(ref e1.Current), ref T1Comp.GetSingle(ref e1.Current), ref T2Comp.GetVec(ref e1.Current), ref T2Comp.GetSingle(ref e1.Current), ref T3Comp.GetVec(ref e1.Current), ref T3Comp.GetSingle(ref e1.Current), ref T4Comp.GetVec(ref e1.Current), ref T4Comp.GetSingle(ref e1.Current), ref T5Comp.GetVec(ref e1.Current), ref T5Comp.GetSingle(ref e1.Current), ref T6Comp.GetVec(ref e1.Current), ref T6Comp.GetSingle(ref e1.Current), ref T7Comp.GetVec(ref e1.Current), ref T7Comp.GetSingle(ref e1.Current));
					remaining -= 8;
				}
				else
				{
					return false;
				}

				return true;
			}

			public void Reset()
			{
				e1 = span1.GetEnumerator();
				remaining = count + 8;
			}
		}

		public Enumerator<T1Arch> GetEnumerator<T1Arch>(Span<T1Arch> span1, int count)
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>, IArchType<T1Arch, T3Comp, T3Vec, T3Single>, IArchType<T1Arch, T4Comp, T4Vec, T4Single>, IArchType<T1Arch, T5Comp, T5Vec, T5Single>, IArchType<T1Arch, T6Comp, T6Vec, T6Single>, IArchType<T1Arch, T7Comp, T7Vec, T7Single>
		{
			return new(span1, count);
		}
	}

	public ref struct ComponentEnumerable<T1Comp, T1Vec, T1Single, T2Comp, T2Vec, T2Single, T3Comp, T3Vec, T3Single, T4Comp, T4Vec, T4Single, T5Comp, T5Vec, T5Single, T6Comp, T6Vec, T6Single, T7Comp, T7Vec, T7Single, T8Comp, T8Vec, T8Single>
		where T1Comp : IComponent<T1Comp, T1Vec, T1Single>, allows ref struct
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Comp : IComponent<T2Comp, T2Vec, T2Single>, allows ref struct
		where T2Vec : unmanaged
		where T2Single : unmanaged
		where T3Comp : IComponent<T3Comp, T3Vec, T3Single>, allows ref struct
		where T3Vec : unmanaged
		where T3Single : unmanaged
		where T4Comp : IComponent<T4Comp, T4Vec, T4Single>, allows ref struct
		where T4Vec : unmanaged
		where T4Single : unmanaged
		where T5Comp : IComponent<T5Comp, T5Vec, T5Single>, allows ref struct
		where T5Vec : unmanaged
		where T5Single : unmanaged
		where T6Comp : IComponent<T6Comp, T6Vec, T6Single>, allows ref struct
		where T6Vec : unmanaged
		where T6Single : unmanaged
		where T7Comp : IComponent<T7Comp, T7Vec, T7Single>, allows ref struct
		where T7Vec : unmanaged
		where T7Single : unmanaged
		where T8Comp : IComponent<T8Comp, T8Vec, T8Single>, allows ref struct
		where T8Vec : unmanaged
		where T8Single : unmanaged
	{
		public ref struct Enumerator<T1Arch>
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>, IArchType<T1Arch, T3Comp, T3Vec, T3Single>, IArchType<T1Arch, T4Comp, T4Vec, T4Single>, IArchType<T1Arch, T5Comp, T5Vec, T5Single>, IArchType<T1Arch, T6Comp, T6Vec, T6Single>, IArchType<T1Arch, T7Comp, T7Vec, T7Single>, IArchType<T1Arch, T8Comp, T8Vec, T8Single>
		{
			public ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single, T6Vec, T6Single, T7Vec, T7Single, T8Vec, T8Single> Current
			{
				get
				{
					return slice;
				}
			}

			public int Remaining
			{
				get
				{
					return Math.Min(8, remaining);
				}
			}

			ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single, T6Vec, T6Single, T7Vec, T7Single, T8Vec, T8Single> slice;

			Span<T1Arch>.Enumerator e1;
			Span<T1Arch> span1;
			int count;
			int remaining;

			public Enumerator(Span<T1Arch> span1, int count)
			{
				this.span1 = span1;
				this.count = count;

				Reset();
			}

			public bool MoveNext()
			{
				if (e1.MoveNext())
				{
					slice = new(ref T1Comp.GetVec(ref e1.Current), ref T1Comp.GetSingle(ref e1.Current), ref T2Comp.GetVec(ref e1.Current), ref T2Comp.GetSingle(ref e1.Current), ref T3Comp.GetVec(ref e1.Current), ref T3Comp.GetSingle(ref e1.Current), ref T4Comp.GetVec(ref e1.Current), ref T4Comp.GetSingle(ref e1.Current), ref T5Comp.GetVec(ref e1.Current), ref T5Comp.GetSingle(ref e1.Current), ref T6Comp.GetVec(ref e1.Current), ref T6Comp.GetSingle(ref e1.Current), ref T7Comp.GetVec(ref e1.Current), ref T7Comp.GetSingle(ref e1.Current), ref T8Comp.GetVec(ref e1.Current), ref T8Comp.GetSingle(ref e1.Current));
					remaining -= 8;
				}
				else
				{
					return false;
				}

				return true;
			}

			public void Reset()
			{
				e1 = span1.GetEnumerator();
				remaining = count + 8;
			}
		}

		public Enumerator<T1Arch> GetEnumerator<T1Arch>(Span<T1Arch> span1, int count)
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>, IArchType<T1Arch, T3Comp, T3Vec, T3Single>, IArchType<T1Arch, T4Comp, T4Vec, T4Single>, IArchType<T1Arch, T5Comp, T5Vec, T5Single>, IArchType<T1Arch, T6Comp, T6Vec, T6Single>, IArchType<T1Arch, T7Comp, T7Vec, T7Single>, IArchType<T1Arch, T8Comp, T8Vec, T8Single>
		{
			return new(span1, count);
		}
	}

	public ref struct ComponentEnumerable<T1Comp, T1Vec, T1Single, T2Comp, T2Vec, T2Single, T3Comp, T3Vec, T3Single, T4Comp, T4Vec, T4Single, T5Comp, T5Vec, T5Single, T6Comp, T6Vec, T6Single, T7Comp, T7Vec, T7Single, T8Comp, T8Vec, T8Single, T9Comp, T9Vec, T9Single>
		where T1Comp : IComponent<T1Comp, T1Vec, T1Single>, allows ref struct
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Comp : IComponent<T2Comp, T2Vec, T2Single>, allows ref struct
		where T2Vec : unmanaged
		where T2Single : unmanaged
		where T3Comp : IComponent<T3Comp, T3Vec, T3Single>, allows ref struct
		where T3Vec : unmanaged
		where T3Single : unmanaged
		where T4Comp : IComponent<T4Comp, T4Vec, T4Single>, allows ref struct
		where T4Vec : unmanaged
		where T4Single : unmanaged
		where T5Comp : IComponent<T5Comp, T5Vec, T5Single>, allows ref struct
		where T5Vec : unmanaged
		where T5Single : unmanaged
		where T6Comp : IComponent<T6Comp, T6Vec, T6Single>, allows ref struct
		where T6Vec : unmanaged
		where T6Single : unmanaged
		where T7Comp : IComponent<T7Comp, T7Vec, T7Single>, allows ref struct
		where T7Vec : unmanaged
		where T7Single : unmanaged
		where T8Comp : IComponent<T8Comp, T8Vec, T8Single>, allows ref struct
		where T8Vec : unmanaged
		where T8Single : unmanaged
		where T9Comp : IComponent<T9Comp, T9Vec, T9Single>, allows ref struct
		where T9Vec : unmanaged
		where T9Single : unmanaged
	{
		public ref struct Enumerator<T1Arch>
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>, IArchType<T1Arch, T3Comp, T3Vec, T3Single>, IArchType<T1Arch, T4Comp, T4Vec, T4Single>, IArchType<T1Arch, T5Comp, T5Vec, T5Single>, IArchType<T1Arch, T6Comp, T6Vec, T6Single>, IArchType<T1Arch, T7Comp, T7Vec, T7Single>, IArchType<T1Arch, T8Comp, T8Vec, T8Single>, IArchType<T1Arch, T9Comp, T9Vec, T9Single>
		{
			public ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single, T6Vec, T6Single, T7Vec, T7Single, T8Vec, T8Single, T9Vec, T9Single> Current
			{
				get
				{
					return slice;
				}
			}

			public int Remaining
			{
				get
				{
					return Math.Min(8, remaining);
				}
			}

			ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single, T6Vec, T6Single, T7Vec, T7Single, T8Vec, T8Single, T9Vec, T9Single> slice;

			Span<T1Arch>.Enumerator e1;
			Span<T1Arch> span1;
			int count;
			int remaining;

			public Enumerator(Span<T1Arch> span1, int count)
			{
				this.span1 = span1;
				this.count = count;

				Reset();
			}

			public bool MoveNext()
			{
				if (e1.MoveNext())
				{
					slice = new(ref T1Comp.GetVec(ref e1.Current), ref T1Comp.GetSingle(ref e1.Current), ref T2Comp.GetVec(ref e1.Current), ref T2Comp.GetSingle(ref e1.Current), ref T3Comp.GetVec(ref e1.Current), ref T3Comp.GetSingle(ref e1.Current), ref T4Comp.GetVec(ref e1.Current), ref T4Comp.GetSingle(ref e1.Current), ref T5Comp.GetVec(ref e1.Current), ref T5Comp.GetSingle(ref e1.Current), ref T6Comp.GetVec(ref e1.Current), ref T6Comp.GetSingle(ref e1.Current), ref T7Comp.GetVec(ref e1.Current), ref T7Comp.GetSingle(ref e1.Current), ref T8Comp.GetVec(ref e1.Current), ref T8Comp.GetSingle(ref e1.Current), ref T9Comp.GetVec(ref e1.Current), ref T9Comp.GetSingle(ref e1.Current));
					remaining -= 8;
				}
				else
				{
					return false;
				}

				return true;
			}

			public void Reset()
			{
				e1 = span1.GetEnumerator();
				remaining = count + 8;
			}
		}

		public Enumerator<T1Arch> GetEnumerator<T1Arch>(Span<T1Arch> span1, int count)
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>, IArchType<T1Arch, T3Comp, T3Vec, T3Single>, IArchType<T1Arch, T4Comp, T4Vec, T4Single>, IArchType<T1Arch, T5Comp, T5Vec, T5Single>, IArchType<T1Arch, T6Comp, T6Vec, T6Single>, IArchType<T1Arch, T7Comp, T7Vec, T7Single>, IArchType<T1Arch, T8Comp, T8Vec, T8Single>, IArchType<T1Arch, T9Comp, T9Vec, T9Single>
		{
			return new(span1, count);
		}
	}

	public ref struct ComponentEnumerable<T1Comp, T1Vec, T1Single, T2Comp, T2Vec, T2Single, T3Comp, T3Vec, T3Single, T4Comp, T4Vec, T4Single, T5Comp, T5Vec, T5Single, T6Comp, T6Vec, T6Single, T7Comp, T7Vec, T7Single, T8Comp, T8Vec, T8Single, T9Comp, T9Vec, T9Single, T10Comp, T10Vec, T10Single>
		where T1Comp : IComponent<T1Comp, T1Vec, T1Single>, allows ref struct
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Comp : IComponent<T2Comp, T2Vec, T2Single>, allows ref struct
		where T2Vec : unmanaged
		where T2Single : unmanaged
		where T3Comp : IComponent<T3Comp, T3Vec, T3Single>, allows ref struct
		where T3Vec : unmanaged
		where T3Single : unmanaged
		where T4Comp : IComponent<T4Comp, T4Vec, T4Single>, allows ref struct
		where T4Vec : unmanaged
		where T4Single : unmanaged
		where T5Comp : IComponent<T5Comp, T5Vec, T5Single>, allows ref struct
		where T5Vec : unmanaged
		where T5Single : unmanaged
		where T6Comp : IComponent<T6Comp, T6Vec, T6Single>, allows ref struct
		where T6Vec : unmanaged
		where T6Single : unmanaged
		where T7Comp : IComponent<T7Comp, T7Vec, T7Single>, allows ref struct
		where T7Vec : unmanaged
		where T7Single : unmanaged
		where T8Comp : IComponent<T8Comp, T8Vec, T8Single>, allows ref struct
		where T8Vec : unmanaged
		where T8Single : unmanaged
		where T9Comp : IComponent<T9Comp, T9Vec, T9Single>, allows ref struct
		where T9Vec : unmanaged
		where T9Single : unmanaged
		where T10Comp : IComponent<T10Comp, T10Vec, T10Single>, allows ref struct
		where T10Vec : unmanaged
		where T10Single : unmanaged
	{
		public ref struct Enumerator<T1Arch>
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>, IArchType<T1Arch, T3Comp, T3Vec, T3Single>, IArchType<T1Arch, T4Comp, T4Vec, T4Single>, IArchType<T1Arch, T5Comp, T5Vec, T5Single>, IArchType<T1Arch, T6Comp, T6Vec, T6Single>, IArchType<T1Arch, T7Comp, T7Vec, T7Single>, IArchType<T1Arch, T8Comp, T8Vec, T8Single>, IArchType<T1Arch, T9Comp, T9Vec, T9Single>, IArchType<T1Arch, T10Comp, T10Vec, T10Single>
		{
			public ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single, T6Vec, T6Single, T7Vec, T7Single, T8Vec, T8Single, T9Vec, T9Single, T10Vec, T10Single> Current
			{
				get
				{
					return slice;
				}
			}

			public int Remaining
			{
				get
				{
					return Math.Min(8, remaining);
				}
			}

			ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single, T6Vec, T6Single, T7Vec, T7Single, T8Vec, T8Single, T9Vec, T9Single, T10Vec, T10Single> slice;

			Span<T1Arch>.Enumerator e1;
			Span<T1Arch> span1;
			int count;
			int remaining;

			public Enumerator(Span<T1Arch> span1, int count)
			{
				this.span1 = span1;
				this.count = count;

				Reset();
			}

			public bool MoveNext()
			{
				if (e1.MoveNext())
				{
					slice = new(ref T1Comp.GetVec(ref e1.Current), ref T1Comp.GetSingle(ref e1.Current), ref T2Comp.GetVec(ref e1.Current), ref T2Comp.GetSingle(ref e1.Current), ref T3Comp.GetVec(ref e1.Current), ref T3Comp.GetSingle(ref e1.Current), ref T4Comp.GetVec(ref e1.Current), ref T4Comp.GetSingle(ref e1.Current), ref T5Comp.GetVec(ref e1.Current), ref T5Comp.GetSingle(ref e1.Current), ref T6Comp.GetVec(ref e1.Current), ref T6Comp.GetSingle(ref e1.Current), ref T7Comp.GetVec(ref e1.Current), ref T7Comp.GetSingle(ref e1.Current), ref T8Comp.GetVec(ref e1.Current), ref T8Comp.GetSingle(ref e1.Current), ref T9Comp.GetVec(ref e1.Current), ref T9Comp.GetSingle(ref e1.Current), ref T10Comp.GetVec(ref e1.Current), ref T10Comp.GetSingle(ref e1.Current));
					remaining -= 8;
				}
				else
				{
					return false;
				}

				return true;
			}

			public void Reset()
			{
				e1 = span1.GetEnumerator();
				remaining = count + 8;
			}
		}

		public Enumerator<T1Arch> GetEnumerator<T1Arch>(Span<T1Arch> span1, int count)
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>, IArchType<T1Arch, T2Comp, T2Vec, T2Single>, IArchType<T1Arch, T3Comp, T3Vec, T3Single>, IArchType<T1Arch, T4Comp, T4Vec, T4Single>, IArchType<T1Arch, T5Comp, T5Vec, T5Single>, IArchType<T1Arch, T6Comp, T6Vec, T6Single>, IArchType<T1Arch, T7Comp, T7Vec, T7Single>, IArchType<T1Arch, T8Comp, T8Vec, T8Single>, IArchType<T1Arch, T9Comp, T9Vec, T9Single>, IArchType<T1Arch, T10Comp, T10Vec, T10Single>
		{
			return new(span1, count);
		}
	}
}
