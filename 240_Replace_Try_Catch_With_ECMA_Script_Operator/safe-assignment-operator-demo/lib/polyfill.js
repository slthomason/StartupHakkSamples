// Note: The `?=` operator itself cannot be polyfilled directly.
// When targeting older JavaScript environments, a post-processor should be used 
// to transform the `?=` operator into the corresponding [Symbol.result] calls.

// Create a unique symbol to represent the result
Symbol.result = Symbol("result");

// Extend Function prototype to handle result destructuring
Function.prototype[Symbol.result] = function (...args) {
    try {
        // Call the function with the provided arguments
        const result = this.apply(this, args);

        // Handle recursive cases, where the result might be a function
        // or an object that implements the Symbol.result
        if (result && typeof result === "object" && Symbol.result in result) {
            return result[Symbol.result](); // Call the result's Symbol.result method
        }

        // Return a tuple: [error, result] - no error occurs
        return [null, result];
    } catch (error) {
        // If an error occurs, return it; provide a default error if undefined
        return [error || new Error("Thrown error is falsy")];
    }
};

// Extend Promise prototype to handle result destructuring for promises
Promise.prototype[Symbol.result] = async function () {
    try {
        // Await the promise to get the result
        const result = await this;
        return [null, result]; // Return a tuple: [error, result] - no error occurs
    } catch (error) {
        // If an error occurs, return it; provide a default error if undefined
        return [error || new Error("Thrown error is falsy")];
    }
};
