## How to run test locally
The test suite of Chirp consists of 3 elements, Infrastructure, Razor and playwright test. 


### Testing Infrastructure.Tests
No prerequisites are needed to accomplish the infrastructure test, simply cd into the folder in your terminal and
run 
  ```bash
  dotnet test
  ```

### Testing Razor.Tests
To run the tests you need to setup and download docker as the

### Testing Playwright.tests
To run the test first download playwright with the following command 

  ```bash
  pwsh bin/Debug/net7.0/playwright.ps1 install
  ```

- The last step installs various browsers and tools to run UI tests.
- (In case the installed browsers are not in the version that Playwright expects, you have to make sure that PowerShell is installed in the right version and with a global scope, see e.g., https://github.com/microsoft/playwright-dotnet/issues/2006)
