//What Are Preprocessor Directives in C#?

#define DEBUG
// The DEBUG symbol is defined for the preprocessor
#if DEBUG
    Console.WriteLine("Debugging is on!"); // This code will be compiled for debugging
#else
    Console.WriteLine("Debugging is off!"); // This code will be compiled for the release
#endif


#define TESTING
...
#if TESTING
    // This problematic code won't get compiled while testing
#else
   // Proper production code goes here
#endif


#define DEBUG
// The DEBUG symbol is defined
#if DEBUG
    Console.WriteLine("Debug mode is on!");
#else
   Console.WriteLine("Release mode is on!");
#endif

//Custom Preprocessor Directives in .NET

#define MY_SYMBOL
// ... and later in the code
#if MY_SYMBOL
    Console.WriteLine("The symbol MY_SYMBOL is defined.");
#endif

//Understanding C# Preprocessor Directives Build Configuration

#if DEBUG
    // This code will only be compiled and executed in Debug mode
    Console.WriteLine("Debug mode is active!");
#endif

#if SPECIAL
    // Code specific to the special build
    Console.WriteLine("Special build is active!");
#endif

//The Usefulness of C# Debug Preprocessor

Debug.WriteLine("Hello, debugger!");
// Writes 'Hello, debugger!' to the output window of the debugger

//C# Preprocessor Directives for Conditional Compiling

#if DEBUG
    //Code here will only execute in Debug mode
#endif

//The Power of C# Ifdef Debug

#ifdef DEBUG
    // This code will only compile in Debug mode
#endif


//Preprocessor Directives in C#: Real-World Examples

#if TODO
    // TODO: Improve this method
    public void NeedsImprovement()
    {
        // ...
    }
#endif

//Accessing Environment Variables using C# Preprocessor Directives

const string ENV_VAR = "MY_ENV_VAR";
string value = Environment.GetEnvironmentVariable(ENV_VAR);