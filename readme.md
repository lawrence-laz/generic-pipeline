[![Build](https://github.com/lawrence-laz/generic-pipeline/workflows/Build/badge.svg)](https://github.com/lawrence-laz/generic-pipeline/actions?query=workflow%3ABuild)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/dd3ad618de8541a88ab111e260733a6f)](https://www.codacy.com/gh/lawrence-laz/generic-pipeline/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=lawrence-laz/generic-pipeline&amp;utm_campaign=Badge_Grade)
[![Codacy Badge](https://app.codacy.com/project/badge/Coverage/dd3ad618de8541a88ab111e260733a6f)](https://www.codacy.com/gh/lawrence-laz/generic-pipeline/dashboard?utm_source=github.com&utm_medium=referral&utm_content=lawrence-laz/generic-pipeline&utm_campaign=Badge_Coverage)

<img align="left" width="100" height="100" src="images/icon.png">

# GenericPipeline

A general purpose dependency free pipeline builder.
https://en.wikipedia.org/wiki/Pipeline_(software)

## 🌟 Features
- Simple as it gets
- Very fast (see benchmarks)
- Zero dispatch-time allocations (except for async `Task`)
- Zero reflection
- Zero dependencies

## TODO
- [ ] publish to NuGet as a preview package 
- [x] create a builder for pipeline
- [x] create dependency injection extensions, where providing a servicebuilder you can resolve behavior
- [x] async implementation
- [ ] finish readme
    - semantic versioning
    - [ ] generic behaviors and handlers allow passing around struct requests without boxing (zero allocations)
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

## ⚡️ Benchmarks
|                 Method  |       Mean | Allocated |
|------------------------:|-----------:|----------:|
|          Regular method |   111.3 ns |         - |
|  Regular method (async) | 5,058.8 ns |     560 B |
|         **GenericPipeline** |   143.7 ns |         - |
| **GenericPipeline (async)** | 6,085.2 ns |     848 B |
|                 MediatR |   646.0 ns |     600 B |
|         MediatR (async) | 6,780.9 ns |    1160 B |
|             PipelineNet |   213.4 ns |     152 B |
|     PipelineNet (async) | 6,961.6 ns |    1184 B |
 
<sub>
<a href="https://www.flaticon.com/free-icons/pipe" title="pipe icons">Pipe icons created by Smashicons - Flaticon</a>
</sub>

