using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;

namespace Yue.WebApi.Validators
{
    public class ValidatorFactory : AttributedValidatorFactory
    {
        public override IValidator GetValidator(Type type)
        {
            if (type != null)
            {
                var attribute = (ValidatorAttribute)Attribute.GetCustomAttribute(type, typeof(ValidatorAttribute));
                if ((attribute != null) && (attribute.ValidatorType != null))
                {
                    var instance = BootStrapper.Container.Resolve(attribute.ValidatorType);
                    return instance as IValidator;
                }
            }
            return null;
        }
    }
}