//What is a Callback function?

// Example of a function that takes a callback
function doSomethingAsync(callback) {
    setTimeout(function() {
        console.log("Async operation completed");
        callback(); // Calling the callback function after the async operation completes
    }, 2000); // Simulating an asynchronous operation that takes 2 seconds
}

// Callback function to be passed to doSomethingAsync
function callbackFunction() {
    console.log("Callback function executed");
}

// Calling the function with the callback
console.log("Starting async operation");
doSomethingAsync(callbackFunction);
console.log("Async operation initiated");


//What are Promises?

// Example function that returns a promise
function fetchData() {
    return new Promise((resolve, reject) => {
      // Simulating asynchronous operation (e.g., fetching data from an API)
      setTimeout(() => {
        const data = { message: "Data fetched successfully" };
        // Simulating successful completion
        resolve(data);
        // Simulating an error
        // reject(new Error("Failed to fetch data"));
      }, 2000);
    });
  }
  
  // Using the fetchData function with promises
  fetchData()
    .then((result) => {
      console.log("Success:", result);
    })
    .catch((error) => {
      console.error("Error:", error);
    });

//What is async/await, and how does it work? 

// Async function declaration
async function fetchData() {
    // Simulating an asynchronous operation (e.g., fetching data from an API)
    return new Promise((resolve, reject) => {
        setTimeout(() => {
            resolve('Data fetched successfully!');
        }, 2000); // Simulating a delay of 2 seconds
    });
}

// Using async/await
async function processData() {
    try {
        console.log('Fetching data...');
        // Await the fetchData function to complete and get the result
        const data = await fetchData();
        console.log('Data received:', data);
        // Code below this line will only execute after fetchData promise is resolved
    } catch (error) {
        console.error('Error fetching data:', error);
    }
}

// Calling the async function
processData();


//What’s the difference between the Spread operator and the Rest operator?

// Spread Operator (...)

const arr1 = [1, 2, 3];
const arr2 = [4, 5, 6];

const combinedArray = [...arr1, ...arr2]; // Combines both arrays
console.log(combinedArray); // Output: [1, 2, 3, 4, 5, 6]


//Rest Parameter (...)

function sum(...numbers) {
    return numbers.reduce((acc, curr) => acc + curr, 0);
}

console.log(sum(1, 2, 3, 4, 5)); // Output: 15


//What are Default Parameters?

// Function declaration with default parameters
function greet(name = 'Guest', message = 'Hello') {
    console.log(`${message}, ${name}!`);
}

// Calling the function without providing arguments
greet(); // Output: Hello, Guest!

// Calling the function with only one argument
greet('John'); // Output: Hello, John!

// Calling the function with both arguments
greet('Alice', 'Hi'); // Output: Hi, Alice!

//What is the difference between Implicit and Explicit Coercion?

//Implicit Coercion:

var x = 10;
var y = "5";
var result = x + y; // JavaScript implicitly coerces 'y' to a number and performs addition
console.log(result); // Output: 105 (a string)


//Explicit Coercion:

var x = "10";
var y = 5;
var result = Number(x) + y; // Explicitly converting 'x' to a number
console.log(result); // Output: 15 (a number)
