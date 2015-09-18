using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Yue.WebApi.Validators;

namespace Yue.WebApi.Models
{
    [Validator(typeof(TestModelValidator))]
    public class TestModel
    {
        public string Hello { get; set; }
    }
}