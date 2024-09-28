// Import the enhancedFetch function from the fetch library
import { enhancedFetch } from "../lib/fetch.js";
// Import the polyfill for enhanced error handling
import "../lib/polyfill.js";

async function fetchTodo(todoId) {
    // Bind enhancedFetch to the window context
    const fetch = enhancedFetch.bind(window);

    // Fetch data from a placeholder API using enhanced error handling
    const [networkError, response] = await fetch[Symbol.result](`https://jsonplaceholder.typicode.com/todos/${todoId}`);

    // Check for network errors
    if (networkError) {
        throw new Error(networkError);
    }

    // We have to do the scope bind
    const json = response.json.bind(response);

    const [jsonError, data] = await json[Symbol.result]();

    if (jsonError) {
        // console.log("JSON parse failed", jsonError);
        throw new Error("User JSON parse failed");
    }

    // Optionally, you can log the data or process it further here
    return data;
}


// This immediately invoked async function fetches a user with a valid ID
// If the fetch is successful, it logs the user data to the console.
// If there is an error (e.g., user not found), it logs the error message.
(async () => {
    const [error, user] = await fetchTodo[Symbol.result](2);

    if (error) {
        console.log("Fetching user failed 1", error);
    } else {
        console.log("Fetched User", user);
    }
})();

// This immediately invoked async function attempts to fetch a user with an invalid ID
// The function expects to handle an error, such as a 404 Not Found response.
// If an error occurs, it logs the error message; otherwise, it logs the user data.
(async () => {
    const [error, user] = await fetchTodo[Symbol.result]("wrong id");

    if (error) {
        console.log("Fetching user failed", error);
    } else {
        console.log("Fetched User", user);
    }
})();