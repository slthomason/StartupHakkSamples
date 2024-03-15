var highSalaryEmployees = connection.Query<Employee>(
    "SELECT * FROM Employees WHERE Salary > @salaryThreshold", 
    new { salaryThreshold = 100000 });