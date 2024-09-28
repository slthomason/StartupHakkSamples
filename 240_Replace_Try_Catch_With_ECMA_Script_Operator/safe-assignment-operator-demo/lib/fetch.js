// Note: The standard Fetch API does not have built-in support for Symbol.result,
// which is a mechanism that can be used for structured error reporting.
// As such, a custom solution must be implemented to achieve similar functionality.
// Below is a suggested way to bind enhancedFetch to the window context

// Enhanced fetch function that provides structured error handling
export async function enhancedFetch(...args) {
	// Perform the fetch request
	const response = await fetch(...args);

	// Note: The Fetch API does not throw an error for non-2xx status codes (like 404).
	// For demo purposes, we throw an error here to handle fetch failures uniformly.
	// Check if the response is not OK (e.g., status code is not in the 200 range)
	if (!response.ok) {
		// If the fetch fails, throw an error with a descriptive message
		throw new Error(`Fetch Failed! Status: ${response.status} - ${response.statusText}`);
	}

	// Return the response object if the fetch is successful
	return response;
}