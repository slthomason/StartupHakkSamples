//What is the significance of JavaScript in web development?

// Example 1: Displaying an alert
alert("Hello, World!");

// Example 2: Handling user input
let name = prompt("What is your name?");
console.log("Hello, " + name);

// Example 3: Adding interactivity
document.getElementById("myButton").addEventListener("click", function() {
    alert("Button clicked!");
});

// Example 4: Making an AJAX request
fetch('https://api.example.com/data')
  .then(response => response.json())
  .then(data => console.log(data))
  .catch(error => console.error('Error fetching data: ' + error));



//Implement the Array.prototype.reduce method by hand. 

// Implementing reduce function for arrays
Array.prototype.customReduce = function(callback, initialValue) {
    // Check if the array is empty and there is no initial value
    if (this.length === 0 && initialValue === undefined) {
        throw new TypeError('Reduce of empty array with no initial value');
    }

    // Initialize accumulator with the initial value or the first element of the array
    let accumulator = initialValue !== undefined ? initialValue : this[0];

    // Start iteration from the initial value index or 1 if initial value not provided
    let startIndex = initialValue !== undefined ? 0 : 1;

    // Iterate through each element of the array
    for (let i = startIndex; i < this.length; i++) {
        accumulator = callback(accumulator, this[i], i, this);
    }

    // Return the final accumulator value
    return accumulator;
};

// Example usage:
const numbers = [1, 2, 3, 4, 5];
const sum = numbers.customReduce((accumulator, currentValue) => accumulator + currentValue, 0);
console.log(sum); // Output: 15 (1 + 2 + 3 + 4 + 5 = 15)


//What is NaN? and How do check if a value is NaN? 

// Example 1: Using isNaN() function
let num1 = 10;
let num2 = 'hello';

console.log(isNaN(num1)); // Output: false
console.log(isNaN(num2)); // Output: true

// Example 2: Using Number.isNaN() method (ES6+)
console.log(Number.isNaN(num1)); // Output: false
console.log(Number.isNaN(num2)); // Output: false (more strict compared to global isNaN)

// Example 3: Using comparison with itself (NaN is the only value in JavaScript that is not equal to itself)
console.log(num1 !== num1); // Output: false
console.log(num2 !== num2); // Output: true


//How to check if a value is an Array?

// Define a value
let value = [1, 2, 3];

// Check if the value is an array
if (Array.isArray(value)) {
    console.log('The value is an array.');
} else {
    console.log('The value is not an array.');
}


//How to check if a number is even without using the % or modulo operator? 

function isEven(number) {
    // Check if the last bit is 0
    return (number & 1) === 0;
}

// Test cases
console.log(isEven(4));  // Output: true
console.log(isEven(7));  // Output: false
console.log(isEven(-2)); // Output: true
