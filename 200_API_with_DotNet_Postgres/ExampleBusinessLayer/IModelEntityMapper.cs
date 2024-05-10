using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleBusinessLayer
{
    public interface IModelEntityMapper
    {
        IMapper GetMapper();
        MapperConfiguration GetConfiguration();
    }
}
