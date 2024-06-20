using Couchbase.Lite;
using Couchbase.Lite.Query;
using System;
using System.Collections.Generic;

namespace CouchbaseLiteExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create or open a database
            var database = new Database("mydb");

            // Create a new document (i.e., a record) in the database
            var document = new MutableDocument()
                .SetString("type", "user")
                .SetString("name", "John Doe")
                .SetInt("age", 28);

            // Save it to the database
            database.Save((MutableDocument)document);

            // Retrieve the document from the database
            var retrievedDocument = database.GetDocument(document.Id);
            Console.WriteLine($"Document ID: {retrievedDocument.Id}");
            Console.WriteLine($"Name: {retrievedDocument.GetString("name")}");
            Console.WriteLine($"Age: {retrievedDocument.GetInt("age")}");

            // Querying the database
            var query = QueryBuilder.Select(SelectResult.All())
                                    .From(DataSource.Database(database))
                                    .Where(Expression.Property("type").EqualTo(Expression.String("user")));

            var result = query.Execute();
            foreach (var row in result)
            {
                var doc = row.GetDictionary("mydb");
                Console.WriteLine($"Name: {doc.GetString("name")}, Age: {doc.GetInt("age")}");
            }

            // Clean up
            database.Close();
        }
    }
}
