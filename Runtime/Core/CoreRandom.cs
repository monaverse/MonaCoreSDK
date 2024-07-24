using UnityEngine;

namespace Mona.SDK.Core
{
    public class CoreRandom
    {
        private const long a = 1103515245;
        private const long c = 12345;
        private const long m = 2147483648; // 2^31

        private long seed;

        public CoreRandom(string seedString)
        {
            seed = StringToLong(seedString);
        }

        private long StringToLong(string input)
        {
            long hash = 0;
            for (int i = 0; i < input.Length; i++)
            {
                hash = 31 * hash + input[i];
            }
            return hash;
        }

        public int Next()
        {
            seed = (a * seed + c) % m;
            return (int)seed;
        }

        public int Next(int minInclusive, int maxExclusive)
        {
            int min = minInclusive < maxExclusive ? minInclusive : maxExclusive;
            int max = minInclusive < maxExclusive ? maxExclusive : minInclusive;

            if (min == max)
                return min;

            long range = (long)maxExclusive - minInclusive;
            return (int)(Next() % range + minInclusive);
        }

        public float NextFloat()
        {
            return (float)Next() / m;
        }

        public float NextFloat(float minInclusive, float maxInclusive)
        {
            float min = minInclusive < maxInclusive ? minInclusive : maxInclusive;
            float max = minInclusive < maxInclusive ? maxInclusive : minInclusive;

            if (Mathf.Approximately(min, max))
                return min;

            float range = maxInclusive - minInclusive;
            return NextFloat() * range + minInclusive;
        }

        public void Reset(string seedString)
        {
            seed = StringToLong(seedString);
        }
    }
}
