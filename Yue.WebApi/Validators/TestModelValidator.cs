using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Yue.WebApi.Models;

namespace Yue.WebApi.Validators
{
    public class TestModelValidator : AbstractValidator<TestModel>
    {
        public TestModelValidator()
        {
            RuleFor(x => x.Hello).NotEmpty().WithMessage("World");
            RuleFor(x => x.Hello).Length(6, 20).WithMessage("Length");
        }
    }
}