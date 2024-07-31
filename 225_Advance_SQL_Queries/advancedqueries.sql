-- Section 1: Subqueries

-- 1.1 Subquery with IN Clause
-- Example: Selecting employees in departments with specific IDs
SELECT *
FROM employees
WHERE department_id IN (SELECT department_id FROM departments WHERE department_name = 'Sales');

-- 1.2 Subquery with EXISTS Clause
-- Example: Selecting employees with existing orders
SELECT *
FROM employees
WHERE EXISTS (SELECT 1 FROM orders WHERE orders.employee_id = employees.employee_id);


-- Section 2: Advance Joins

-- Example: Selecting employees and their managers
SELECT e.employee_name, m.employee_name AS manager_name
FROM employees e
JOIN employees m ON e.manager_id = m.employee_id;



-- Section 3: Window Functions

-- 2.1 ROW_NUMBER() for Ranking
-- Example: Ranking employees by salary within each department
SELECT employee_name, salary, department_id,
       ROW_NUMBER() OVER (PARTITION BY department_id ORDER BY salary DESC) AS salary_rank
FROM employees;

-- 2.2 LEAD() and LAG() for Time-Based Analysis
-- Example: Analyzing the change in sales over time
SELECT order_date, total_sales,
       LAG(total_sales) OVER (ORDER BY order_date) AS previous_sales,
       LEAD(total_sales) OVER (ORDER BY order_date) AS next_sales
FROM orders;


-- Section 3: Common Table Expressions (CTEs)

-- 3.1 Recursive CTE for Hierarchical Data
-- Example: Selecting all employees in a hierarchical manner
WITH RECURSIVE EmployeeHierarchy AS (
  SELECT employee_id, employee_name, manager_id
  FROM employees
  WHERE manager_id IS NULL

UNION ALL

  SELECT e.employee_id, e.employee_name, e.manager_id
  FROM employees e
  JOIN EmployeeHierarchy eh ON e.manager_id = eh.employee_id
)
SELECT * FROM EmployeeHierarchy;

-- 3.2 Using CTEs for Complex Queries
-- Example: Calculating the average salary difference from department average
WITH DepartmentAvg AS (
  SELECT department_id, AVG(salary) AS avg_salary
  FROM employees
  GROUP BY department_id
)
SELECT e.employee_name, e.salary, d.avg_salary, e.salary - d.avg_salary AS salary_difference
FROM employees e
JOIN DepartmentAvg d ON e.department_id = d.department_id;


-- Section 4: Advanced Aggregations

-- 4.1 GROUP_CONCAT() for Concatenation
-- Example: Concatenating employee names in each department
SELECT department_id, GROUP_CONCAT(employee_name ORDER BY employee_name ASC) AS employee_list
FROM employees
GROUP BY department_id;

-- 4.2 ROLLUP for Hierarchical Aggregation
-- Example: Aggregating sales by region and month with ROLLUP
SELECT region, MONTH(order_date) AS month, SUM(total_sales) AS monthly_sales
FROM sales
GROUP BY ROLLUP(region, MONTH(order_date));