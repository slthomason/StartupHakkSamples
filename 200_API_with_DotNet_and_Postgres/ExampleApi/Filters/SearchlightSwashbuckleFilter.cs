using Microsoft.OpenApi.Models;
using Searchlight;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ExampleApi.Filters
{
    public class SearchlightSwashbuckleFilter : ISchemaFilter, IOperationFilter
    {
        private SearchlightEngine _engine;

        public SearchlightSwashbuckleFilter(SearchlightEngine engine) 
        {
            _engine = engine;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var returnTypeName = context.MethodInfo.ReturnType.FullName;
            if (returnTypeName != null) { 
                if (returnTypeName.StartsWith("Searchlight.FetchResult`1[["))
                {
                    var innerTypeName = context.MethodInfo.ReturnType.GenericTypeArguments[0]?.Name;
                    ApplySearchlightDocumentation(operation, innerTypeName);
                }
                else if (returnTypeName.StartsWith("System.Threading.Tasks.Task`1[[Searchlight.FetchResult`1[["))
                {
                    var innerTypeName = context.MethodInfo.ReturnType.GenericTypeArguments[0]?.GenericTypeArguments[0]?.Name;
                    ApplySearchlightDocumentation(operation, innerTypeName);
                }
            }
        }

        private void ApplySearchlightDocumentation(OpenApiOperation operation, string? innerTypeName)
        {
            operation.Description = $"Queries {innerTypeName} records using the specified filtering, sorting, nested fetch, and pagination rules requested.  To write a query, see the [Searchlight query language](https://github.com/tspence/csharp-searchlight/wiki/Querying-with-Searchlight).";
            foreach (var parameter in operation.Parameters)
            {
                parameter.Description = GetOperationParameterDescription(parameter.Name, innerTypeName);
            }
        }

        private string GetOperationParameterDescription(string name, string? innerTypeName)
        {
            var table = _engine.FindTable(innerTypeName);
            switch (name)
            {
                case "filter": return $"The filter for this query as written in the [Searchlight query language](https://github.com/tspence/csharp-searchlight/wiki/Querying-with-Searchlight)";
                case "include":
                    var commands = String.Join(", ", from command in table.Commands select command.GetName());
                    var commandText = String.IsNullOrWhiteSpace(commands) 
                        ? $"No collections are currently available on {innerTypeName}, but may be offered in the future." 
                        : "Available collections: " + commands;
                    return $"To fetch additional data on this object, specify the list of elements to retrieve. {commands}";
                case "order": return $"The sort order for this query.  See See [Searchlight query language](https://github.com/tspence/csharp-searchlight/wiki/Querying-with-Searchlight)";
                case "pageSize": return $"The page size for results (default {_engine.DefaultPageSize?.ToString() ?? "unlimited"}, maximum of {_engine.MaximumPageSize?.ToString() ?? "unlimited"}).  See [Searchlight Query Language](https://github.com/tspence/csharp-searchlight/wiki/Querying-with-Searchlight)";
                case "pageNumber": return $"The page number for results (default 0).  See [Searchlight Query Language](https://github.com/tspence/csharp-searchlight/wiki/Querying-with-Searchlight)";
                case "skip": return $"TODO";
                case "take": return $"TODO";
                default: throw new Exception($"Unknown parameter: {name}");
            }
        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var name = context.Type?.FullName;
            if (name != null && name.StartsWith("Searchlight.FetchResult`1"))
            {
                var innerTypeName = context.Type?.GenericTypeArguments[0]?.Name;
                schema.Description = $"The collection of {innerTypeName} records matching your query.";
                foreach (var property in schema.Properties)
                {
                    property.Value.Description = GetSchemaPropertyDescription(property.Key, innerTypeName);
                }
            }
        }

        private string GetSchemaPropertyDescription(string key, string? innerTypeName)
        {
            switch (key)
            {
                case "totalCount": return $"The total number of {innerTypeName} records matching the filter.  If unknown, returns null.";
                case "pageSize": return $"If the original request was submitted using Page Size-based pagination, contains the page size for this query.  Null otherwise.";
                case "pageNumber": return $"If the original request was submitted using Page Size-based pagination, contains the page number of this current result.  Null otherwise.";
                case "records": return $"The paginated and filtered list of {innerTypeName} records matching the parameters you supplied.";
                default: throw new Exception($"Unknown property: {key}");
            }
        }
    }
}