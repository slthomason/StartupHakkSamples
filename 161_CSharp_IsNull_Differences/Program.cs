//string.IsNullOrEmpty

string myString = "Hello, World!";
bool isNullOrEmpty = string.IsNullOrEmpty(myString);


//.IsNull (Custom Extension Method)
public static bool IsNull(this string value)
{
    return value == null;
}
//IsNullOrEmptyWhitespace
public static bool IsNullOrEmptyWhitespace(this string value)
{
    return string.IsNullOrWhiteSpace(value);
}


//Practical Examples:


// Using string.IsNullOrEmpty
bool isValid = !string.IsNullOrEmpty(userInput);

// Using .IsNull
bool isValid = userInput.IsNull();

// Using IsNullOrEmptyWhitespace
bool isValid = !userInput.IsNullOrEmptyWhitespace();