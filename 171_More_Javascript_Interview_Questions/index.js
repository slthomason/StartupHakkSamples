// How does prototypal inheritance work in JavaScript?


// Parent object constructor function
function Animal(name) {
    this.name = name;
  }
  
  // Adding a method to the prototype of Animal
  Animal.prototype.sayName = function() {
    console.log("My name is " + this.name);
  };
  
  // Child object constructor function
  function Dog(name, breed) {
    // Call the parent constructor with proper context and arguments
    Animal.call(this, name);
    this.breed = breed;
  }
  
  // Inherit properties and methods from Animal
  Dog.prototype = Object.create(Animal.prototype);
  Dog.prototype.constructor = Dog; // Fix the constructor pointer
  
  // Adding a method specific to Dog
  Dog.prototype.bark = function() {
    console.log("Woof!");
  };
  
  // Create instances of Animal and Dog
  var animal = new Animal("Generic Animal");
  var dog = new Dog("Buddy", "Golden Retriever");
  
  // Testing inherited methods
  animal.sayName(); // Output: My name is Generic Animal
  dog.sayName();    // Output: My name is Buddy
  
  // Testing Dog-specific method
  dog.bark();       // Output: Woof!
  

  //What is Functional Programming, and what are the features of JavaScript that make it a candidate as a functional language?

  // Immutability example
const immutableArray = Object.freeze([1, 2, 3]);
// immutableArray.push(4); // Error: Cannot add property 3, object is not extensible
console.log(immutableArray); // Output: [1, 2, 3]

// Pure function example
function add(a, b) {
  return a + b;
}

console.log(add(2, 3)); // Output: 5
console.log(add(2, 3)); // Output: 5 (Same output for same input)



// Why are functions called First-class Objects?

// Assigning a function to a variable
function greet(name) {
    return `Hello, ${name}!`;
  }
  
  let hello = greet;
  console.log(hello("Alice")); // Output: Hello, Alice!
  
  // Passing a function as an argument to another function
  function applyOperation(func, x, y) {
    return func(x, y);
  }
  
  function add(x, y) {
    return x + y;
  }
  
  let result = applyOperation(add, 5, 3);
  console.log(result); // Output: 8
  
  // Returning a function from another function
  function createMultiplier(factor) {
    return function(x) {
      return x * factor;
    };
  }
  
  let double = createMultiplier(2);
  console.log(double(5)); // Output: 10
  
  // Storing functions in data structures
  let functionArray = [add, greet, double];
  functionArray.forEach(function(func) {
    if (typeof func === 'function') {
      console.log(func); // Output: [Function: add], [Function: greet], [Function]
    }
  });
  

  //Implement the Array.prototype.filter method by hand.

  Array.prototype.myFilter = function(callback, thisArg) {
    if (typeof callback !== 'function') {
      throw new TypeError(callback + ' is not a function');
    }
  
    const filteredArray = [];
    const arrayToFilter = this;
  
    for (let i = 0; i < arrayToFilter.length; i++) {
      if (callback.call(thisArg, arrayToFilter[i], i, arrayToFilter)) {
        filteredArray.push(arrayToFilter[i]);
      }
    }
  
    return filteredArray;
  };
  
  // Example usage:
  const numbers = [1, 2, 3, 4, 5];
  const evenNumbers = numbers.myFilter(function(num) {
    return num % 2 === 0;
  });
  
  console.log(evenNumbers); // Output: [2, 4]
  

  //What is ECMAScript? 

  // Define a function to calculate the factorial of a number
function factorial(n) {
    if (n === 0 || n === 1) {
      return 1;
    } else {
      return n * factorial(n - 1);
    }
  }
  
  // Calculate and print the factorial of 5
  console.log(factorial(5)); // Output: 120

  
//What’s the difference between var, let, and const keywords?

//var
function example() {
    var x = 10;
    if (true) {
        var x = 20;
        console.log(x); // Output: 20
    }
    console.log(x); // Output: 20
}
example();

//let
function example() {
    let x = 10;
    if (true) {
        let x = 20;
        console.log(x); // Output: 20
    }
    console.log(x); // Output: 10
}
example();

//const
function example() {
    const x = 10;
    // x = 20; // This would cause an error because we are trying to reassign a constant variable
    console.log(x); // Output: 10
}
example();
