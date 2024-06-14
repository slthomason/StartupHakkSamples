using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.KeyValue;

namespace CouchbaseSDKExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Initialize the Couchbase cluster
            var clusterOptions = new ClusterOptions
            {
                ConnectionString = "couchbase://localhost",
                UserName = "username", // Replace with your Couchbase username
                Password = "password"  // Replace with your Couchbase password
            };

            // Create a cluster object
            var cluster = await Cluster.ConnectAsync(clusterOptions);
            var bucket = await cluster.BucketAsync("bucket_name");

            // Sample Document Insertion
            var collection = bucket.DefaultCollection();
            var documentId = "document_id";
            var documentContent = new Dictionary<string, object>
            {
                { "Name", "John" },
                { "Age", 30 }
            };

            // Insert the document
            await collection.InsertAsync(documentId, documentContent);
            Console.WriteLine($"Inserted document: {documentId}");

            // Retrieve the document
            var getResult = await collection.GetAsync(documentId);
            var retrievedDocument = getResult.ContentAs<Dictionary<string, object>>();
            Console.WriteLine($"Retrieved document: Name = {retrievedDocument["Name"]}, Age = {retrievedDocument["Age"]}");

            // Update the document
            retrievedDocument["Age"] = 31;
            await collection.UpsertAsync(documentId, retrievedDocument);
            Console.WriteLine($"Updated document: {documentId}");

            // Perform other database operations here

            // Dispose the cluster
            await cluster.DisposeAsync();
        }
    }
}
