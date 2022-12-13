# TimeTrack

# Helpful Commands

## Set up the database

`SqlLocalDB.exe create TimeTrack`

Can reset with stop, delete, create.

## Create an initial migration to a sub directory
`dotnet ef migrations add InitialCreate -o Data/Migrations`

## Setup App User For Deployment

[Deployment setup](https://learn.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?tabs=azure-portal%2Cvisualstudio-deploy%2Cdeploy-instructions-azure-portal%2Cazure-portal-logs%2Cazure-portal-resources) which
seems more recent.

## Migrating the app

There's no fancy CI yet, so once you're ready to deploy a migration get the connection string from Azure App Service and open up Powershell:

```
dotnet ef database update --connection "<paste in the connection string>"
```