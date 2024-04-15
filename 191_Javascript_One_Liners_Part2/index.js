//1. Remove falsy values from array
//Using this method, you will be able to filter out all falsy values in the array.

const removeFalsy = (arr) => arr.filter(Boolean)

removeFalsy([0, 'a string', '', NaN, true, 5, undefined, 'another string', false])
// ['a string', true, 5, 'another string']





//2. Check if a number is even or odd
//Super simple task that can be solved by using the modulo operator (%).

const isEven = num => num % 2 === 0
isEven(2) // true
isEven(1) // false





//3. Get a random integer between two numbers
//This method is used to get a random integer between two numbers.

const random = (min, max) => Math.floor(Math.random() * (max - min + 1) + min)

random(1, 50) // 25
random(1, 50) // 34





//4. Get average value of arguments

//We can use the reduce method to get the average 
//value of the arguments that we provide in this function.

const average = (...args) => args.reduce((a, b) => a + b) / args.length;

average(1, 2, 3, 4, 5);   // 3








//5. Truncate a number to a fixed decimal point
//Using the Math.pow() method, 
//we can truncate a number to a certain decimal point that we provide in the function.

const round = (n, d) => Number(Math.round(n + "e" + d) + "e-" + d)

round(1.005, 2) //1.01
round(1.555, 2) //1.56







//6. Calculate the number of difference days between two dates
//Sometimes we need to calculate the number of days between two dates,
// a line of code can be done.

const diffDays = (date, otherDate) => Math.ceil(Math.abs(date - otherDate) / (1000 * 60 * 60 * 24));

diffDays(new Date("2021-11-3"), new Date("2022-2-1"))  // 90