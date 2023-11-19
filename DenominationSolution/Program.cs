namespace DenominationSolution;

class Program
{
    static void Main(string[] args)
    {
        List<int> denominations = new List<int> { 10, 50, 100 };

        List<int> amounts = new List<int> { 30, 50, 60, 80, 100, 140, 230, 370, 610, 980 };

        foreach (int amount in amounts)
        {
            var result = DenominationCombinationHelpers.Combinations(amount);

            List<DenominationInfo[]> combinations = result.Select(item => item.ToArray()).ToList();

            DenominationCombinationHelpers.PrintCombinations(amount, combinations);
        }
    }
}

