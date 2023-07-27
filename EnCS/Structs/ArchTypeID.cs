using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EnCS
{
    public unsafe ref struct ArchTypeID
    {
        fixed int data[Config.MAX_COMPONENTS / 32];

        public ArchTypeID(params uint[] components)
        {
            for (int i = 0; i < components.Length; i++)
            {
#if ERROR_CHECK
                if (i >= Config.MAX_COMPONENTS)
                    throw new IndexOutOfRangeException("Component id out of range.");
#endif
                Set((int)components[i]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Get(int id)
        {
#if ERROR_CHECK
            if (id < 0 || id >= Config.MAX_COMPONENTS)
                throw new IndexOutOfRangeException("Index out of bounds.");
#endif

            return (data[id / 32] & (1 << (id % 32))) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count()
        {
            fixed(int* pData = data)
            {
                int count = 0;
                for (int i = 0; i < Config.MAX_COMPONENTS / 32; i++)
                {
                    count += BitOperations.PopCount(((uint*)pData)[0]);
                }
                return count;
            }
        }

        /// <summary>
        /// Check if id is subset of this.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(in ArchTypeID id)
        {
            for (int i = 0; i < Config.MAX_COMPONENTS / 32; i++)
            {
                if ((data[i] & id.data[i]) != id.data[i])
                    return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Set(int id)
        {
#if ERROR_CHECK
            if (id < 0 || id >= Config.MAX_COMPONENTS)
                throw new IndexOutOfRangeException("Index out of bounds.");
#endif

            data[id / 32] |= 1 << (id % 32);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < Config.MAX_COMPONENTS; i++)
            {
                builder.Append(Get(i) ? 1 : 0);
            }

            return builder.ToString();
        }

        public static bool operator ==(in ArchTypeID left, in ArchTypeID right)
        {
            for (int i = 0; i < Config.MAX_COMPONENTS / 32; i++)
            {
                if (left.data[i] != right.data[i])
                    return false;
            }

            return true;
        }

        public static bool operator !=(in ArchTypeID left, in ArchTypeID right)
        {
            return !(left == right);
        }

        public static ArchTypeID Combine(in ArchTypeID left, in ArchTypeID right)
        {
            ArchTypeID id = new ArchTypeID();
            for (int i = 0; i < Config.MAX_COMPONENTS; i++)
            {
                if (left.Get(i) || right.Get(i))
                    id.Set(i);
            }

            return id;
        }

        public static ArchTypeID Add(in ArchTypeID left, uint id)
        {
            ArchTypeID archtype = new ArchTypeID();
            for (int i = 0; i < Config.MAX_COMPONENTS; i++)
                archtype.Set(i);

            archtype.Set((int)id);
            return archtype;
        }
    }
}
