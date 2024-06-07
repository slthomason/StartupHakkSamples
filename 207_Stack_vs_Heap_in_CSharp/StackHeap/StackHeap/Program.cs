using System;

namespace StackHeap
{
    class Program
    {
        static void Main(string[] args)
        {
            //Stack
            int x = 10;
            int y = 20;
            int result = Add(x, y);
            Console.WriteLine($"Result of adding {x} and {y} is: {result}");


            //Heap

            Person person1 = new Person();
            person1.Name = "John";
            person1.Age = 30;

            Person person2 = new Person();
            person2.Name = "Jane";
            person2.Age = 25;

            Console.WriteLine($"{person1.Name} is {person1.Age} years old.");
            Console.WriteLine($"{person2.Name} is {person2.Age} years old.");
        }

        static int Add(int a, int b)
        {
            int sum = a + b;
            return sum;
        }
    }

    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
