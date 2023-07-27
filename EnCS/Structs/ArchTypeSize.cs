using System.Runtime.CompilerServices;
using System.Text;

namespace EnCS
{
    public unsafe ref struct ArchTypeSize
    {
        fixed short data[Config.MAX_COMPONENTS_PER_ARCHTYPE + 1];

        public ArchTypeSize(params short[] size)
        {
#if ERROR_CHECK
            if (size.Length >= Config.MAX_COMPONENTS_PER_ARCHTYPE)
                throw new IndexOutOfRangeException($"Each archtype cannot contain more than {Config.MAX_COMPONENTS_PER_ARCHTYPE} components.");
#endif

            short window = 0;
            for (int i = 0; i < size.Length; i++)
            {
                data[i] = size[i];
                data[i] += window;
                window += size[i];
            }

            data[Config.MAX_COMPONENTS_PER_ARCHTYPE] = window;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref readonly short GetOffset(uint id)
        {
#if ERROR_CHECK
            if (id < 0 || id >= Config.MAX_COMPONENTS_PER_ARCHTYPE)
                throw new IndexOutOfRangeException("Index out of bounds.");
#endif

            return ref data[id];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref readonly short GetWindow()
        {
            return ref data[Config.MAX_COMPONENTS_PER_ARCHTYPE];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetSize(uint id)
        {
#if ERROR_CHECK
            if (id < 0 || id >= Config.MAX_COMPONENTS_PER_ARCHTYPE - 1)
                throw new IndexOutOfRangeException("Index out of bounds.");
#endif

            return data[id + 1] - data[id];
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('(');

            for (int i = 0; i < Config.MAX_COMPONENTS_PER_ARCHTYPE; i++)
            {
                builder.Append($"{data[i]},");
            }

            builder.Append($"{data[Config.MAX_COMPONENTS_PER_ARCHTYPE]}");
            builder.Append(')');

            return builder.ToString();
        }
    }
}
