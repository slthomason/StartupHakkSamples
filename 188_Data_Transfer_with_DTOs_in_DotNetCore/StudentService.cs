public class StudentService
{
    // Method to retrieve student information from a data source
    public StudentDTO GetStudentInfo(int studentId)
    {
        // Assuming there is a method to fetch student information from a data source
        // For this example, we create a mock method
        var studentEntity = GetStudentEntityFromDataSource(studentId);

        // Mapping the entity to DTO
        var studentDTO = MapToStudentDTO(studentEntity);

        return studentDTO;
    }

    // Mock method to retrieve student information from a data source
    private StudentEntity GetStudentEntityFromDataSource(int studentId)
    {
        // In a real scenario, this method would fetch data from a database or another source
        // For simplicity, we create a mock student entity
        return new StudentEntity
        {
            FirstName = "John",
            LastName = "Doe",
            Age = 20,
            Course = "Computer Science"
        };
    }

    // Mapping method to convert a StudentEntity to a StudentDTO
    private StudentDTO MapToStudentDTO(StudentEntity studentEntity)
    {
        // Manually creating a StudentDTO instance
        return new StudentDTO(
            studentEntity.FirstName,
            studentEntity.LastName,
            studentEntity.Age,
            studentEntity.Course
        );
    }
}
