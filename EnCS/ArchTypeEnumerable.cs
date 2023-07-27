using System.Runtime.CompilerServices;

namespace EnCS
{
	public ref struct ContainerEnumerable<T1Comp, T1Vec, T1Single>
		where T1Comp : unmanaged, IComponent<T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
	{
		public static void GetEnumerable<TContainerManager, T1Arch>()
			where TContainerManager : unmanaged, IContainerManager<T1Arch, T1Comp, T1Vec, T1Single>
			where T1Arch : unmanaged, IArchType<T1Comp, T1Vec, T1Single>
		{

		}
	}

	public ref struct ArchTypeEnumerable<TEnum, T1Comp, T1Vec, T1Single>
		where TEnum : unmanaged, IArchTypeEnumerable<TEnum, T1Comp, T1Vec, T1Single>
		where T1Comp : unmanaged, IComponent<T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
	{
		public ref struct Enumerator
		{
			public ArchTypeSlice<TEnum, T1Comp, T1Vec, T1Single> Current
			{
				get
				{
                    return new (ref enumerator.Current);
				}
			}

			Span<TEnum>.Enumerator enumerator;

			public Enumerator(Span<TEnum> span)
			{
				enumerator = span.GetEnumerator();
			}

			public bool MoveNext()
			 => enumerator.MoveNext();
		}

		Span<TEnum> span;

        public ArchTypeEnumerable(Span<TEnum> span)
        {
			this.span = span;
        }

        public Enumerator GetEnumerator()
		{
			return new Enumerator(span);
		}
	}

	public ref struct ArchTypeEnumerable<TEnum, T1, T1Vec, T1Single, T2, T2Vec, T2Single>
		where TEnum : unmanaged, IArchTypeEnumerable<TEnum, T1, T1Vec, T1Single>, IArchTypeEnumerable<TEnum, T2, T2Vec, T2Single>
		where T1 : unmanaged, IComponent<T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2 : unmanaged, IComponent<T2Vec, T2Single>
		where T2Vec : unmanaged
		where T2Single : unmanaged
	{
		public ref struct Enumerator
		{
			public readonly ArchTypeSlice<TEnum, T1, T1Vec, T1Single, T2, T2Vec, T2Single> Current
			{
				get
				{
					return new(ref enumerator.Current);
				}
			}

			Span<TEnum>.Enumerator enumerator;

			public Enumerator(Span<TEnum> span)
			{
				enumerator = span.GetEnumerator();
			}

			public bool MoveNext()
			 => enumerator.MoveNext();
		}

		Span<TEnum> span;

		public ArchTypeEnumerable(Span<TEnum> span)
		{
			this.span = span;
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(span);
		}
	}
}
