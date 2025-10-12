using System;
using DotNetEnv;

namespace Assignment4
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load variables from .env
            Env.Load("../.env.local");
            string dbUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            Console.WriteLine($"DB: {dbUrl}");
        }
    }
}