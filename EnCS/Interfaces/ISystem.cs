namespace EnCS
{
	public interface ISystem<TComp, TVector, TSingle>
		where TComp : unmanaged, IComponent<TComp, TVector, TSingle>
		where TVector : unmanaged
		where TSingle : unmanaged
	{
		void Update(ref TVector vector);

		void Update<T1Arch>(ref ComponentEnumerableNew<TComp, TVector, TSingle>.Enumerator<T1Arch> loop)
			where T1Arch : unmanaged, IArchType<T1Arch, TComp, TVector, TSingle>;
	}
}
