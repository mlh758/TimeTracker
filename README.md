# PracTrack

Allows users to track their clinical hours (activities) against clients and groups that are composed of clients. There is no cross-user functionality within
the application so everything is built around the user. The goal is the help users complete their AAPIC internship applications and later licensure.

The `Shared` project contains ViewModels which are how the `Server` and `Client` communicate.

## General Structure

### Client

The `Client` project holds the front end. This is all written in Blazor WASM with a small JS bridge to do Webauthn login. The `Pages` directory should have anything
that is actually routed to. `Components` is any reuseable element or building block for a page. `Services` is non-rendering tools to support other functionality.

### Server

The entrypoint in the data model is the `User` model. Users are assigned credentials for Webauthn login, and create clients to log clinical time with.

`Client` is the model that all other functionality revolves around. The demographic information is there to build up reports on what kind of clinical services
the user was performing during their practicum hours in school. Users can participate in activities directly, or as a part of a group.

`Category` and `CustomCategory` provide the values for the various demographic attributes of a client and are the entry point to any reporting. Users will want to
ask questions like "How many hours have I worked with disabled patients?".

`Assessments` are another descriptor for clinical activities. They are tied to the specific activity rather than the client since different sessions may perform
different assessments. This list is pulled from an online dataset.

`ActivityGrouping` categorizes activity. Not all activity is done with a client or a group. Supervision would be an example of this so not all activity is going to
have those models associated with it.

## Helpful Commands

### Set up the database

`SqlLocalDB.exe create TimeTrack`

Can reset with stop, delete, create.

### Create an initial migration to a sub directory
`dotnet ef migrations add InitialCreate -o Data/Migrations`

### Setup App User For Deployment

[Deployment setup](https://learn.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?tabs=azure-portal%2Cvisualstudio-deploy%2Cdeploy-instructions-azure-portal%2Cazure-portal-logs%2Cazure-portal-resources) which
seems more recent.

### Migrating the app

There's no fancy CI yet, so once you're ready to deploy a migration get the connection string from Azure App Service and open up Powershell:

```
dotnet ef database update --connection "<paste in the connection string>"
```
