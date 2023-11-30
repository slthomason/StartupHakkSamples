//Example 1: Defining and undefining symbols


#define MY_SYMBOL

using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Always printed");

        #if MY_SYMBOL
            Console.WriteLine("Printed if MY_SYMBOL is defined");
        #endif
        
        // output:
        //
        // Always printed
        // Printed if MY_SYMBOL is defined
    }
}

#define MY_SYMBOL
#undef MY_SYMBOL

using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Always printed");

        #if MY_SYMBOL
            Console.WriteLine("Printed if MY_SYMBOL is defined");
        #endif
        
        // output:
        //
        // Always printed
    }
}


#define MY_SYMBOL
#undef MY_SYMBOL
#undef DEBUG

using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Always printed");

        #if MY_SYMBOL
            Console.WriteLine("Printed if MY_SYMBOL is defined");
        #elif (MY_SYMBOL || DEBUG)
            Console.WriteLine("Printed if MY_SYMBOL or DEBUG is defined");
        #else
            Console.WriteLine("Printed if neither MY_SYMBOL or DEBUG is defined");
        #endif
        
        // output:
        //
        // Always printed
        // Printed if neither MY_SYMBOL or DEBUG is defined
    }
}

//Example 2: Trying to use a new API… or falling back to the older version
public class MyClass
{
    static void Main()
    {
#if NET40
        WebClient _client = new WebClient();
#else
        HttpClient _client = new HttpClient();
#endif
    }
    //...
}

//Example 3: Doing conditional compilation in Unity

using UnityEngine;

public class Test : MonoBehaviour {
  void Start () {

    #if UNITY_EDITOR
      Debug.Log("This is a debug that only shows in the editor.");
    #endif

  }          
}