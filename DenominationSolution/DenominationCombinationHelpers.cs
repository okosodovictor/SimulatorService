using System;
namespace DenominationSolution
{
	public static class DenominationCombinationHelpers
	{
        public static DenominationInfo[][] Combinations(int value)
        {
            var hundreds = Math.DivRem(value, 100, out var hrem);
            var fifties = Math.DivRem(hrem, 50, out var trem);
            var tens = Math.DivRem(trem, 10, out var urem);

            if (urem != 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            return DenominationCombinationHelpers
                .Combination10(tens)
                .CrossConcat(Combination50(fifties))
                .CrossConcat(Combination100(hundreds))
                .Select(Normalize)
                .ToArray();
        }


        public static DenominationInfo[][] CrossConcat(this
            DenominationInfo[][] left,
            DenominationInfo[][] right)
        {
            if (left.Length == 0)
            {
                return right;
            }

            if (right.Length == 0)
            {
                return left;
            }

            var concatenation = new List<DenominationInfo[]>();

            foreach (var lcombination in left)
            {
                foreach (var rcombination in right)
                {
                    concatenation.Add(lcombination
                        .Concat(rcombination)
                        .ToArray());
                }
            }

            return concatenation.ToArray();
        }

        public static DenominationInfo[][] Combination10(int count)
        {
            return Multiply(count, new[]
            {
                new [] {new DenominationInfo(10, 1)}
            });
        }

        public static DenominationInfo[][] Combination50(int count)
        {
            return Multiply(count, new[]
            {
                new [] {new DenominationInfo(50, 1)},
                new [] {new DenominationInfo(10, 5)}
            });
        }

        public static DenominationInfo[][] Combination100(int count)
        {
            return Multiply(count, new[]
            {
                new [] {new DenominationInfo(100, 1)},
                new [] {new DenominationInfo(50, 2)},
                new [] {new DenominationInfo(50, 1), new DenominationInfo(10, 5)},
                new [] {new DenominationInfo(10, 10)}
            });
        }

        public static DenominationInfo[][] Multiply(
            int count,
            DenominationInfo[][] combination)
        {
            return Enumerable
                .Range(0, count)
                .Select(i => combination)
                .Aggregate(Array.Empty<DenominationInfo[]>(), CrossConcat)
                .ToArray();
        }

        public static DenominationInfo[] Normalize(DenominationInfo[] combination)
        {
            return combination
                .GroupBy(info => info.Denomination)
                .Select(group => new DenominationInfo(group.Key, group.Sum(info => info.Count)))
                .ToArray();
        }

        public static void PrintCombinations(int amount, List<DenominationInfo[]> combinations)
        {
            Console.WriteLine($"For {amount} EUR:");

            foreach (var combo in combinations)
            {
                Console.WriteLine($"  {string.Join(" + ", combo.Select(info => $"{info.Count} x {info.Denomination} EUR"))}");
            }

            Console.WriteLine();
        }
    }
}

