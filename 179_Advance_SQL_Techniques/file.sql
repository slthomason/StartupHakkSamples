/* Subqueries */
SELECT *
FROM employees
WHERE department_id IN (SELECT department_id FROM departments WHERE location_id = 1700);

/*Joins*/

SELECT e.employee_id, e.first_name, e.last_name, d.department_name
FROM employees e
JOIN departments d ON e.department_id = d.department_id;


/* Aggregate Functions */

SELECT AVG(salary) AS avg_salary
FROM employees;

/*Window Functions*/

SELECT employee_id, salary, 
       AVG(salary) OVER (PARTITION BY department_id) AS avg_salary_by_dept
FROM employees;

/*Common Table Expressions (CTEs)*/

WITH high_salary_employees AS (
    SELECT *
    FROM employees
    WHERE salary > 100000
)
SELECT * FROM high_salary_employees;

/* Pivot Tables */
SELECT *
FROM (
    SELECT department_id, job_id, salary
    FROM employees
)
PIVOT (
    AVG(salary)
    FOR job_id IN ('ST_CLERK', 'IT_PROG', 'SA_REP', 'SA_MAN')
);

/* Unions and Intersections*/
SELECT employee_id FROM employees
UNION
SELECT employee_id FROM job_history;

/*Case Statements*/

SELECT employee_id,
       CASE
           WHEN salary > 100000 THEN 'High'
           WHEN salary > 50000 THEN 'Medium'
           ELSE 'Low'
       END AS salary_category
FROM employees;




/* Recursive Queries */

WITH RECURSIVE employee_hierarchy AS (
    SELECT employee_id, first_name, last_name, manager_id
    FROM employees
    WHERE manager_id IS NULL
    UNION ALL
    SELECT e.employee_id, e.first_name, e.last_name, e.manager_id
    FROM employees e
    INNER JOIN employee_hierarchy eh ON e.manager_id = eh.employee_id
)
SELECT * FROM employee_hierarchy;

/*Ranking Functions*/

SELECT employee_id, salary,
       RANK() OVER (PARTITION BY department_id ORDER BY salary DESC) AS salary_rank
FROM employees;

/* Data Modification Statements */
INSERT INTO employees (employee_id, first_name, last_name, email, hire_date)
VALUES (1001, 'John', 'Doe', 'john.doe@example.com', '2023-02-15');

/* Temporary Tables */

CREATE TEMPORARY TABLE temp_employees AS
SELECT * FROM employees WHERE department_id = 50;

/* Grouping Sets */

SELECT department_id, job_id, SUM(salary) AS total_salary
FROM employees
GROUP BY GROUPING SETS ((department_id), (department_id, job_id));

/* Stored Procedures */
CREATE PROCEDURE get_employee_info (IN employee_id INT)
BEGIN
    SELECT * FROM employees WHERE employee_id = employee_id;
END;


/* Indexing */
CREATE INDEX idx_last_name ON employees (last_name);


/* Materialized Views */
CREATE MATERIALIZED VIEW mv_employee_salary AS
SELECT employee_id, salary FROM employees;

/* Database Constraints */

ALTER TABLE employees ADD CONSTRAINT fk_department_id FOREIGN KEY (department_id) REFERENCES departments(department_id);

/* Conditional Aggregation */

SELECT department_id,
       COUNT(CASE WHEN salary > 50000 THEN 1 END) AS high_salary_count,
       COUNT(CASE WHEN salary <= 50000 THEN 1 END) AS low_salary_count
FROM employees
GROUP BY department_id;

/* Window Frame Clauses */

SELECT employee_id, salary,
       SUM(salary) OVER (ORDER BY employee_id ROWS BETWEEN 1 PRECEDING AND 1 FOLLOWING) AS salary_sum
FROM employees;

/* Dynamic SQL */
EXECUTE IMMEDIATE 'SELECT * FROM employees WHERE department_id = :dept_id' USING 50;
