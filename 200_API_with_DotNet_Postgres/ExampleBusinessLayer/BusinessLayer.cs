using AutoMapper.Configuration;
using ExampleDataLayer;
using FluentValidation;
using Searchlight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleBusinessLayer
{
    public class BusinessLayer : IBusinessLayer
    {
        private readonly SearchlightEngine _engine;
        private readonly IModelEntityMapper _mapper;

        public BusinessLayer(SearchlightEngine engine, IModelEntityMapper mapper) 
        {
            _engine = engine;
            _mapper = mapper;
        }

        public async Task<List<TModel>> Create<TModel, TEntity>(List<TModel> models) where TEntity : class
        {
            // Insert entities in database
            var entities = _mapper.GetMapper().Map<List<TModel>, List<TEntity>>(models);
            await using (var db = new BloggingContext())
            {
                db.Set<TEntity>().AddRange(entities);
                await db.SaveChangesAsync();
            }

            // Convert back to models
            var resultModels = _mapper.GetMapper().Map<List<TEntity>, List<TModel>>(entities);
            return resultModels;
        }

        public async Task<FetchResult<TEntity>> Query<TEntity>(string filter, string include, string order, int? pageSize, int? pageNumber) where TEntity : class
        {
            var request = new FetchRequest() { filter = filter, include = include, order = order, pageSize = pageSize, pageNumber = pageNumber };
            var source = _engine.FindTable(typeof(TEntity).Name);
            var syntax = source.Parse(request);
            await using (var db = new BloggingContext())
            {
                return syntax.QueryCollection(db.Set<TEntity>());
            }
        }
    }
}
