using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using GenericPipeline;

namespace FluentValidationExamples;

public class FluentValidationBehavior : PipelineBehavior
{
    private readonly IEnumerable<IValidator> _validators;

    public FluentValidationBehavior(IEnumerable<IValidator> validators)
    {
        _validators = validators;
    }

    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        var validator = _validators.FirstOrDefault(validator => validator is IValidator<TRequest>);
        if (validator is IValidator<TRequest> requestValidator)
        {
            var validationResult = requestValidator.Validate(request);
            if (validationResult.IsValid)
            {
                return HandleNext<TRequest, TResponse>(request);
            }
            else
            {
                throw new Exception(string.Join(Environment.NewLine, validationResult.Errors));
            }
        }
        else
        {
            return HandleNext<TRequest, TResponse>(request);
        }
    }
}

