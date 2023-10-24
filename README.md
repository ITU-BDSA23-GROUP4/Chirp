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

dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate
dotnet ef database update
