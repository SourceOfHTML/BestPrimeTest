using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;

namespace PrimeTest;
class Program
{
    public static bool IsPrime;
    public static bool IsRunning = true;
    static void Main(string[] args)
    {
        long input;
        Console.WriteLine("Hi! Welcome to Patrick's Probabilistic Primality test of Pain!");
        Console.WriteLine("Give us a number that you want to test the primeness of! (Positive, whole numbers only)");

        while (IsRunning)
        {
            input = Convert.ToInt64(Console.ReadLine());

            if (input % 2 == 0 || input == 0 || input == 1 || input < 0)
            {
                Console.WriteLine("Sorry, invalid number, OR even number.");
            }
            else
            {
                IsPrime = Primality(input);
            }
            
            if (IsPrime)
            {
                Console.WriteLine("Indeed! " + input + " is a prime number!");
            }
            else
            {
                Console.WriteLine("No, " + input + " is not prime.");
            }

            Console.WriteLine("Would you like to try again with another number? (Y/N)");

            ConsoleKeyInfo keyPress = Console.ReadKey();

            if (keyPress.Key == ConsoleKey.Y)
            {
                Console.WriteLine("Alright then! Give us another number!");
            }
            else if(keyPress.Key == ConsoleKey.N)
            {
                Console.WriteLine("Alright then! Leave. Fuck off. Just press any key to do that.");
                IsRunning = false;
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("I'm just gonna pretend you pressed N because I cannot be arsed to nest While Loops to make this shit dynamic.");
                IsRunning = false;
                Console.ReadKey();
            }
        }
    }
    public static bool Primality(long n)
    {
        // This is the Miller-Rabin Primality test.
        List<int> Witnesses = new List<int>();
        /*
         * if n < 1,373,653, it is enough to test a = 2 and 3;
         * if n < 25,326,001, it is enough to test a = 2, 3, and 5;
         * if n < 3,215,031,751, it is enough to test a = 2, 3, 5, and 7;
         * if n < 3,825,123,056,546,413,051, it is enough to test a = 2, 3, 5, 7, 11, 13, 17, 19, and 23;
         * if n > 3,825,123,056,546,413,051, it is enough to test a = 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, and 37;
         * Note that the last statement only works because Longs are 64 bit,
         * If we had more range, then the statement would be false.
         */
        if (n < 1_373_653)
        {
            Witnesses.Add(2);
            Witnesses.Add(3);
        }
        else if (n < 25_326_001)
        {
            Witnesses.Add(2);
            Witnesses.Add(3);
            Witnesses.Add(5);
        }
        else if (n < 3_215_031_751)
        {
            Witnesses.Add(2);
            Witnesses.Add(3);
            Witnesses.Add(5);
            Witnesses.Add(7);
        }
        else if (n < 3_825_123_056_546_413_051)
        {
            Witnesses.Add(2);
            Witnesses.Add(3);
            Witnesses.Add(5);
            Witnesses.Add(7);
            Witnesses.Add(11);
            Witnesses.Add(13);
            Witnesses.Add(17);
            Witnesses.Add(19);
            Witnesses.Add(23);
        }
        else
        {
            Witnesses.Add(2);
            Witnesses.Add(3);
            Witnesses.Add(5);
            Witnesses.Add(7);
            Witnesses.Add(11);
            Witnesses.Add(13);
            Witnesses.Add(17);
            Witnesses.Add(19);
            Witnesses.Add(23);
            Witnesses.Add(29);
            Witnesses.Add(31);
            Witnesses.Add(37);
        }
        return Prep(n, Witnesses);
    }
    public static bool Prep(long n, List<int> wits)
    {
        long defendant = n - 1;
        int testPositive = 0;
        int falseWitness = 0;
        int powersOfTwo = 0;

        while (defendant % 2 == 0)
        {
            defendant /= 2;
            powersOfTwo++;
        }
        foreach (int witness in wits)
        {
            if(witness < n)
            {
                // Witness^Defendant Mod n has to equal -1 or 1 (a^d mod n = +-1)
                long mod = (long)(Math.Pow(witness, defendant) % n);
                for(int i = 1; i < powersOfTwo; i++)
                {
                    mod = (long) Math.Pow(mod, 2) % n;
                }
                if (mod == 1 || mod == n - 1)
                {
                    testPositive++;
                }
            }
            else
            {
                falseWitness++;
            }
        }
        if (testPositive == wits.Count - falseWitness)
        {
            return true;
        }
        else
        {
            return false;
        }

        // Proper info on the method in: https://en.wikipedia.org/wiki/Miller%E2%80%93Rabin_primality_test
    }
}