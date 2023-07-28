using System;
using System.Runtime.CompilerServices;

namespace EnCS
{
	public ref struct ArchTypeEnumerable<T1Arch, T1Comp, T1Vec, T1Single>
		where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>
		where T1Comp : unmanaged, IComponent<T1Comp, T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
	{
		public ref struct Enumerator
		{
			public ArchTypeSliceNew<T1Vec, T1Single> Current
			{
				get
				{
					return enumerator1.Current;
				}
			}

			ComponentEnumerable<T1Arch, T1Comp, T1Vec, T1Single>.Enumerator enumerator1;

			public Enumerator(ComponentEnumerable<T1Arch, T1Comp, T1Vec, T1Single> e1)
			{
				enumerator1 = e1.GetEnumerator();
			}

			public bool MoveNext()
			 => enumerator1.MoveNext();
		}

		ComponentEnumerable<T1Arch, T1Comp, T1Vec, T1Single> e1;

		public ArchTypeEnumerable(ComponentEnumerable<T1Arch, T1Comp, T1Vec, T1Single> e1)
		{
			this.e1 = e1;
		}

		public ArchTypeEnumerable(Span<T1Arch> span1)
		{
			this.e1 = new(span1);
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(e1);
		}
	}

	public ref struct ArchTypeEnumerable<T1Arch, T2Arch, T1Comp, T1Vec, T1Single>
		where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>
		where T2Arch : unmanaged, IArchType<T2Arch, T1Comp, T1Vec, T1Single>
		where T1Comp : unmanaged, IComponent<T1Comp, T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
	{
		public ref struct Enumerator
		{
			public ArchTypeSliceNew<T1Vec, T1Single> Current
			{
				get
				{
					return slice;
				}
			}

			ArchTypeSliceNew<T1Vec, T1Single> slice;
			ComponentEnumerable<T1Arch, T1Comp, T1Vec, T1Single>.Enumerator enumerator1;
			ComponentEnumerable<T2Arch, T1Comp, T1Vec, T1Single>.Enumerator enumerator2;

			public Enumerator(ComponentEnumerable<T1Arch, T1Comp, T1Vec, T1Single> e1, ComponentEnumerable<T2Arch, T1Comp, T1Vec, T1Single> e2)
			{
				enumerator1 = e1.GetEnumerator();
				enumerator2 = e2.GetEnumerator();
			}

			public bool MoveNext()
			{
				if (enumerator1.MoveNext())
				{
					slice = enumerator1.Current;
				}
				else if (enumerator2.MoveNext())
				{
					slice = enumerator2.Current;
				}
				else
				{
					return false;
				}

				return true;
			}
		}

		ComponentEnumerable<T1Arch, T1Comp, T1Vec, T1Single> e1;
		ComponentEnumerable<T2Arch, T1Comp, T1Vec, T1Single> e2;

		public ArchTypeEnumerable(ComponentEnumerable<T1Arch, T1Comp, T1Vec, T1Single> e1, ComponentEnumerable<T2Arch, T1Comp, T1Vec, T1Single> e2)
		{
			this.e1 = e1;
			this.e2 = e2;
		}

		public ArchTypeEnumerable(Span<T1Arch> span1, Span<T2Arch> span2)
		{
			this.e1 = new(span1);
			this.e2 = new(span2);
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(e1, e2);
		}
	}

	public ref struct ComponentEnumerable<TArch, T1Comp, T1Vec, T1Single>
		where TArch : unmanaged, IArchType<TArch, T1Comp, T1Vec, T1Single>
		where T1Comp : unmanaged, IComponent<T1Comp, T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
	{
		public ref struct Enumerator
		{
			public ArchTypeSliceNew<T1Vec, T1Single> Current
			{
				get
				{
                    return new (
						ref T1Comp.GetVec(ref enumerator.Current),
						ref T1Comp.GetSingle(ref enumerator.Current)
					);
				}
			}

			Span<TArch>.Enumerator enumerator;

			public Enumerator(Span<TArch> span)
			{
				enumerator = span.GetEnumerator();
			}

			public bool MoveNext()
			 => enumerator.MoveNext();
		}

		Span<TArch> span;

        public ComponentEnumerable(Span<TArch> span)
        {
			this.span = span;
        }

        public Enumerator GetEnumerator()
		{
			return new Enumerator(span);
		}
	}

	public ref struct ArchTypeEnumerable<TArch, T1Comp, T1Vec, T1Single, T2Comp, T2Vec, T2Single>
		where TArch : unmanaged, IArchType<TArch, T1Comp, T1Vec, T1Single>, IArchType<TArch, T2Comp, T2Vec, T2Single>
		where T1Comp : unmanaged, IComponent<T1Comp, T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Comp : unmanaged, IComponent<T2Comp, T2Vec, T2Single>
		where T2Vec : unmanaged
		where T2Single : unmanaged
	{
		public ref struct Enumerator
		{
			public readonly ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single> Current
			{
				get
				{
					return new(
						ref T1Comp.GetVec(ref enumerator.Current),
						ref T1Comp.GetSingle(ref enumerator.Current),
						ref T2Comp.GetVec(ref enumerator.Current),
						ref T2Comp.GetSingle(ref enumerator.Current)
					);
				}
			}

			Span<TArch>.Enumerator enumerator;

			public Enumerator(Span<TArch> span)
			{
				enumerator = span.GetEnumerator();
			}

			public bool MoveNext()
			 => enumerator.MoveNext();
		}

		Span<TArch> span;

		public ArchTypeEnumerable(Span<TArch> span)
		{
			this.span = span;
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(span);
		}
	}
}
