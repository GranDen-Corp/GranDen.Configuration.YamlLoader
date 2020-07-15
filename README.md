# .NET Core configuration YAML help library

[![Build Status](https://dev.azure.com/GranDen-Corp/GranDen.Configuration.YamlLoader/_apis/build/status/GranDen-Corp.GranDen.Configuration.YamlLoader?branchName=dev)](https://dev.azure.com/GranDen-Corp/GranDen.Configuration.YamlLoader/_build/latest?definitionId=32&branchName=dev)

Provides two NuGet packages for easily use YAML(**.yaml**) file as .NET Core configuration source.

## GranDen.Configuration.YamlLoader

![Nuget](https://img.shields.io/nuget/v/GranDen.Configuration.YamlLoader?style=plastic)

ASP.NET Core Configuration Provider library for using YAML file to store settings, originally from [this](https://andrewlock.net/creating-a-custom-iconfigurationprovider-in-asp-net-core-to-parse-yaml/) article.

## GranDen.Configuration.YamlData.Binder

![Nuget](https://img.shields.io/nuget/v/GranDen.Configuration.YamlData.Binder?style=plastic)

Utility package for easier load .NET Core configuration from YAML into [Options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options) instance.
