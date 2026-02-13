using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using UtilLib.Memory;
using UtilLib.Span;

namespace EnCS
{
	public struct HierarchicalContainer<TArchMem, TVec, TSingle> : IHierarchicalContainer<TVec, TSingle>, IIndexedContainer<TVec, TSingle>
		where TArchMem : IArchMemory<TArchMem, TVec, TSingle>
		where TVec : allows ref struct
		where TSingle : allows ref struct
	{
		public readonly nint Entities => entityCount;

		TArchMem memory;

		Memory<int> map;
		Memory<SpanHierarchy.Node> nodes;
		int entityCount;

		public HierarchicalContainer(int maxNodes)
		{
			this.map = new int[maxNodes];
			this.memory = TArchMem.Create(maxNodes);
			this.nodes =  new SpanHierarchy.Node[maxNodes];
			this.entityCount = 0;
		}

		public ArchRef<TSingle> CreateRoot()
		{
			SpanHierarchy<int> hierarchy = new(map.Span, nodes.Span, ref entityCount);
			return new ArchRef<TSingle>(hierarchy.SetRoot(entityCount));
		}

		public ArchRef<TSingle> CreateChild(in ArchRef<TSingle> parentPtr)
		{
			SpanHierarchy<int> hierarchy = new(map.Span, nodes.Span, ref entityCount);
			return new ArchRef<TSingle>(hierarchy.CreateChild((int)parentPtr.idx, entityCount));
		}

		public void DeleteChild(in ArchRef<TSingle> ptr)
		{
			SpanHierarchy<int> hierarchy = new(map.Span, nodes.Span, ref entityCount); // TODO: Delete is not entierly function, add delted map
			hierarchy.DeleteNode((int)ptr.idx);
		}

		public ChildrenEnumerator<TSingle> GetChildren(ref readonly ArchRef<TSingle> parentPtr)
		{
			SpanHierarchy<int> hierarchy = new(map.Span, nodes.Span, ref entityCount);
			return new ChildrenEnumerator<TSingle>(hierarchy.GetChildren((int)parentPtr.idx));
		}

		public ArchRef<TSingle> GetRoot()
		{
			SpanHierarchy<int> hierarchy = new(map.Span, nodes.Span, ref entityCount);
			return new ArchRef<TSingle>(hierarchy.GetRoot());
		}

		public TSingle GetSingle(ref readonly ArchRef<TSingle> ptr)
		{
			return memory.GetSingle(ptr.idx);
		}

		public TVec GetVec(ref readonly ArchRef<TSingle> ptr)
		{
			return memory.GetVec(ptr.idx);
		}

		public TSingle GetSingle(nint idx)
		{
			return memory.GetSingle(idx);
		}

		public TVec GetVec(nint idx)
		{
			return memory.GetVec(idx);
		}

		public FixedRefBuffer8<TSingle> GetSingleArray(nint idx)
		{
			return memory.GetSingleArray(idx);
		}
	}
}
