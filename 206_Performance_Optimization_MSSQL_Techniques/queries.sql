--1. Indexing Strategies

CREATE CLUSTERED INDEX IX_CustomerID ON Orders(CustomerID);

--2. Query Optimization

SELECT TOP 10 * FROM Products ORDER BY Price DESC;

--We can leverage the ROW_NUMBER() function for better performance:

SELECT * 
FROM (
    SELECT *, ROW_NUMBER() OVER (ORDER BY Price DESC) AS RowNum
    FROM Products
) AS RankedProducts
WHERE RowNum <= 10;


--3. Database Normalization and Denormalization

ALTER TABLE Books
ADD COLUMN AuthorName VARCHAR(100);

--4. Resource Allocation and Configuration
EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;
EXEC sp_configure 'max server memory', 8192; -- Set max memory to 8 GB
RECONFIGURE;

--5. Regular Maintenance and Monitoring
-- Rebuild indexes
ALTER INDEX ALL ON TableName REBUILD;

-- Reorganize indexes
ALTER INDEX ALL ON TableName REORGANIZE;