using Microsoft.ClearScript.V8;

namespace ConsoleApp19;
internal class Program
{
    static void Main(string[] args)
    {
        try
        {
            using (var engine = new V8ScriptEngine())
            {
                string javascriptCode = @"
            function add(a, b) {
                if(!isNaN(a) && !isNaN(b)){
                    return a + b;
                }
                return 'Addition is only possible for numbers. Check your input parameters';
                }
            function stringManipulation(inputString){
                return inputString.toLowerCase()+'\n'+ inputString.toUpperCase()+'\n String Length:'+inputString.length;
                }
                    ";

                var inputString = "I am running JS in C# environment using V8 Engine.";
                var stringfnCall = $"stringManipulation('{inputString}')";
                var stringResult = engine.Evaluate(javascriptCode + stringfnCall);
                Console.WriteLine($"stringResult : {stringResult}");


                var mathfnCall1 = " add(2, 33);";
                var mathfnCall12 = " add(2, '3');";
                var mathResult = engine.Evaluate(javascriptCode + mathfnCall1);
                Console.WriteLine($"\nmathResult1 : {mathResult}");
                mathResult = engine.Evaluate(javascriptCode + mathfnCall12);
                Console.WriteLine($"\nmathResult2 : {mathResult}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception:{ex.Message}");
        }

    }
}