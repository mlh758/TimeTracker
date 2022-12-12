# TimeTrack

# Helpful Commands

## Set up the database

`SqlLocalDB.exe create TimeTrack`

Can reset with stop, delete, create.

## Create an initial migration to a sub directory
`dotnet ef migrations add InitialCreate -o Data/Migrations`

## Setup App User For Deployment

[Out of date, but seems to work](https://azure.microsoft.com/en-us/blog/adding-users-to-your-sql-azure-database/)
