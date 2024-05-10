using AutoMapper;
using ExampleBusinessLayer.Models;
using ExampleDataLayer.Entities;

namespace ExampleBusinessLayer
{
    public class ModelEntityMapper : IModelEntityMapper
    {
        private IMapper _mapper;
        private MapperConfiguration _config;

        public ModelEntityMapper() 
        {
            _config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BlogEntity, BlogModel>()
                    .ForMember(entity => entity.ID, opt => opt.MapFrom(model => model.BlogId))
                    .ReverseMap();
                cfg.CreateMap<PostEntity, PostModel>()
                    .ForMember(entity => entity.ID, opt => opt.MapFrom(model => model.PostId))
                    .ReverseMap();
            });
            _mapper = _config.CreateMapper();
        }

        public MapperConfiguration GetConfiguration()
        {
            return _config;
        }

        public IMapper GetMapper()
        {
            return _mapper;
        }
    }
}
