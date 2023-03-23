#!/bin/bash
dotnet test /p:CollectCoverage=true /p:CoverletOutput=TestResults/ /p:CoverletOutputFormat=opencover
reportgenerator \
    -reports:"test/GenericPipeline.Tests/TestResults/coverage.opencover.xml" \
    -targetdir:"coveragereport" \
    -reporttypes:Html
xdg-open coveragereport/index.htm

