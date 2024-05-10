using ExampleBusinessLayer.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleBusinessLayer.Validators
{
    public class PostModelValidator : AbstractValidator<PostModel>
    {
        public PostModelValidator() 
        {
            RuleFor(post => post.Title).NotEmpty();
        }
    }
}
