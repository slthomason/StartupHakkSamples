// The collection
const theList = new Array<string>();

// Function to add items
function addToCollection(item: string) {
    theList.push(item);
}

// Use the typed function
addToCollection("one");
addToCollection("two");



const createUser =({
     userName ='Spencer',
     avatar = 'image.png'
}={})=>({
    userName,
    avatar
});

//const foo = createUser({userName});