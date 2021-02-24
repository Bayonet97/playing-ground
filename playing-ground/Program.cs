using System;

namespace playing_ground
{
    class Program
    {
        static void Main(string[] args)
        {
            int year = 2020;

            int newYear = year++;
            int anotherYear = ++year;

            Console.WriteLine($"Year++: {newYear}, ++Year: {anotherYear}");
        }
    }
}
