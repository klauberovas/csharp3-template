using System;
public class Greed
{
    public void Start()
    {
        ShowResultToConsole(RollDice());
    }

    private List<int> RollDice()
    {
        var rolledDice = new List<int>();
        var randomGenerator = new Random();

        for (int i = 0; i < 5; i++)
        {
            rolledDice.Add(randomGenerator.Next(1, 7));
        }

        return rolledDice;
    }

    private int CalculateScore(List<int> rolledDice)
    {
        int score = 0;
        var counts = new Dictionary<int, int>();

        // četnost hodů
        foreach (var num in rolledDice)
        {
            counts[num] = counts.GetValueOrDefault(num) + 1;
        }

        // Trojice
        var triplesScores = new Dictionary<int, int> { { 1, 1000 }, { 2, 200 }, { 3, 300 }, { 4, 400 }, { 5, 500 }, { 6, 600 } };

        foreach (var key in counts.Keys.ToList())
        {
            if (counts[key] >= 3)
            {
                score += triplesScores[key];
                counts[key] -= 3;
            }
        }

        // Body za 1 a 5
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

    private void ShowResultToConsole(List<int> rolledDice)
    {
        Console.Write("Tvoje hody kostkami: [ ");
        Console.Write(string.Join(", ", rolledDice));
        Console.Write(" ] ");

        int score = CalculateScore(rolledDice);
        Console.WriteLine("Skore: " + score);
    }
}
