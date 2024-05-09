using System;
using System.Collections.Generic;
using Couchbase;
using Couchbase.Configuration.Client;
using Couchbase.Core;
using Couchbase.N1QL;

public class CouchbaseManager : IDisposable
{
    private readonly Cluster _cluster;
    private readonly IBucket _bucket;

    public CouchbaseManager(string serverUri, string bucketName)
    {
        _cluster = new Cluster(new ClientConfiguration
        {
            Servers = new List<Uri> { new Uri(serverUri) }
        });
        
        _bucket = _cluster.OpenBucket(bucketName);
    }

    public void InsertDocument<T>(string documentId, T content)
    {
        var document = new Document<T>
        {
            Id = documentId,
            Content = content
        };

        _bucket.Insert(document);
    }

    public void Dispose()
    {
        _cluster.Dispose();
    }
}
public class Program
{
  public void Main(string[] args)
  {
    var couchbaseManager = new CouchbaseManager("http://localhost:8091", "bucket_name");

    var documentContent = new Dictionary<string, object>
    {
        { "Name", "John" },
        { "Age", 30 }
    };

    couchbaseManager.InsertDocument("document_id", documentContent);
    couchbaseManager.Dispose();
  }
}