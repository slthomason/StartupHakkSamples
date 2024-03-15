//StudentEntity class

public class StudentEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Course { get; set; }
}

//StudentDTO class

public class StudentDTO
{
    // Properties representing student data in the DTO
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Course { get; set; }

    // Constructor for creating a StudentDTO
    public StudentDTO (string firstName, string lastName, int age, string course)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Course = course;
    }
}