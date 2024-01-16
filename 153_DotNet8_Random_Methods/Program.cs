//Random.Shared.GetItems Method

//Previous .Net Version
var selectedValues = inputArray.OrderBy(_ => random.Next()).Take(amountToBeSelected).ToArray();

//.NET 8
var selectedValues = Random.Shared.GetItems(inputArray, amountToBeSelected);

var names = new string[] { "Bob", "Ana", "Jessica", "Mike", "Rick" };
Console.WriteLine($"Names: {JsonSerializer.Serialize(names)}");

var randomlySelectedNames = Random.Shared.GetItems(names, 2);
Console.WriteLine($"Selected names: {JsonSerializer.Serialize(randomlySelectedNames)}{Environment.NewLine}");


//Random.Shared.Shuffle Method

//Previous .Net Version
var random = new Random();
var shuffledArray = numbers.OrderBy(_ => random.Next()).ToArray();

int inputArrayLength = inputArray.Length;

for (int i = inputArrayLength - 1; i > 0; i--)
{
    int j = random.Next(i + 1);
    var temp = inputArray[i];
    inputArray[i] = inputArray[j];
    inputArray[j] = temp;
}

//.Net 8

Random.Shared.Shuffle(arrayInput);

var numbers = new int[] { 1, 2, 3, 4, 5, };
Console.WriteLine($"Numbers before shuffling: {JsonSerializer.Serialize(numbers)}");

Random.Shared.Shuffle(numbers);
Console.WriteLine($"Numbers after shuffling: " +
    $"{JsonSerializer.Serialize(numbers)}{Environment.NewLine}");


//RandomNumberGenerator

var randomString = RandomNumberGenerator.GetString(someString, stringLength);

var randomString = RandomNumberGenerator.GetString("HelloDotNet8", 10);
Console.WriteLine($"Random number generator: {JsonSerializer.Serialize(randomString)}");
