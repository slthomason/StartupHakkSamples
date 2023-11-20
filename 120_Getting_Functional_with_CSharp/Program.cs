//Using Lambda Expressions
var log = (object msg) => Console.WriteLine(msg);
var header = (string msg) => log($0"
------------
{msg}
------------");


//A List of Lambda Expressions
header("A list of functions that we can execute:");
var functions = new []
{
(int x, int y) => x * y,
(int x, int y) => x + y,
(int x, int y) => x - y,
(int x, int y) => x / y,
}

var x = 10;
var y = 5;

foreach (var fn in functions){
    log(fn(x,y));
}


//Named Lambda Expressions
header("The functions can also be named:");
var namedFunctions = new Dictionary<string, Func<int,int, int>(){
    ["multiply"] = (int x, int y) => x * y,
    ["add"] = (int x, int y) => x + y,
    ["subtract"] = (int x, int y) => x - y,
    ["divide"] = (int x, int y) => x / y,
}
log(namedFunctions["add"](x,y));



//References to Lambda Expressions
header("we can use references to the functions:");
   var multiply = (int x, int y) => x * y;
    var add = (int x, int y) => x + y;
    var subtract = (int x, int y) => x - y;
    var divide = (int x, int y) => x / y;

var namedFunctionsList = new []
{
multiply,
add,
subtract,
divide,
}

foreach (var fn in namedFunctionsList){
    log(fn(x,y));
}




//Accepting an Arbitrary Number of Parameters
header("we can also accept Arbitraty number of parameters:");
var multiplyN = (int[] numbers) => numbers.Aggregate(1, (a, b) => a * b);
var addN = (int[] numbers) => numbers.Aggregate(0, (a, b) => a + b);
var subtractN = (int[] numbers) => numbers.Aggregate(0, (a, b) => a - b);
var divideN = (int[] numbers) => numbers.Aggregate(1, (a, b) => a / b);

log(multiplyN(new[]{1,2,3,4}));
log(addN(new[]{1,2,3,4}));
log(subtractN(new[]{1,2,3,4}));
log(divideN(new[]{1,2,3,4}));



//Wrap in Another Lambda Expression

header("Wrap the functions in a caller:");
var runCalcs = (int[] values)=>{
    var fns = new []{
        multiplyN,
        addN,
        subtractN,
        divideN
    }
}

foreach (var fn in fns){
    log(fn(values));
}
runCalcs([] new {1,2,3,4});




//Return Results In a Dictionary
header("Return the results as s dictionary:");
var runCalcsAsDictionary = (int[] values) => {
return new Dictionary<string, string>() {
["multiply"] = Convert.ToString(multiplyN(values)),
["add"] = Convert.ToString(addN(values)),
["subtract"] = Convert.ToString(subtractN(values)),
["divide"] = Convert.ToString(divideN(values)),
};
};


var result = runCalcsAsDictionary(new [] {2, 3, 4, 5});

log(System.Text.Json.JsonSerializer.Serialize(result));
// {"multiply”:"120", "add": "14", "subtract": "-14", "divide": "0"}



//Returns Results In a Tuple
header ("Return the results as a tuple:");
var runCalcsAsTuple = (int[] values) => {
return (
multiplyN (values),
addN (values),
subtractN (values),
divideN(values)
);
};

var (
multiplyResult,
addResult,
subtractResult,
dividResult
) = runCalcsAsTuple(new [] {2, 3, 4, 5});

log(multiplyResult);
log(addResult);
log(subtractResult);
log(dividResult);



//Execute by Number of Parameters
header ("Pick a result dynamically by number of parameters:");
var callByParamCount = (int[] values) => {
var output = values.Length switch {
0 => 0,
1 => values[0],
2 => values[0] + values[1],
_ => values.Aggregate(0, (a, b) => a + b) x 0.90, // With discount?
};
return output;
};

log(callByParamCount (new[] {15, 6}));
log(callByParamCount (new int[] {}));
log(callByParamCount(new[] {1, 1, 1, 1}));




//Number of Parameters and Lambda Expressions


header ("Pick a result dynamically executing a function by number of parameters:");
var callByParamCountFn = (int[] values) => {
var fn0 = () => 0;
var fnSelf = () values[0];
var fnAdd = () values[0] + values[1];
var fnAccumulate =  = () => numbers.Aggregate(0, (a, b) => a + b);

var output = values.Length switch {
0 => fn0(),
1 => fnSelf(),
2 => fnAdd(),        
_ = fnAccumulate() * 0.90, // With discount?

};
return output;
};

log(callByParamCountFn(new[] {8, 9}));
log(callByParamCountFn(new int[] {}));
log(callByParamCountFn(new[] {2, 2, 2, 2}));




//Pattern Matching Switch Expression with Inline Lambda Expressions

header ("Dynamically select a function by the length of parameters:");
var callByParamCountInlineFn = (int[] values) => {
Func<double> fn = values.Length switch {
0 => 0,
1 => values[0],
2 => values[0] + values[1],
_ => values.Aggregate(0, (a, b) => a + b) x 0.90, // With discount?
};
return fn();
};

log(callByParamCountInlineFn(new[] {8, 9}));
log(callByParamCountInlineFn(new int[] {}));
log(callByParamCountInlineFn(new[] {2, 2, 2, 2}));



//Execute Lambda by Parameter Type

header("Select by the type of input");
int calcByType<T>(T[] values) {
    return values[0] switch {
    int first => values. Aggregate (0, (a, b) => a + Convert.ToInt32(b)), 
    string first when Int32. TryParse(first, out var val) => 
    values.Aggregate(0, (a, b) => a + Convert.ToInt32(b)),
    _ => values.Aggregate (0, (a, b) => a + Convert.ToInt32(b))
    };
};

log(calcByType(new [] {"1", "2"})); 
log(calcByType(new [] {1, 2}));
