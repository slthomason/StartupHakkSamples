//1. Basic Structure
using System;

public class HelloWorld
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}

//2. Data Types

//Value Types:
int age = 25;
char grade = 'A';
float temperature = 98.6f;

//Reference Types:
//a) string
string name = "John Doe";

//b) class
public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

Person person = new Person();
person.FirstName = "John";
person.LastName = "Doe";

//c) array
int[] numbers = new int[] { 1, 2, 3, 4, 5 };

//3. Variables
int age = 30; // integer variable
string name = "John"; // string variable
double PI = 3.14159; // double for floating-point numbers
bool isLoggedIn = true; // boolean variable

//Use ‘var’ for type inference:
var number = 5; // compiler infers type as int
var message = "This is a message"; // compiler infers type as string

//4. Constants
const double GRAVITY = 9.81; // constant for gravitational acceleration
const string COMPANY_NAME = "MyCompany"; // constant company name

//5. Conditional Statements
int age = 20;

if (age >= 18)
{
    Console.WriteLine("You are eligible to vote.");
}
else
{
    Console.WriteLine("You are not eligible to vote.");
}

switch (variable) { /*...*/ } // Switch statement


//6. Loops

for (int i = 1; i <= 5; i++)
{
    Console.WriteLine(i);
}

foreach (var item in collection) { /*...*/ } // Foreach loop

while (condition) { /*...*/ } // While loop

do { /*...*/ } while (condition); // Do-while loop

//7. Arrays
string[] names = new string[3] { "Alice", "Bob", "Charlie" };
Console.WriteLine(names[1]); // Output: Bob (accessing element at index 1)

//8. Lists
List<int> numbers = new List<int>();
numbers.Add(1);
numbers.Add(2);
numbers.Add(3);

foreach (var number in numbers)
{
    Console.WriteLine(number);
}