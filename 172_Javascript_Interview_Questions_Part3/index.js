//What are the new features in ES6 or ECMAScript 2015?

// ES5
var multiply = function(x, y) {
    return x * y;
  };
  
  // ES6
  const multiply = (x, y) => x * y;


// What are Classes?
  class Car {
    constructor(make, model, year) {
      this.make = make;
      this.model = model;
      this.year = year;
      this.odometerReading = 0;
    }
  
    getDescriptiveName() {
      return `${this.year} ${this.make} ${this.model}`;
    }
  
    readOdometer() {
      console.log(`This car has ${this.odometerReading} miles on it.`);
    }
  
    updateOdometer(mileage) {
      if (mileage >= this.odometerReading) {
        this.odometerReading = mileage;
      } else {
        console.log("You can't roll back an odometer!");
      }
    }
  
    incrementOdometer(miles) {
      this.odometerReading += miles;
    }
  }
  
  // Creating an instance of the Car class
  const myCar = new Car('Audi', 'A4', 2019);
  
  // Accessing attributes and calling methods of the instance
  console.log(myCar.getDescriptiveName());
  myCar.readOdometer();
  
  // Modifying attributes using methods
  myCar.updateOdometer(100);
  myCar.readOdometer();
  
  // Incrementing attribute value using a method
  myCar.incrementOdometer(50);
  myCar.readOdometer();
  
  //What are Template Literals?

  // Basic example
const name = 'John';
const age = 30;
const greeting = `Hello, my name is ${name} and I am ${age} years old.`;

console.log(greeting);
// Output: Hello, my name is John and I am 30 years old.

// Multiline example
const multiLineString = `
    This is a multiline
    string example.
    It preserves the line breaks
    and indentation.
`;

console.log(multiLineString);
/* Output:
    This is a multiline
    string example.
    It preserves the line breaks
    and indentation.
*/


//What is Object Destructuring?

const student = {
    name: 'Alice',
    age: 20,
    scores: {
      math: 90,
      science: 85
    }
  };
  
  // Destructuring nested object
  const { name, age, scores: { math, science } } = student;
  
  console.log(name);    // Output: Alice
  console.log(age);     // Output: 20
  console.log(math);    // Output: 90
  console.log(science); // Output: 85

  
  //What are ES6 Modules?

  // math.js
export const add = (a, b) => {
    return a + b;
  };
  
  export const subtract = (a, b) => {
    return a - b;
  };

  
// app.js
import { add, subtract } from './math.js';

console.log(add(5, 3)); // Output: 8
console.log(subtract(10, 4)); // Output: 6



//What are set objects?

// Creating a new Set
let mySet = new Set();

// Adding values to the Set
mySet.add(1);
mySet.add(2);
mySet.add(3);
mySet.add(1); // This value will not be added as it's a duplicate

console.log(mySet); // Output: Set(3) { 1, 2, 3 }

// Checking if a value exists in the Set
console.log(mySet.has(2)); // Output: true
console.log(mySet.has(4)); // Output: false

// Deleting a value from the Set
mySet.delete(2);
console.log(mySet); // Output: Set(2) { 1, 3 }

// Iterating through the Set
for (let item of mySet) {
  console.log(item);
}
// Output:
// 1
// 3

// Getting the size of the Set
console.log(mySet.size); // Output: 2

// Creating a Set from an array
let anotherSet = new Set([1, 2, 3, 4, 4, 5]);
console.log(anotherSet); // Output: Set(5) { 1, 2, 3, 4, 5 }
