using FluentValidation;
using GenericPipeline;

namespace FluentValidationExamples;

public static class FluentValidationPipelineExtensions
{
    public static Pipeline AppendFluentValidators(this Pipeline pipeline, params IValidator[] validators)
    {
        pipeline.AppendBehavior(new FluentValidationBehavior(validators));
        return pipeline;
    }
}

