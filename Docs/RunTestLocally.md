## How to run test locally
The test suite of Chirp consists of 3 test folders each targeting their own part of the application, Infrastructure, Razor and playwright tests. All the tests are found in *Chirp/test/*


### Infrastructure.Tests
No prerequisites are needed to accomplish the infrastructure test, simply cd into the *Chirp/test.Chirp.Infrastructure.Tests/* folder in your terminal and
run 
  ```bash
  dotnet test
  ```

### Razor.Tests
To run the tests you need to setup and download docker. A complete guide for downloading and setting up docker correctly with our application can be found in our [README.md](..\README.md)
After following the guide cd into the Chirp.Razor.tests folder and run the following command
```bash
dotnet test
```

### Playwright.tests
To run the test first download playwright with the following command

  ```bash
  pwsh bin/Debug/net7.0/playwright.ps1 install
  ```
This install various browsers and tools to run UI tests. The browser we use is chromium based.
<br>
if you run in to issues with the version of .net replace net7.0 in the command with the correct version
<br>
if you don't have powerShell installed follow these instructions
[Install PowerShell](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.4)

Extra logic for authorization hasn't been implemented so the developer has to manually enter their github username into the variable at line 18 in the **PlaywrightTests.cs** file. Further explanation is also found there. After completing these steps you can run the test with: 
```bash 
dotnet test 
  ```
When you run the test a chromium based browser will open and the first step tries to login. Here the automation stops and the user has to login through github themselves. **No passwords are saved!** After this step is completed playwright will do the rest itself.
