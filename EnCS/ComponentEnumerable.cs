using System;
using System.Runtime.CompilerServices;

namespace EnCS
{
	public ref struct ComponentEnumerableNew<T1Comp, T1Vec, T1Single>
		where T1Comp : unmanaged, IComponent<T1Comp, T1Vec, T1Single>
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

			ArchTypeSlice<T1Vec, T1Single> slice;

			Span<T1Arch>.Enumerator e1;

			public Enumerator(Span<T1Arch> span1)
			{
				e1 = span1.GetEnumerator();
			}

			public bool MoveNext()
			{
				if (e1.MoveNext())
				{
					slice = new(ref T1Comp.GetVec(ref e1.Current), ref T1Comp.GetSingle(ref e1.Current));
				}
				else
				{
					return false;
				}

				return true;
			}
		}

		public ref struct Enumerator<T1Arch, T2Arch>
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>
			where T2Arch : unmanaged, IArchType<T2Arch, T1Comp, T1Vec, T1Single>
		{
			public ArchTypeSlice<T1Vec, T1Single> Current
			{
				get
				{
					return slice;
				}
			}

			ArchTypeSlice<T1Vec, T1Single> slice;

			Span<T1Arch>.Enumerator e1;
			Span<T2Arch>.Enumerator e2;

			public Enumerator(Span<T1Arch> span1, Span<T2Arch> span2)
			{
				e1 = span1.GetEnumerator();
				e2 = span2.GetEnumerator();
			}

			public bool MoveNext()
			{
				if (e1.MoveNext())
				{
					slice = new(ref T1Comp.GetVec(ref e1.Current), ref T1Comp.GetSingle(ref e1.Current));
				}
				else if(e2.MoveNext())
				{
					slice = new(ref T1Comp.GetVec(ref e2.Current), ref T1Comp.GetSingle(ref e2.Current));
				}
				else
				{
					return false;
				}

				return true;
			}
		}

		public Enumerator<T1Arch> GetEnumerator<T1Arch>(Span<T1Arch> span1)
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>
		{
			return new (span1);
		}

		public Enumerator<T1Arch, T2Arch> GetEnumerator<T1Arch, T2Arch>(Span<T1Arch> span1, Span<T2Arch> span2)
			where T1Arch : unmanaged, IArchType<T1Arch, T1Comp, T1Vec, T1Single>
			where T2Arch : unmanaged, IArchType<T2Arch, T1Comp, T1Vec, T1Single>
		{
			return new (span1, span2);
		}
	}

	/*
	public ref struct ComponentEnumerable<TArch, T1Comp, T1Vec, T1Single>
		where TArch : unmanaged, IArchType<TArch, T1Comp, T1Vec, T1Single>
		where T1Comp : unmanaged, IComponent<T1Comp, T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
	{
		public ref struct Enumerator
		{
			public ArchTypeSlice<T1Vec, T1Single> Current
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

	public ref struct ComponentEnumerable<TArch, T1Comp, T1Vec, T1Single, T2Comp, T2Vec, T2Single>
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

		public ComponentEnumerable(Span<TArch> span)
		{
			this.span = span;
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(span);
		}
	}
	*/
}
