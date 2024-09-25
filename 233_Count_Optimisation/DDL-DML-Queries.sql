-- Step 1: Create a Database and Table

-- Create a new database named EmployeeDB
CREATE DATABASE EmployeeDB;

-- Switch to the EmployeeDB database context
USE EmployeeDB;

-- Create a table named 'employees' to store employee data
CREATE TABLE employees (
    id INT PRIMARY KEY,
    name VARCHAR(50),
    department VARCHAR(50),
    salary DECIMAL(10, 2),
    hire_date DATE
);


-- Step 2: Seed Data into the Table

-- Insert a large number of records into the employees table
-- NOTE: This operation may take some time to complete due to the volume of data

DECLARE @i INT = 1;  -- Initialize a counter variable for the loop
WHILE @i <= 1000000   -- Loop to insert 10,00,000 employee records
BEGIN
    INSERT INTO employees (id, name, department, salary, hire_date)
    VALUES (@i, CONCAT('Employee ', @i), 'HR', 1100.00, '2020-01-01'
    );

    SET @i = @i + 1;  -- Increment the counter variable
END


-- Step 3: Run COUNT(*) and COUNT(1) Queries

-- Turn on time statistics to measure the execution time of queries
SET STATISTICS TIME ON;

SELECT COUNT(*) AS total_employees FROM employees;

SELECT COUNT(1) AS total_employees FROM employees;

-- Turn off time statistics after the queries have been executed
SET STATISTICS TIME OFF;