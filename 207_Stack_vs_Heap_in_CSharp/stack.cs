//Consider this simple example:

public int ConvertMinutesToSeconds(int minutes)
{
    int seconds = minutes * 60;
    return seconds;
}

//The Twist of Stack Overflow
struct Cart 
{
    int Fruits;
    int Vegetables;
    int Beverages;
    // Assuming additional data making it 400 bytes in size.
}

void CheckOut() 
{
    Cart cart = new Cart();
    int amount = CalculateAmount(cart); // Passes 'cart' by value
}

int CalculateAmount(Cart cart) {
    // Calculate and return amount
}

//After Optimization with ref

void CheckOut() 
{
    Cart cart = new Cart();
    int amount = CalculateAmount(ref cart); // Now passing 'cart' by reference
}

int CalculateAmount(ref Cart cart) 
{
    // Directly work with the original 'Cart' instance
    // Calculate and return amount
}

//Copying Data in the Stack
public int GetFruitsCount() 
{
    // Declare an integer 'fruits' and initialize it with a value of 10.
    // This value is stored in the stack.
    int fruits = 10;

    // Copy the value of 'fruits' (which is 10) into a new integer variable 'vegetables'.
    // A separate memory location is used for 'vegetables' in the stack,
    // meaning 'fruits' and 'vegetables' are independent of each other.
    int vegetables = fruits;

    // Modify 'vegetables' by adding 5 to it. This operation affects only 'vegetables'.
    // The value of 'fruits' remains unchanged because 'vegetables' is a copy,
    // not a reference to 'fruits'.
    vegetables += 5;

    // Return the value of 'fruits', which is still 10, demonstrating that
    // the earlier operation on 'vegetables' did not affect 'fruits'.
    return fruits;
}