//Understanding Reference Types

public class Animal
{
    public string Name { get; set; }
}

string CreateAnimal()
{
    Animal animal = new Animal();
    animal.Name = "Baloo";
}

//Boxing-Unboxing: The Bridge Between Two Worlds
int num = 42;
object Obj = num; // Boxing:   Value Type -> Reference Type
int i = (int)Obj; // Unboxing: Reference Type -> Value Type


//Copying Data in the Heap
public class Cart 
{
    public int ItemsCount { get; set; }
}

public int GetFruitsCount() 
{
    // A new instance of the Cart class is instantiated and assigned to the variable 'fruits'.
    // This instance is allocated on the heap, and 'fruits' holds a reference to it.
    Cart fruits = new Cart();
    fruits.ItemsCount = 10;

    // The 'vegetables' variable is assigned the reference held by 'fruits'.
    // Both 'fruits' and 'vegetables' now reference the same Cart instance on the heap.
    // No new Cart instance is created; instead, 'vegetables' becomes an alias for the 'fruits' object.
    Cart vegetables = fruits;

    // Modifying the ItemsCount property of the 'vegetables' reference impacts the same instance that 'fruits' refers to,
    // because they are aliases of each other, pointing to the same memory location on the heap.
    vegetables.ItemsCount += 5;

    // Returning the ItemsCount property of the 'fruits' reference will reflect the modification made through 'vegetables',
    // demonstrating the shared nature of object references.
    // The return value will be 15, indicating that the operation through one reference affects the state of the shared object.
    return fruits.ItemsCount;
}