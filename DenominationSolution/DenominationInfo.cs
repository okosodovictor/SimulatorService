using System;
namespace DenominationSolution
{
	public readonly struct DenominationInfo
	{
        public int Denomination { get; }

        public int Count { get; }

        public DenominationInfo(int denomination, int count)
        {
            Count = count;
            Denomination = denomination;
        }
    }
}

