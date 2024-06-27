using AutoMapper;
using AutoMapper.Configuration.Annotations;
using ExampleDataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Searchlight;
using SecurityBlanket.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ExampleBusinessLayer.Models
{
    /// <summary>
    /// A blog site containing multiple articles published by a single author
    /// </summary>
    [SearchlightModel(DefaultSort = nameof(BlogModel.ID))]
    public class BlogModel : ICustomSecurity
    {
        /// <summary>
        /// The ID number of this record
        /// </summary>
        [SearchlightField]
        public string? ID { get; set; }

        /// <summary>
        /// The URL of this blog site
        /// </summary>
        [SearchlightField]
        public string? URL { get; set; }
        
        /// <summary>
        /// The author or publisher of this blog
        /// </summary>
        [SearchlightField]
        public string? Author { get; set; }
        
        /// <summary>
        /// A short description of this blog site
        /// </summary>
        [SearchlightField]
        public string? Description { get; set; }

        public bool IsVisible(HttpContext context)
        {
            return true;
        }
    }
}
