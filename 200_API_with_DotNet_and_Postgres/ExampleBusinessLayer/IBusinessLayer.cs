using Searchlight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ExampleBusinessLayer
{
    public interface IBusinessLayer
    {
        Task<List<TModel>> Create<TModel, TEntity>(List<TModel> models) where TEntity : class;

        Task<FetchResult<TEntity>> Query<TEntity>(string filter, string include, string order, int? pageSize,
            int? pageNumber) where TEntity : class;
    }
}
