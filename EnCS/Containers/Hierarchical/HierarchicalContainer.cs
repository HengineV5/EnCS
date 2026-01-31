using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using UtilLib.Span;

namespace EnCS
{
	public struct HierarchicalContainer<TArch, TPtr> : IHierarchicalContainer<HierarchicalContainer<TArch, TPtr>, TArch, TPtr>, IIndexedContainer<HierarchicalContainer<TArch, TPtr>, TArch, TPtr>
		where TArch : unmanaged
		where TPtr : allows ref struct
	{
		public readonly nint Entities => entityCount;

		Memory<TArch> buff;
		Memory<SpanHierarchy.Node> nodes;
		int entityCount;

		public HierarchicalContainer(nuint maxNodes)
		{
			this.buff = new TArch[maxNodes];
			this.nodes =  new SpanHierarchy.Node[maxNodes];
			this.entityCount = 0;
		}

		public ArchRef<TPtr> CreateRoot(in TArch data)
		{
			SpanHierarchy<TArch> hierarchy = new(buff.Span, nodes.Span, ref entityCount);
			return new ArchRef<TPtr>(hierarchy.SetRoot(data));
		}

		public ArchRef<TPtr> CreateChild(in ArchRef<TPtr> parentPtr, in TArch data)
		{
			SpanHierarchy<TArch> hierarchy = new(buff.Span, nodes.Span, ref entityCount);
			return new ArchRef<TPtr>(hierarchy.CreateChild((int)parentPtr.idx, data));
		}

		public void DeleteChild(in ArchRef<TPtr> ptr)
		{
			SpanHierarchy<TArch> hierarchy = new(buff.Span, nodes.Span, ref entityCount);
			hierarchy.DeleteNode((int)ptr.idx);
		}

		public ChildrenEnumerator<TPtr, TArch> GetChildren(in ArchRef<TPtr> parentPtr)
		{
			SpanHierarchy<TArch> hierarchy = new(buff.Span, nodes.Span, ref entityCount);
			return new ChildrenEnumerator<TPtr, TArch>(hierarchy.GetChildren((int)parentPtr.idx));
		}

		public Span<TArch> GetChildrenValues(ArchRef<TPtr> parentPtr)
		{
			SpanHierarchy<TArch> hierarchy = new(buff.Span, nodes.Span, ref entityCount);
			return buff.Span[hierarchy.GetChildren((int)parentPtr.idx)];
		}

		public ArchRef<TPtr> GetRoot()
		{
			SpanHierarchy<TArch> hierarchy = new(buff.Span, nodes.Span, ref entityCount);
			return new ArchRef<TPtr>(hierarchy.GetRoot());
		}

		public ref TArch GetValue(ArchRef<TPtr> ptr)
		{
			return ref buff.Span[(int)ptr.idx];
		}

		public ref TArch GetValue(nint idx)
		{
			return ref buff.Span[(int)idx];
		}
	}

	public ref struct ChildrenEnumerator<TPtr, TArch>
		where TArch : unmanaged
		where TPtr : allows ref struct
	{
		public ArchRef<TPtr> Current => new ArchRef<TPtr>(currentIdx);

		Range range;
		int currentIdx;

		public ChildrenEnumerator(Range range)
		{
			this.range = range;
			this.currentIdx = range.Start.Value - 1;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			currentIdx++;

			if (currentIdx >= range.End.Value)
				return false;

			return true;
		}

		public void Reset()
		{
			currentIdx = range.Start.Value - 1;
		}
	}
}
