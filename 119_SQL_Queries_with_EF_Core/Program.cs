
//EF Core And SQL Queries

var startDate = new DateOnly(2023, 1, 1);

var ordersIn2023 = await dbContext
    .Database
    .SqlQuery<OrderSummary>(
        $"SELECT * FROM OrderSummaries AS o WHERE o.CreatedOn >= {startDate}")
    .ToListAsync();

//SELECT * FROM OrderSummaries AS o WHERE o.CreatedOn >= @p0



//Composing SQL Queries With LINQ

var startDate = new DateOnly(2023, 1, 1);

var ordersIn2023 = await dbContext
    .Database
    .SqlQuery<OrderSummary>("SELECT * FROM OrderSummaries AS o")
    .Where(o => o.CreatedOn >= startDate)
    .ToListAsync();

/*
SELECT s.Id, s.CustomerId, s.TotalPrice, s.CreatedOn
FROM (
    SELECT * FROM OrderSummaries AS o
) AS s
WHERE s.CreatedOn >= @p0
*/

var startDate = new DateOnly(2023, 1, 1);

var ordersIn2023 = await dbContext
    .Database
    .SqlQuery<OrderSummary>(
        $"SELECT * FROM OrderSummaries AS o WHERE o.CreatedOn >= {startDate}")
    .OrderBy(o => o.Id)
    .Skip(10)
    .Take(5)
    .ToListAsync();

/*
SELECT s.Id, s.CustomerId, s.TotalPrice, s.CreatedOn
FROM (
    SELECT * FROM OrderSummaries AS o WHERE o.CreatedOn >= @p0
) AS s
ORDER BY s.Id
OFFSET @p1 ROWS FETCH NEXT @p2 ROWS ONLY

*/

//SQL Queries For Data Modifications
var startDate = new DateOnly(2023, 1, 1);

dbContext.Database.ExecuteSql(
    $"UPDATE Orders SET Status = 5 WHERE CreatedOn >= {startDate}");