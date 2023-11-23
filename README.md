# Chirp
Project repository for BDSA course by:
- Lukas Andersson (Lukan)
- Oliver Asger-Sharp Johansen (oash) ITU email: oash@itu.dk, Public github: SkarpCat
- Marius Thomsen (Mariu) ITU email: mariu@itu.dk, Puplic git hup: AlbinoLoaf
- Anna Høybye Johansen (annaj)
- Niels Christian Skov Faber (nfab) ITU email: nfab@itu.dk, Puplic github: Faberen

## Co-author lines
Remember to put diamondbrackets around the email if they don't come on the copy<br />
<br />
Co-authored-by: Marius <mariu@itu.dk><br />
Co-authored-by: Oliver <oash@itu.dk><br />
Co-authored-by: Lukas <lukan@itu.dk><br />
Co-authored-by: Niels <nfab@itu.dk><br />
Co-authored-by: Anna <annaj@itu.dk><br />

## Versioning
Use this semantic for versioning on releases.
v1.2.3
- The first position is a breaking update, indicating that it is not safe to update to.
An examle is an breaking API change.
- The second position is a feature update, indicating that it is safe to update to.
- The third position is a bug fix, indicating that it is safe to update to.

## SlN
dotnet new sln
dotnet sln add src/Chirp.Razor/Chirp.Razor.csproj
dotnet sln add src/Chirp.Core/Chirp.Core.csproj
dotnet sln add src/Chirp.Infrastructure/Chirp.Infrastructure.csproj
dotnet sln add test/Chirp.Infrastructure.Test/Chirp.Infrastructure.Test.csproj
dotnet sln add test/Chirp.Razor.Test/Chirp.Razor.Test.csproj

# Running the program

## Command prompt version
Running the program in the terminal
- Enter the correct folder to where program.cs is
- dotnet run <br />
<br />
To read all files type: dotnet run read <br />
To make a cheep type: dotnet run cheep "message" <br />
To get help, type: dotnet run help <br />
<br />

Running the programs tests <br />
- Enter the correct folder for the type of test
- dotnet test<br />

# Running migrations

Install tool (only once per user)
  dotnet tool install --global dotnet-ef

Add package (only once per project)
  dotnet add package Microsoft.EntityFrameworkCore.Design

Make sure to delete all migations files.
Change directory to Chirp/src/Chirp.Infrastructure.
  dotnet ef migrations add InitialCreate
  dotnet ef database update

# Docker setup

## Guide
To setup the Docker container for development on own pc you need to run the following command:
```docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Admin123" -p 1433:1433 --name chirpdb --hostname chirpdb -d mcr.microsoft.com/mssql/server:2022-latest```
<br />
After this the Container should have been created and a new Image can be seen in your Docker Desktop app. With the new lines of code in Program.cs it should create the database on the container. We can all just use the same command since the connectionstring is already made for this password. hostname and port.

## What is that command
In this section i will give a brief rundown of what is going on in the command. <br />
```docker run``` Is something i believe you know since this is what starts the command. <br />
```-e "ACCEPT_EULA=Y"``` Is here to accept som agreements this is nice since we can just accept through command line. <br />
```-e "MSSQL_SA_PASSWORD=Admin123"``` This is here to make a password. <br />
```-p 1433:1433``` This is the port it will be hosted on. It will be localhost:1433. <br />
```--name chirpdb --hostname chirpdb``` This is the name for the Container. <br />
```-d mcr.microsoft.com/mssql/server:2022-latest``` This is the more important part. This is the Image that the Container is based upon which in our cas is going to be MsSQL (SQL Server).