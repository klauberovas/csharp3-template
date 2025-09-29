public class Greed
{
    private readonly Random randomGenerator = new();
    public void StartGame()
    {
        while (true)
        {
            Console.WriteLine("Press ENTER to roll the dice. Press ESC to exit.");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            // Roll dice if ENTER is pressed
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                ShowRollResult(RollDice());
            }
            // Exit game if ESC is pressed
            else if (keyInfo.Key == ConsoleKey.Escape)
            {
                Console.WriteLine("Game ended.");
                break;
            }
        }
    }

    // Method to roll 1-6 dice randomly
    private List<int> RollDice()
    {
        int numOfDice = randomGenerator.Next(1, 7);
        var rolledDice = new List<int>();

        for (int i = 0; i < numOfDice; i++)
        {
            rolledDice.Add(randomGenerator.Next(1, 7));
        }

        return rolledDice;
    }

    // Method to calculate  the score based on dice roll
    private int CalculateScore(List<int> rolledDice)
    {
        int score = 0;

        // Count the frequency of each dice value
        var counts = new Dictionary<int, int>();

        foreach (var num in rolledDice)
        {
            counts[num] = counts.GetValueOrDefault(num) + 1;
        }

        // Special combinations
        // 1) Straight (1-6)
        var straightKeys = new int[] { 1, 2, 3, 4, 5, 6 };
        if (rolledDice.Count == 6 && counts.Count == 6 && counts.All(n => n.Value == 1) && straightKeys.All(k => counts.ContainsKey(k)))
        {
            return 1200;
        }

        // 2) Three pairs
        if (rolledDice.Count == 6 && counts.Count == 3 && counts.All(n => n.Value == 2))
        {
            return 800;
        }

        // Base scrore for Triples
        var triplesScores = new Dictionary<int, int> { { 1, 1000 }, { 2, 200 }, { 3, 300 }, { 4, 400 }, { 5, 500 }, { 6, 600 } };

        // 3) Four/Five/Six-of-a-Kind
        foreach (var key in counts.Keys.ToList())
        {
            if (counts[key] == 4)
            {
                score += triplesScores[key] * 2;
                counts[key] -= 4;
            }
            else if (counts[key] == 5)
            {
                score += triplesScores[key] * 4;
                counts[key] -= 5;
            }
            else if (counts[key] == 6)
            {
                score += triplesScores[key] * 8;
                counts[key] -= 6;
            }
        }

        // 4) Triples
        foreach (var key in counts.Keys.ToList())
        {
            if (counts[key] >= 3)
            {
                score += triplesScores[key];
                counts[key] -= 3;
            }
        }

        // 5) Single 1 and 5
        if (counts.ContainsKey(1))
        {
            score += counts[1] * 100;
        }

        if (counts.ContainsKey(5))
        {
            score += counts[5] * 50;
        }

        return score;
    }

    // Display the dice roll and score
    private void ShowRollResult(List<int> rolledDice)
    {
        Console.Write("You rolled: [ ");
        Console.Write(string.Join(", ", rolledDice));
        Console.Write(" ] ");

        int score = CalculateScore(rolledDice);
        Console.WriteLine("| Score: " + score);
    }
}
