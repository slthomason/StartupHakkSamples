using ExampleApi;
using ExampleBusinessLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace ExampleTestSuite
{
    [TestClass]
    public class CoherencyTests
    {
        [TestMethod]
        public void TestAutomapperSetup()
        {
            // A test to make sure all models and entities are fully mapped to each other
            var mem = new ModelEntityMapper();
            mem.GetConfiguration().AssertConfigurationIsValid();
        }

        [TestMethod]
        public void TestSwashbuckleDocumentation()
        {
            // Fetch the OpenAPI specification for this site and assert that all methods and models
            // have documentation.  We do this by scanning for controllers and parameters.
            var server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());

            // Fetch the swagger provider
            var swagger = server.Services.GetService<ISwaggerProvider>();
            Assert.IsNotNull(swagger);

            // Generate swagger
            var openApiDoc = swagger.GetSwagger("v1");
            Assert.IsNotNull(openApiDoc);

            // Assert that everything has the necessary documentation
            foreach (var path in openApiDoc.Paths)
            {
                // Check methods
                foreach (var method in path.Value.Operations)
                {
                    Assert.IsFalse(String.IsNullOrWhiteSpace(method.Value.Summary), $"Method {method.Key} for path {path.Key} does not have a <summary> xmldoc.");
                    Assert.IsFalse(String.IsNullOrWhiteSpace(method.Value.Description), $"Method {method.Key} for path {path.Key} does not have a <remarks> xmldoc.");
                    foreach (var parameter in method.Value.Parameters)
                    {
                        Assert.IsFalse(String.IsNullOrWhiteSpace(parameter.Description), $"Method {method.Key} for path {path.Key} does not have a <param name=\"{parameter.Name}\"> xmldoc.");
                    }
                    foreach (var response in method.Value.Responses)
                    {
                        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Value.Description), $"Method {method.Key} for path {path.Key} does not have a <returns> xmldoc for return type {response.Key}.");
                    }
                    if (method.Value.RequestBody != null)
                    {
                        Assert.IsFalse(String.IsNullOrWhiteSpace(method.Value.RequestBody.Description), $"Method {method.Key} for path {path.Key} does not have a <param> xmldoc for the request body.");
                    }
                }
            }

            // Check schemas
            foreach (var schema in openApiDoc.Components.Schemas)
            {
                Assert.IsFalse(String.IsNullOrWhiteSpace(schema.Value.Description), $"Schema {schema.Key} does not have a <summary> xmldoc on the class.");
                foreach (var property in schema.Value.Properties)
                {
                    Assert.IsFalse(String.IsNullOrWhiteSpace(property.Value.Description), $"Schema {schema.Key} does not have a <summary> xmldoc for property {property.Key}.");
                }
            }
        }

        [TestMethod]
        public void TestSearchlightEngine()
        {
            // Make sure all Searchlight models are configured correctly
        }
    }
}