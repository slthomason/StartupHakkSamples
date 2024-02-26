// Null Vs Undefined

var test1 = null;

console.log(test1);

// null


var test2;

console.log(test2);

// undefined





//Closures


function init() {

    var name = "Mozilla"; // name is a local variable created by init

    function displayName() {

      // displayName() is the inner function, that forms the closure

      console.log(name); // use variable declared in the parent function

    }

    displayName();

  }

  init();



//Event Delegation


const div = document.getElementsByTagName("div")[0]


div.addEventListener("click", (event) => {

  if(event.target.tagName === 'BUTTON') {

    console.log("button was clicked")

  }

})



//Function.prototype.bind


const module = {

    x: 42,

    getX: function () {

      return this.x;

    },

  };

  

  const unboundGetX = module.getX;

  console.log(unboundGetX()); // The function gets invoked at the global scope

  // Expected output: undefined

  

  const boundGetX = unboundGetX.bind(module);

  console.log(boundGetX());

  // Expected output: 42



//Higher-Order Functions


 //Assign a function to a variable originalFunc

const originalFunc = (num) => { return num + 2 };


//Re-assign the function to a new variable newFunc

const newFunc = originalFunc;


//Access the function's name property

newFunc.name; //'originalFunc'


//Return the function's body as a string

newFunc.toString(); //'(num) => { return num + 2 }'


//Add our own isMathFunction property to the function

newFunc.isMathFunction = true;


//Pass the function as an argument

const functionNameLength = (func) => { return func.name.length }; 

functionNameLength(originalFunc); //12


//Return the function

const returnFunc = () => { return newFunc };

returnFunc(); //[Function: originalFunc]




//Array.prototype.map()


const array1 = [1, 4, 9, 16];


// Pass a function to map

const map1 = array1.map((x) => x * 2);


console.log(map1);

// Expected output: Array [2, 8, 18, 32]



//The arguments object


function func1(a, b, c) {

    console.log(arguments[0]);

    // Expected output: 1

  

    console.log(arguments[1]);

    // Expected output: 2

  

    console.log(arguments[2]);

    // Expected output: 3

  }

  

func1(1, 2, 3);



//How to create an object without a prototype?


var dictionary = Object.create(null, {

    destructor: { value: "A destructive element" }

});


function getDefinition(word) {

    return dictionary[word];

}


// Outputs: "A destructive element"

console.log(getDefinition("destructor"));


// Outputs: undefined

console.log(getDefinition("constructor"));



//What are Arrow functions?


const materials = ['Hydrogen', 'Helium', 'Lithium', 'Beryllium'];


console.log(materials.map((material) => material.length));

// Expected output: Array [8, 6, 7, 9]


//How to check if a certain property exists in an object?


const user = {

    name: 'John Doe',

    age: 30,

    email: 'johndoe@example.com'

  };

  console.log(user.hasOwnProperty('name')); // Returns true

  console.log(user.hasOwnProperty('address')); // Returns false



//Wrapper Object


var s = "hello world!";                             // A string

var word = s.substring(s.indexOf(" ")+1, s.length); // Use string properties


//OR


var s = "test";         // Start with a string value.

s.len = 4;              // Set a property on it.

var t = s.len;          // Now query the ...

