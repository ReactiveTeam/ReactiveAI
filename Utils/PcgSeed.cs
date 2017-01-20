using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public static class PcgSeed
    {
        /// <summary>
        ///   Provides a Time-dependent seed value, matching the default behavior of System.Random.
        /// </summary>
        public static ulong TimeBasedSeed()
        {
            return (ulong)Environment.TickCount;
        }

        /// <summary>
        ///   Provides a seed based on Time and unique GUIDs.
        /// </summary>
        public static ulong GuidBasedSeed()
        {
            ulong upper = (ulong)(Environment.TickCount ^ Guid.NewGuid().GetHashCode()) << 32;
            ulong lower = (ulong)(Environment.TickCount ^ Guid.NewGuid().GetHashCode());
            return upper | lower;
        }
    }
}
