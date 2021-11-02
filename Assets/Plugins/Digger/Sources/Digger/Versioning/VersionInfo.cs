using System;
using System.Collections.Generic;

namespace Digger
{
    [Serializable]
    public class VersionInfo
    {
        public long Version;
        public List<Vector3i> AliveChunks;
    }
}