﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public class PcgExtended
    {
        // This attribute ensures that every thread will get its own instance of PCG.
        // An alternative, since PCG supports streams, is to use a different stream per
        // thread. 
        [ThreadStatic]
        static PcgExtended _defaultInstance;

        int _advancePow2 = 16;
        uint[] _data;
        ulong _increment = 1442695040888963407ul;

        ulong _state;
        ulong _tableMask;
        int _tablePow2 = 10;
        int _tableSize;
        ulong _tickMask;

        /// <summary>
        ///   Default instance.
        /// </summary>
        public static PcgExtended Default
        {
            get
            {
                if (_defaultInstance == null)
                    _defaultInstance = new PcgExtended(PcgSeed.GuidBasedSeed(), ShiftedIncrement);
                return _defaultInstance;
            }
        }

        public int Next()
        {
            uint result = NextUInt();
            return (int)(result >> 1);
        }

        public int Next(int maxExclusive)
        {
            if (maxExclusive <= 0)
                throw new ArgumentException("Max Exclusive must be positive");

            uint uMaxExclusive = (uint)maxExclusive;
            uint threshold = (uint)-uMaxExclusive % uMaxExclusive;

            while (true)
            {
                uint result = NextUInt();
                if (result >= threshold)
                    return (int)(result % uMaxExclusive);
            }
        }

        public int Next(int minInclusive, int maxExclusive)
        {
            if (maxExclusive <= minInclusive)
                throw new ArgumentException("MaxExclusive cannot be smaller or equal to MinInclusive");

            uint uMaxExclusive = unchecked((uint)(maxExclusive - minInclusive));
            uint threshold = (uint)-uMaxExclusive % uMaxExclusive;

            while (true)
            {
                uint result = NextUInt();
                if (result >= threshold)
                    return (int)unchecked(result % uMaxExclusive + minInclusive);
            }
        }

        public int[] NextInts(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Zero count");

            var resultA = new int[count];
            for (int i = 0; i < count; i++)
                resultA[i] = Next();

            return resultA;
        }

        public int[] NextInts(int count, int maxExclusive)
        {
            if (count <= 0)
                throw new ArgumentException("Zero count");
            if (maxExclusive <= 0)
                throw new ArgumentException("Max Exclusive must be positive");

            var resultA = new int[count];
            for (int i = 0; i < count; i++)
                resultA[i] = Next(maxExclusive);

            return resultA;
        }

        public int[] NextInts(int count, int minInclusive, int maxExclusive)
        {
            if (count <= 0)
                throw new ArgumentException("Zero count");

            var resultA = new int[count];
            for (int i = 0; i < count; i++)
                resultA[i] = Next(minInclusive, maxExclusive);

            return resultA;
        }

        public uint NextUInt()
        {
            var index = _state & _tableMask;
            bool tick = (_state & _tickMask) == 0ul;
            if (tick)
            {
                bool carry = false;
                for (int i = 0; i < _tableSize; i++)
                {
                    if (carry)
                        carry = ExternalStep(ref _data[i], i + 1);

                    bool carry2 = ExternalStep(ref _data[i], i + 1);
                    carry = carry || carry2;
                }
            }

            uint rhs = _data[index];

            ulong oldState = _state;
            _state = unchecked(_state * Multiplier + _increment);
            int rot = (int)(oldState >> 59);
            uint xorShifted = (uint)(((oldState >> 18) ^ oldState) >> 27);
            uint lhs = (xorShifted >> rot) | (xorShifted << (-rot & 31));

            uint result = lhs ^ rhs;
            return result;
        }

        public uint NextUInt(uint maxExclusive)
        {
            uint threshold = (uint)-maxExclusive % maxExclusive;

            while (true)
            {
                uint result = NextUInt();
                if (result >= threshold)
                    return result % maxExclusive;
            }
        }

        public uint NextUInt(uint minInclusive, uint maxExclusive)
        {
            if (maxExclusive <= minInclusive)
                throw new ArgumentException();

            uint diff = maxExclusive - minInclusive;
            uint threshold = (uint)-diff % diff;

            while (true)
            {
                uint result = NextUInt();
                if (result >= threshold)
                    return result % diff + minInclusive;
            }
        }

        public uint[] NextUInts(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Zero count");

            var resultA = new uint[count];
            for (int i = 0; i < count; i++)
                resultA[i] = NextUInt();

            return resultA;
        }

        public uint[] NextUInts(int count, uint maxExclusive)
        {
            if (count <= 0)
                throw new ArgumentException("Zero count");

            var resultA = new uint[count];
            for (int i = 0; i < count; i++)
                resultA[i] = NextUInt(maxExclusive);

            return resultA;
        }

        public uint[] NextUInts(int count, uint minInclusive, uint maxExclusive)
        {
            if (count <= 0)
                throw new ArgumentException("Zero count");

            var resultA = new uint[count];
            for (int i = 0; i < count; i++)
                resultA[i] = NextUInt(minInclusive, maxExclusive);

            return resultA;
        }

        public float NextFloat()
        {
            return (float)(NextUInt() * ToDouble01);
        }

        public float NextFloat(float maxInclusive)
        {
            if (maxInclusive <= 0)
                throw new ArgumentException("Max must be larger than 0");

            return (float)(NextUInt() * ToDouble01) * maxInclusive;
        }

        public float NextFloat(float minInclusive, float maxInclusive)
        {
            if (maxInclusive < minInclusive)
                throw new ArgumentException("Max must be larger than min");

            return (float)(NextUInt() * ToDouble01) * (maxInclusive - minInclusive) + minInclusive;
        }

        public float[] NextFloats(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Zero count");

            var resultA = new float[count];
            for (int i = 0; i < count; i++)
                resultA[i] = NextFloat();

            return resultA;
        }

        public float[] NextFloats(int count, float maxInclusive)
        {
            if (count <= 0)
                throw new ArgumentException("Zero count");

            var resultA = new float[count];
            for (int i = 0; i < count; i++)
                resultA[i] = NextFloat(maxInclusive);

            return resultA;
        }

        public float[] NextFloats(int count, float minInclusive, float maxInclusive)
        {
            if (count <= 0)
                throw new ArgumentException("Zero count");

            var resultA = new float[count];
            for (int i = 0; i < count; i++)
                resultA[i] = NextFloat(minInclusive, maxInclusive);

            return resultA;
        }

        public double NextDouble()
        {
            return NextUInt() * ToDouble01;
        }

        public double NextDouble(double maxInclusive)
        {
            if (maxInclusive <= 0)
                throw new ArgumentException("Max must be larger than 0");

            return NextUInt() * ToDouble01 * maxInclusive;
        }

        public double NextDouble(double minInclusive, double maxInclusive)
        {
            if (maxInclusive <= minInclusive)
                throw new ArgumentException("Max must be larger than min");

            return NextUInt() * ToDouble01 * (maxInclusive - minInclusive) + minInclusive;
        }

        public double[] NextDoubles(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Zero count");

            var resultA = new double[count];
            for (int i = 0; i < count; i++)
                resultA[i] = NextDouble();

            return resultA;
        }

        public double[] NextDoubles(int count, double maxInclusive)
        {
            if (count <= 0)
                throw new ArgumentException("Zero count");

            var resultA = new double[count];
            for (int i = 0; i < count; i++)
                resultA[i] = NextDouble(maxInclusive);

            return resultA;
        }

        public double[] NextDoubles(int count, double minInclusive, double maxInclusive)
        {
            if (count <= 0)
                throw new ArgumentException("Zero count");

            var resultA = new double[count];
            for (int i = 0; i < count; i++)
                resultA[i] = NextDouble(minInclusive, maxInclusive);

            return resultA;
        }

        public byte NextByte()
        {
            uint result = NextUInt();
            return (byte)(result % 256);
        }

        public byte[] NextBytes(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Zero count");

            var resultA = new byte[count];
            for (int i = 0; i < count; i++)
                resultA[i] = NextByte();

            return resultA;
        }

        public bool NextBool()
        {
            uint result = NextUInt();
            return result % 2 == 1;
        }

        public bool[] NextBools(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Zero count");

            var resultA = new bool[count];
            for (int i = 0; i < count; i++)
                resultA[i] = NextBool();

            return resultA;
        }

        public int Equidistribution()
        {
            return 1 << _tablePow2;
        }

        public int EquidistributionPow2()
        {
            return _tablePow2;
        }

        public int PeriodPow2()
        {
            return (1 << _tablePow2) * 32 + 64;
        }

        public void SetStream(int sequence)
        {
            SetStream((ulong)sequence);
        }

        public void SetStream(ulong sequence)
        {
            // The increment must be odd.
            _increment = (sequence << 1) | 1;
        }

        public ulong CurrentStream()
        {
            return _increment >> 1;
        }

        public PcgExtended() : this(PcgSeed.GuidBasedSeed(), ShiftedIncrement)
        {
        }

        public PcgExtended(int seed) : this((ulong)seed, ShiftedIncrement)
        {
        }

        public PcgExtended(ulong seed) : this(seed, ShiftedIncrement)
        {
        }

        public PcgExtended(int seed, int sequence) : this((ulong)seed, (ulong)sequence)
        {
        }

        public PcgExtended(ulong seed, ulong sequence)
        {
            Initialize(seed, sequence);
        }

        public PcgExtended(int seed, int tablePow2, int advancePow2)
          : this((ulong)seed, tablePow2, advancePow2)
        {
        }

        public PcgExtended(ulong seed, int tablePow2, int advancePow2)
        {
            _tablePow2 = tablePow2;
            _advancePow2 = advancePow2;
            Initialize(seed, ShiftedIncrement);
        }

        public PcgExtended(int seed, int sequence, int tablePow2, int advancePow2)
          : this((ulong)seed, (ulong)sequence, tablePow2, advancePow2)
        {
        }

        public PcgExtended(ulong seed, ulong sequence, int tablePow2, int advancePow2)
        {
            _tablePow2 = tablePow2;
            _advancePow2 = advancePow2;
            Initialize(seed, sequence);
        }

        void Initialize(ulong seed, ulong sequence)
        {
            SetStream(sequence);
            _state = seed + _increment;
            _state = Bump(_state);

            _tableSize = 1 << _tablePow2;
            _data = new uint[_tableSize];
            // The correct way to initialize this is to use a seed sequence, but this 
            // is more convenient. 
            uint xdiff = InternalNext() - InternalNext();
            for (int i = 0; i < _tableSize; i++)
                _data[i] = InternalNext() ^ xdiff;

            _tableMask = (1ul << _tablePow2) - 1ul;
            _tickMask = (1ul << _advancePow2) - 1ul;
        }

        uint InternalNext()
        {
            ulong oldState = _state;
            // Overflow is part of the design.
            _state = unchecked(oldState * Multiplier + _increment);
            uint xorShifted = (uint)(((oldState >> 18) ^ oldState) >> 27);
            int rot = (int)(oldState >> 59);
            return (xorShifted >> rot) | (xorShifted << (-rot & 31));
        }

        ulong Bump(ulong state)
        {
            return state * Multiplier + _increment;
        }

        bool ExternalStep(ref uint randval, int i)
        {
            uint state = UnOutput(randval);
            state = unchecked(state * PcgMultiplier + PcgIncrement + (uint)i * 2u);

            int rshift = (int)(state >> 28) + 4;
            state ^= state >> rshift;
            state *= McgMultiplier;
            uint result = state ^ (state >> 22);
            randval = result;

            return result == 0u;
        }

        uint UnOutput(uint @internal)
        {
            // unxorshift
            @internal ^= @internal >> 22;
            @internal *= McgUnmultiplier;
            int rshift = (int)(@internal >> 28);
            @internal = UnXorShift(@internal, 32, 4 + rshift);
            return @internal;
        }

        uint UnXorShift(uint x, int bits, int shift)
        {
            if (2 * shift >= bits)
                return x ^ (x >> shift);

            uint lowmask1 = (1u << (bits - 2 * shift)) - 1u;
            uint highmask1 = ~lowmask1;
            uint top1 = x;
            uint bottom1 = x & lowmask1;
            top1 ^= top1 >> shift;
            top1 &= highmask1;
            x = top1 | bottom1;
            uint lowmask2 = (1u << (bits - shift)) - 1u;
            uint bottom2 = x & lowmask2;
            bottom2 = UnXorShift(bottom2, bits - shift, shift);
            bottom2 &= lowmask1;
            return top1 | bottom2;
        }

        // This shifted to the left and or'ed with 1ul results in the default increment.
        const ulong ShiftedIncrement = 721347520444481703ul;
        const ulong Multiplier = 6364136223846793005ul;
        const uint PcgMultiplier = 747796405u;
        const uint PcgIncrement = 2891336453u;
        const uint McgMultiplier = 277803737u;
        const uint McgUnmultiplier = 2897767785u;

        // 1 / (uint.MaxValue + 1)
        const double ToDouble01 = 1.0 / 4294967296.0;
    }
}
