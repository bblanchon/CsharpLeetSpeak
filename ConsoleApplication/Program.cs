namespace ConsoleApplication
{
    using System;
    using System.Reflection;
    using LeetSpeak;

    class Program
    {
        static void Main()
        {
            LeetSpeakTranslator.LeetifyAssembly(Assembly.GetExecutingAssembly());

            Console.WriteLine("Hello World!");
        }
    }
}
