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

