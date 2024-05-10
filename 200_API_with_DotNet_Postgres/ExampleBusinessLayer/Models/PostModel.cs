using AutoMapper;
using AutoMapper.Configuration.Annotations;
using ExampleDataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Searchlight;
using SecurityBlanket.Interfaces;

namespace ExampleBusinessLayer.Models
{
    [SearchlightModel(DefaultSort = nameof(PostModel.ID))]
    public class PostModel : ICustomSecurity
    {
        [SearchlightField]
        public string? ID { get; set; }

        [SearchlightField]
        public string? Title { get; set; }
        
        [SearchlightField]
        public string Url { get; set; } = string.Empty;

        [SearchlightField]
        public string? Summary { get; set; }
        
        [SearchlightField]
        public DateOnly? PostedDate { get; set; }

        public bool IsVisible(HttpContext context)
        {
            return true;
        }
    }
}
