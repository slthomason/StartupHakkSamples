-- Create departments table
CREATE TABLE departments (
    department_id INT PRIMARY KEY,
    department_name VARCHAR(50) NOT NULL
);

-- Insert sample data into departments
INSERT INTO departments (department_id, department_name) VALUES
(1, 'Sales'),
(2, 'Marketing'),
(3, 'HR');

-- Create employees table
CREATE TABLE employees (
    employee_id INT PRIMARY KEY,
    employee_name VARCHAR(50) NOT NULL,
    department_id INT,
    manager_id INT,
    salary DECIMAL(10, 2),
    FOREIGN KEY (department_id) REFERENCES departments(department_id),
    FOREIGN KEY (manager_id) REFERENCES employees(employee_id)
);

-- Insert sample data into employees
INSERT INTO employees (employee_id, employee_name, department_id, manager_id, salary) VALUES
(1, 'John Doe', 1, NULL, 60000),
(2, 'Jane Smith', 1, 1, 65000),
(3, 'Alice Johnson', 2, 1, 70000),
(4, 'Chris Brown', 3, 2, 50000);

-- Create orders table
CREATE TABLE orders (
    order_id INT PRIMARY KEY,
    order_date DATE,
    employee_id INT,
    total_sales DECIMAL(10, 2),
    FOREIGN KEY (employee_id) REFERENCES employees(employee_id)
);

-- Insert sample data into orders
INSERT INTO orders (order_id, order_date, employee_id, total_sales) VALUES
(1, '2023-01-01', 1, 5000),
(2, '2023-02-01', 2, 7000),
(3, '2023-03-01', 1, 3000),
(4, '2023-04-01', 3, 10000);

-- Create sales table
CREATE TABLE sales (
    sale_id INT PRIMARY KEY,
    order_date DATE,
    region VARCHAR(50),
    total_sales DECIMAL(10, 2)
);

-- Insert sample data into sales
INSERT INTO sales (sale_id, order_date, region, total_sales) VALUES
(1, '2023-01-01', 'North', 15000),
(2, '2023-02-01', 'South', 20000),
(3, '2023-03-01', 'East', 12000),
(4, '2023-04-01', 'West', 18000);

