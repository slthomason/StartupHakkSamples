using ExampleBusinessLayer.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace ExampleBusinessLayer.Validators
{
    public class BlogModelValidator : AbstractValidator<BlogModel>
    {
        public BlogModelValidator(IHttpContextAccessor httpContextAccessor)
        {
            RuleFor(blog => blog.URL).NotNull().NotEmpty();
            RuleFor(blog => blog.ID)
                .Must(value => NullOnCreate(value, httpContextAccessor.HttpContext.Request.Method))
                .WithMessage("The ID field must be null when calling create.");
        }

        private bool NullOnCreate(string? value, string method)
        {
            if (method.ToUpper() == "POST")
            {
                return value == null;
            }

            return value != null;
        }
    }

    public class BlogModelListValidator : AbstractValidator<List<BlogModel>>
    {
        public BlogModelListValidator(IHttpContextAccessor httpContextAccessor)
        {
            RuleForEach(list => list).SetValidator(new BlogModelValidator(httpContextAccessor));
        }
    }
}
