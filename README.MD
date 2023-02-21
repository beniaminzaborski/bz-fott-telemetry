# Feet On The Tarmac - Telemetry
![Project logo](./doc/fott-logo.png)

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)

## General info
This is "Feet On The Tarmac" telemetry project. It is responsible for grab competitors times from checkpoints.
	
## Technologies
Project is created with:
* .NET 6
* Azure Function
* Azure Event Hubs
* Azure Service Bus
* Azure Application Insights

## Setup
To run this project:
* Build the project

```
dotnet build Bz.Fott.Telemetry.csproj
```

* Start application

```
dotnet run Bz.Fott.Telemetry.csproj
```
