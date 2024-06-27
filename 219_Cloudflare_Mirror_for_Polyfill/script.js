// Function to replace polyfill.io links with Cloudflare's mirror
function replacePolyfillLinks() {
    const scripts = document.querySelectorAll('script[src*="polyfill.io"]');
    scripts.forEach(script => {
        const newSrc = script.src.replace('cdn.polyfill.io', 'cdnjs.cloudflare.com/ajax/libs/polyfill');
        script.src = newSrc;
    });
}

// Run the function on page load
window.onload = replacePolyfillLinks;
