using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Digger.NativeCollections
{
    public static class Utils
    {
        public static void IncrementAt(NativeArray<int> bytes, int index)
        {
            unsafe {
                Interlocked.Increment(ref ((int*) bytes.GetUnsafePtr())[index]);
            }
        }
    }
}