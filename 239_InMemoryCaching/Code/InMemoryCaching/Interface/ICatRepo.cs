using System;
using InMemoryCaching.Database;

namespace InMemoryCaching.Interface;

public interface ICatRepo
{
    Task<List<Categories>> getCategories();
}
