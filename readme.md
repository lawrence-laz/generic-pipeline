[![Build](https://github.com/lawrence-laz/generic-pipeline/workflows/Build/badge.svg)](https://github.com/lawrence-laz/generic-pipeline/actions?query=workflow%3ABuild)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/dd3ad618de8541a88ab111e260733a6f)](https://www.codacy.com/gh/lawrence-laz/generic-pipeline/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=lawrence-laz/generic-pipeline&amp;utm_campaign=Badge_Grade)
[![Codacy Badge](https://app.codacy.com/project/badge/Coverage/dd3ad618de8541a88ab111e260733a6f)](https://www.codacy.com/gh/lawrence-laz/generic-pipeline/dashboard?utm_source=github.com&utm_medium=referral&utm_content=lawrence-laz/generic-pipeline&utm_campaign=Badge_Coverage)

# GenericPipeline

A general purpose dependency free pipeline builder.
https://en.wikipedia.org/wiki/Pipeline_(software)

## Features
- Simple as it gets
- Very fast (see benchmarks)
- Zero dispatch-time allocations
- Zero reflection
- Zero dependencies

## TODO
- [ ] publish to NuGet as a preview package 
- [x] create a builder for pipeline
- [ ] create dependency injection extensions, where providing a servicebuilder you can resolve behavior
- [ ] async implementation
- [ ] finish readme
    - semantic versioning
- [ ] document IRequest and IRequestHandlers and their purpose
    - shows explicitly what is the expected return type for a request
    - provides compile-time check for request return type for all calls (oh c#, if you can validate, why don't you infer...)
- [ ] Write a section about c#'s lack of inference from generic constraints, and why that needs to specify generic type, but show much faster it is and enabled zero allocation struct records.
- [ ] use pipeline pattern terminology to reason about this package in description, so it would use familiar language to the industry. 
- [ ] provide examples on how to implement various patterns, principles and paradigms
    - [ ] dynamic dispatch (proxy)
    - [ ] decoration
    - [ ] chain of responsibility
    - [ ] middleware 
    - [x] mediator
        - notice, this is not a mediator in itself, it can be, if your final handler will be implemented as a dispatcher that will "mediate" the incoming requests to appropriate receivers, but you would have to implement such dispatcher yourself. Instead, this is a tool to build pipline-line systems composed of behaviors, and meadiator pattern is just one of the things you can do with it.
        - maybe provide and example of how mediator could be implemented?
    - [ ] supports single responsibility principle
    - [ ] aspect oriented programming (behaviors are generic, meaning single behavior can be reused across pipelines/request types) 
- [ ] compare to ASP.net core Middleware-like programming, just for any other class besides controllers
- [ ] does not rely on dependency injection, but is compatible with it
- [ ] pipeline builder
    - AddBefore, AddAfter

## Benchmarks
TODO:
- [ ] Compare to mediatr (standard and all singletons)
- [ ] benchmark against other existing stuff: https://github.com/ipvalverde/PipelineNet
- [ ] generic behaviors and handlers allow passing around struct requests without boxing (zero allocations)

<sub>
<a href="https://www.flaticon.com/free-icons/pipe" title="pipe icons">Pipe icons created by Smashicons - Flaticon</a>
</sub>

