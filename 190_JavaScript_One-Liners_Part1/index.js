//1. Generate a random string

//We can use Math.random to generate a random string, 
//it’s very convenient when we need a unique ID.

const randomString = () => Math.random().toString(36).slice(2)

randomString() // gi1qtdego0b
randomString() // f3qixv40mot
randomString() // eeelv1pm3ja




//2. Escape HTML special characters
//If you know about XSS, one of the solutions is to escape HTML strings.

const escape = (str) => str.replace(/[&<>"']/g, (m) => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[m]))

escape('<div class="medium">Hi Medium.</div>') 
// &lt;div class=&quot;medium&quot;&gt;Hi Medium.&lt;/div&gt


//3. Uppercase the first character of each word in a string
//This method is used to uppercase the first character of each word in a string.

const uppercaseWords = (str) => str.replace(/^(.)|\s+(.)/g, (c) => c.toUpperCase())

uppercaseWords('hello world'); // 'Hello World'

//The more easier is below:

const uppercaseWordsEasy = (str) => str.replace(/^(.)|\s+(.)/g, (c) => c.toUpperCase())


//4. Convert a string to camelCase
const toCamelCase = (str) => str.trim().replace(/[-_\s]+(.)?/g, (_, c) => (c ? c.toUpperCase() : ''));

toCamelCase('background-color'); // backgroundColor
toCamelCase('-webkit-scrollbar-thumb'); // WebkitScrollbarThumb
toCamelCase('_hello_world'); // HelloWorld
toCamelCase('hello_world'); // helloWorld


//5. Remove duplicate values in an array
//It is very necessary to remove the duplicates of the array,
// using “Set” will become very simple.

const removeDuplicates = (arr) => [...new Set(arr)]

console.log(removeDuplicates([1, 2, 2, 3, 3, 4, 4, 5, 5, 6])) 
// [1, 2, 3, 4, 5, 6]




//6. Flatten an array
//We are often tested in interviews, which can be achieved in two ways.

const flat = (arr) =>
    [].concat.apply(
        [],
        arr.map((a) => (Array.isArray(a) ? flat(a) : a))
    )
// Or
const flatt = (arr) => arr.reduce((a, b) => (Array.isArray(b) ? [...a, ...flatt(b)] : [...a, b]), [])

flatt(['cat', ['lion', 'tiger']]) // ['cat', 'lion', 'tiger']


