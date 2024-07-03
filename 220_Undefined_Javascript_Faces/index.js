// The Simple Cases

// null: A real value with a distinct type
let nullableValue = null;

// undefined as missing: A generated value when a field isn’t known
let optionalValue;

// Checking for missing or undefined value
if (optionalValue === undefined) {
    console.log("optionalValue is undefined");
}

// undefined as value: A real value with a distinct type assigned to a variable
let explicitlyUndefinedValue = undefined;
console.log(explicitlyUndefinedValue); // undefined

// The Interpretative Cases

// Falsy: When a real value looks undefined
let falsyValue = 0;
if (!falsyValue) {
    console.log("falsyValue is treated as undefined or false");
}

// ReferenceError: When a symbol isn’t known
try {
    console.log(nonExistentVariable);
} catch (e) {
    console.log(e); // ReferenceError
}

// <empty slot> in sparse arrays
let sparseArray = [1, , 3];
console.log(sparseArray[1]); // undefined

// -1 as undefined: Returned from some functions
let array = [1, 2, 3];
console.log(array.indexOf(4)); // -1

// undefined replacement: Custom undefined value
function customUndefined() {
    let undefined = 123;
    console.log(undefined); // 123
}
customUndefined();

// The TypeScript Cases

// function arguments: unspecified arguments and explicit undefined
function func(a: number, b?: number) {
    console.log(a, b);
}
func(1); // 1, undefined
func(1, undefined); // 1, undefined

// void: a special TypeScript version
function noReturn(): void {
    console.log("This function returns void");
}
