import { enhancedFetch } from "../lib/fetch.js";

// Function to fetch a Todo item by its ID
async function fetchTodo(todoId) {
    // Binding the enhanced fetch to the current window scope
    const fetch = enhancedFetch.bind(window);
    try {
        // Fetching the Todo item from the placeholder API using the provided todoId
        const response = await fetch(`https://jsonplaceholder.typicode.com/todos/${todoId}`);

        // Check if the response status is in the range of 200-299 (indicates success)
        if (!response.ok) {
            // If the response is not ok, throw an error with the status code
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        // Parse and return the JSON data from the response
        const data = await response.json();
        return data;
    } catch (error) {
        // Log the error details to the console for debugging
        console.error("Error fetching Todo:", error);
        // Rethrow the error to allow further handling by the caller
        throw error;
    }
}

// This immediately invoked async function fetches a todo with a valid ID
// If the fetch is successful, it logs the user data to the console.
// If there is an error (e.g., todo item not found), it logs the error message.
(async () => {
    try {
        // Attempt to fetch the Todo item with ID 1
        const todoItem = await fetchTodo(1);
        console.log("Fetched Todo item: ", todoItem); // Log the fetched Todo item
    } catch (error) {
        // If an error occurs, log a message indicating failure
        console.log("Fetching Todo failed:", error);
    }
})();

// This immediately invoked async function attempts to fetch a todo with an invalid ID
// The function expects to handle an error, such as a 404 Not Found response.
// If an error occurs, it logs the error message; otherwise, it logs the todo item data.
(async () => {
    try {
        // Attempt to fetch a Todo item using an invalid ID
        const todoItem = await fetchTodo("wrong id");
        console.log("Fetched Todo item: ", todoItem); // This line will not execute if an error is thrown
    } catch (error) {
        // If an error occurs, log a message indicating failure
        console.log("Fetching Todo failed:", error);
    }
})();
