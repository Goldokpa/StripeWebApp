using System;

class Generic
{
    static void Main()
    {
        int[] EvenNumbers = new int[6];

        EvenNumbers[0] = 1;
        EvenNumbers[1] = 4;
        EvenNumbers[2] = 5;
        EvenNumbers[3] = 6;
        EvenNumbers[4] = 7;
        EvenNumbers[5] = 8;

        if (EvenNumbers[0] == 1 || EvenNumbers[3] == 6) 
        {
            Console.WriteLine("your number is 1 or 6");
        }
        else
        {
            Console.WriteLine(" your number is wrong");
        }
    }
}