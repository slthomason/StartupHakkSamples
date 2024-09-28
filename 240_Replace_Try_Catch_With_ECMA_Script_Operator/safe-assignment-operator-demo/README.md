# Demo of ECMAScript Safe Assignment Operator 

> [!WARNING]  
> This proposal is actively under development.

Credit goes to : [arthurfiorette/proposal-safe-assignment-operator: Draft for ECMAScript Error Safe Assignment Operator (github.com)](https://github.com/arthurfiorette/proposal-safe-assignment-operator)

This is demo of proposal introduces a new operator, `?=`  _(Safe Assignment)_, which simplifies error handling by transforming the result of a function into a tuple. If the function throws an error, the operator returns `[error, null]`; if the function executes successfully, it returns `[null, result]`. This operator is compatible with promises, async functions, and any value that implements the [`Symbol.result`]method.

## Example: Using the Safe Assignment Operator

The following code snippet illustrates how to utilize the `?=` operator for simplified error handling when fetching data from an API.

```javascript
// Note: The `?=` operator itself cannot be polyfilled directly.
// When targeting older JavaScript environments, a post-processor should be used 
// to transform the `?=` operator into the corresponding [Symbol.result] calls.

async function fetchTodo() {
    // Bind enhancedFetch to the window context
    const fetch = enhancedFetch.bind(window);

    // Fetch data from a placeholder API using enhanced error handling
    const [networkError, response] = await fetch[Symbol.result]("https://jsonplaceholder.typicode.com/todos/1");

    // Check for network errors
    if (networkError) {
        return console.error('Network error :', networkError);
    }

    //We have to do the scope bind
    const json = response.json.bind(response);

    //const [jsonError, data] ?= await json();
    const [jsonError, data] = await json[Symbol.result]();

    if (jsonError) {
        // console.log("JSON parse failed", jsonError);
        throw new Error("User JSON parse failed");
    }

    // Optionally, you can log the data or process it further here
    return data;
}

(async () => {
    //const [error, user] ?= await fetchUser(11);
    const [error, user] = await fetchTodo[Symbol.result](11);

    if (error) {
        console.log("Fetching Todos failed ", error);
    } else {
        console.log("Fetched Todos", user);
    }
})();
```

### Polyfilling
This still as proposal and not introduce, it can be polyfilled using the code provided at  [`polyfill.js`]

However, the  `?=`  operator itself cannot be polyfilled directly. When targeting older JavaScript environments, a post-processor should be used to transform the  `?=`  operator into the corresponding  `[Symbol.result]`  calls.

This thing will handle by compiler such as babel.

```javascript
const [error, data] ?= await asyncAction(arg1, arg2)
// should become
const [error, data] = await asyncAction[Symbol.result](arg1, arg2)
```
<br/>

```javascript
const [error, data] ?= action()
// should become
const [error, data] = action[Symbol.result]()
```
<br/>

```javascript
const [error, data] ?= obj
// should become
const [error, data] = obj[Symbol.result]()
```

### Using  `?=`  with Functions and Objects Without  `Symbol.result`

If the function or object does not implement a `Symbol.result` method, the `?=` operator should throw a `TypeError`.