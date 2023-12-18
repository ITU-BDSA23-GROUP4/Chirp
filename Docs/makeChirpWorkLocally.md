There has to be some documentation on how to come from cloning your project to a running system. That is, Rasmus or Helge have to know precisely what to do in which order. Likely, it is best to describe how we clone your project, which commands we have to execute, and what we are supposed to see then.


# How to make Chirp! work locally

## 1. Clone the repository
Follow this link: [github.com/ITU-BDSA23-GROUP4](https://github.com/ITU-BDSA23-GROUP4/Chirp.git)
<br>
![cloning](Images\cloning.png)

copy the url and run the following command in your terminal where you want to clone the repository to.
```bash
git clone https://github.com/ITU-BDSA23-GROUP4/Chirp.git
```

## 2. Running and installing migrations
naviate to the root folder of the program, run the following command in your terminal.
```bash
--global dotnet-ef
```
naviagte to *Chirp/src/Chirp.Infrastructure*
<br> delete all migrations file if they exists
<br>

![cloning](Images\deleteMigations.png)
<br>

then run the following command

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
## 3. Setting up docker
There is a complete guide to docker in our [README.md](..\README.md#docker-setup)


## 4. Running the program
navigate to *src\Chirp.Razor* and run the following command
```bash
dotnet run
```
