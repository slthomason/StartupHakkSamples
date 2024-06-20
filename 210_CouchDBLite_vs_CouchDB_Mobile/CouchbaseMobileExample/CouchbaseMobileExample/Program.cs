using Couchbase.Lite;
using Couchbase.Lite.Sync;
using System;

namespace CouchbaseMobileExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create or open a local database
            var database = new Database("mydb");

            // Create a new document
            var document = new MutableDocument()
                .SetString("type", "task")
                .SetString("name", "Do laundry")
                .SetBoolean("completed", false);

            // Save the document
            database.Save((MutableDocument)document);

            // Configure the Sync Gateway URL and replication
            var endpoint = new URLEndpoint(new Uri("ws://localhost:4984/mydb"));
            var replicationConfig = new ReplicatorConfiguration(database, endpoint)
            {
                ReplicatorType = ReplicatorType.PushAndPull
            };

            // Start the replicator
            var replicator = new Replicator(replicationConfig);
            replicator.Start();

            // Observe the replicator status
            replicator.AddChangeListener((sender, e) =>
            {
                if (e.Status.Error != null)
                {
                    Console.WriteLine($"Error: {e.Status.Error}");
                }
                else
                {
                    Console.WriteLine($"Status: {e.Status.Activity}");
                }
            });

            // Wait for some time to let the synchronization complete
            System.Threading.Thread.Sleep(10000);

            // Clean up
            replicator.Stop();
            database.Close();
        }
    }
}
