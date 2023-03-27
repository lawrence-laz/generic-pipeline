[![NuGet Version](https://img.shields.io/nuget/v/GenericPipeline?label=NuGet)](https://www.nuget.org/packages/GenericPipeline/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/GenericPipeline?label=Downloads)](https://www.nuget.org/packages/GenericPipeline/)
[![Build](https://github.com/lawrence-laz/generic-pipeline/workflows/Build/badge.svg)](https://github.com/lawrence-laz/generic-pipeline/actions?query=workflow%3ABuild)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/dd3ad618de8541a88ab111e260733a6f)](https://www.codacy.com/gh/lawrence-laz/generic-pipeline/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=lawrence-laz/generic-pipeline&amp;utm_campaign=Badge_Grade)
[![Codacy Badge](https://app.codacy.com/project/badge/Coverage/dd3ad618de8541a88ab111e260733a6f)](https://www.codacy.com/gh/lawrence-laz/generic-pipeline/dashboard?utm_source=github.com&utm_medium=referral&utm_content=lawrence-laz/generic-pipeline&utm_campaign=Badge_Coverage)

<img align="left" width="100" height="100" src="images/icon.png">

# GenericPipeline

A general purpose dependency free pipeline builder.
https://en.wikipedia.org/wiki/Pipeline_(software)

> ⚠️ This project is in active development and it's a **work in progress**. Bugs and breaking changes are to be expected while in prerelease. [Semantic versioning](https://semver.org/) and stable api is to be expected starting from `v1.0` only.

## 🌟 Features
- **Simplicity**: Designed with simplicity in mind, the library offers a straightforward and easy-to-use pipeline implementation.
- **High Performance**: The library boasts exceptional performance, as demonstrated by its benchmark results.
- **No Dispatch-Time Allocations**: With the exception of async `Task`, the library makes no dispatch-time allocations, thus providing a highly efficient pipeline implementation.
- **No Dependencies**: The library has zero external dependencies, making it lightweight and easy to integrate with other projects.

## 📦️ Get started
Download from [nuget.org](https://www.nuget.org/packages/GenericPipeline/0.0.1-preview):
```
dotnet add package GenericPipeline --prerelease
```
## ⚖️ Feature parity
|                                               | GenericPipeline | MediatR | MessagePipe | PipelineNet | Mediator |
|-----------------------------------------------|:---------------:|:-------:|:-----------:|:-----------:|:--------:|
| Usable **without** dependency injection       |              ✔️  |     ❌  |          ❌ |          ✔️  |       ❌ |
| Usable **with** dependency injection          |              ✔️  |     ✔️   |          ✔️  |          ✔️¹ |       ✔️  |
| .NET framework support                        |              ✔️  |     ✔️   |          ✔️  |          ✔️  |       ❌ |
| Allocation-free dispatching                   |              ✔️  |     ❌  |          ✔️  |          ❌ |       ✔️  |
| Generic request handling                      |              ✔️  |     ✔️   |          ✔️  |          ❌ |       ✔️  |
| Async and sync handlers/behaviors             |              ✔️  |     ❌  |          ✔️  |          ✔️  |       ❌ |
| Change handlers at runtime                    |              ✔️  |     ❌  |          ❌ |          ✔️² |       ❌ |
| Behaviors                                     |              ✔️  |     ✔️   |          ✔️  |          ✔️  |       ✔️  |
| Streams                                       |              ❌ |     ✔️   |          ❌ |          ❌ |       ✔️  |
| Notifications                                 |              ❌ |     ✔️   |          ✔️  |          ❌ |       ✔️  |

¹ <sup>- Unity container only,</sup>
² <sup>- add only, cannot remove</sup>


## ⚡️ Benchmarks
Overhead per invocation for each library. Scenario contains a call to a behavior and a handler.

Sync:
|                Method |         Mean | Allocated |
|---------------------- |-------------:|----------:|
|    Simple method call |       5.7 ns |         - |
|       GenericPipeline |      31.2 ns |         - |
|               MediatR |     502.8 ns |     600 B |
|           PipelineNet |      95.4 ns |     152 B |

Awaited async:
|                Method |         Mean | Allocated |
|---------------------- |-------------:|----------:|
|    Simple method call |     4 863 ns |     560 B |
|       GenericPipeline |     5 906 ns |     864 B |
|               MediatR |     6 662 ns |    1160 B |
|           PipelineNet |     7 017 ns |    1184 B |
 
<sub>
<a href="https://www.flaticon.com/free-icons/pipe" title="pipe icons">Pipe icons created by Smashicons - Flaticon</a>
</sub>

