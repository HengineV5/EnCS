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
}
