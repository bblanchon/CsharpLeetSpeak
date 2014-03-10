namespace ConsoleApplication
{
    using System;
    using System.Reflection;
    using LeetSpeak;

    class Program
    {
        static void Main(string[] args)
        {
            LeetSpeakTranslator.LeetifyAssembly(Assembly.GetExecutingAssembly());

            Console.WriteLine("Hello World!");
        }
    }
}
