public class FizzBuzz
{
    public void CountTo(int lastNum)
    {
        for (int actualNum = 1; actualNum <= lastNum; actualNum++)
        {
            if (actualNum % 3 == 0 && actualNum % 5 == 0)
            {
                Console.WriteLine("FizzBuzz");
            }
            else if (actualNum % 3 == 0)
            {
                Console.WriteLine("Fizz");
            }
            else if (actualNum % 5 == 0)
            {
                Console.WriteLine("Buzz");
            }
            else
            {
                Console.WriteLine(actualNum);
            }
        }
    }
}
