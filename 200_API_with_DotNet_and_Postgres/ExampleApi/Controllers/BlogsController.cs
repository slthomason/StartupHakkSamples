using ExampleBusinessLayer;
using ExampleBusinessLayer.Models;
using ExampleBusinessLayer.Validators;
using ExampleDataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Searchlight;
using System.Data;
using FluentValidation;
using JsonPatchCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExampleApi.Controllers
{
    /// <summary>
    /// Test
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBusinessLayer _businessLayer;

        /// <summary>
        /// Test
        /// </summary>
        /// <param name="businessLayer"></param>
        public BlogsController(IBusinessLayer businessLayer) 
        { 
            _businessLayer = businessLayer;
        }

        /// <summary>Create Blogs</summary>
        /// <remarks>
        /// Create one or more blog records
        ///
        /// A blog record contains information about a blog site.  The site contains multiple articles published by a
        /// single author.
        /// </remarks>
        /// <param name="models">comment</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<BlogModel>> BlogCreate([FromBody]List<BlogModel> models)
        {
            var results = await _businessLayer.Create<BlogModel, BlogEntity>(models);
            return results;
        }

        /// <summary>
        /// More Stuff
        /// </summary>
        /// <remarks>Test</remarks>
        /// <param name="id">comment</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Task<BlogModel[]> BlogRetrieve([FromRoute]Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// More Stuff
        /// </summary>
        /// <remarks>Test</remarks>
        /// <param name="id">comment</param>
        /// <param name="model">comment</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public Task<BlogModel[]> BlogUpdate([FromRoute]Guid id, [FromBody]BlogModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// More Stuff
        /// </summary>
        /// <remarks>Test</remarks>
        /// <param name="id">comment</param>
        /// <param name="patch">comment</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public Task<BlogModel[]> BlogPatch([FromRoute]Guid id, [FromBody]JsonPatchDocument<BlogModel> patch)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// More Stuff
        /// </summary>
        /// <remarks>Test</remarks>
        /// <param name="id">comment</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task<BlogModel[]> BlogDelete([FromRoute]Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetch blogs
        /// </summary>
        /// <remarks>Test</remarks>
        /// <param name="include">test</param>
        /// <param name="order">t</param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="filter">t</param>
        /// <returns></returns>
        [HttpGet("query")]
        public Task<FetchResult<BlogModel>> BlogQuery([FromQuery] string filter, [FromQuery] string include, [FromQuery] string order, [FromQuery] int? pageSize, [FromQuery] int? pageNumber)
        {
            return _businessLayer.Query<BlogModel>(filter, include, order, pageSize, pageNumber);
        }
    }
}