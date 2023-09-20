# Logbog
## Tuesday 12-09-2023
Met at 10:00 am. <br />
Jobs of the day:
1. Merge all our work
2. Fix the issues that appear
3. work on remaining issue
4. plan for next lecture/assignments

### Issues resolved 
[Issue #2](/../../issues/2), [Issue #4](/../../issues/4), [Issue #5](/../../issues/5), [Issue #3](/../../issues/3)<br />
We wanted to move the record cheeps, for our concerns. <br />
There was a lot of reference issue, to get a hold on the database, which is a different project. <br />
Working on 2c - we had issues. We split up in to, working on it with some getting help from TA's, and others continue working in the group, to find a solution. <br />
We agreed to be more specific with naming conventions. <br />

### Next assignments
- Lukas will work on database
- Anna will clean up code (add comments, refactor)





## Tuesday 19-09-2023 
Met at 10:00 am. <br />
Jobs of the day:
1. Merge all done branches (singleton and EndToEnd) - no issues appeared
2. Refactoring cheeps to its own file
3. Life compiler fix (compiler dont work)
4. Finish unit and integration test
5. Merge everything into main
6. Make workflow for publish

### Issues resolves
[Issue #12](/../../issues/12), [Issue #10](/../../issues/10), [Issue #9](/../../issues/9), [Issue #7](/../../issues/7), [Issue #8](/../../issues/8) <br />
(1) - There were no issues when merging singleton and EndToEnd into development. When working with tests, we had issues when getting a hold of the database, in the endToEnd test, we struggles to get the chirp.dll file and the dotnet file, so that it would be used globally, and not only depend on the single computer.  <br />
(2) - When merging the refactored cheeps branch into the other, the layout of the dates where changes, giving it a few complications. We solves this by setting a default culture to the date and time, making it be in the same format across devices. <br />
(3) - The main soltuion file in the root folder of the project did not contain the refrences to each project file. Furthermore a second solution file existed in the SimpleDB folder, all contributing to VS code and Visual Studio not being able to load the projects properly. The refrences has now ben added and the second solution file has been deleted. <br /> 
(4) -  The first integration test is done and runs correctly. It checks that the ConstructCheep method creates a cheep which then calls SaveToFile method form the dabatase. The method ReadFromFile is then used to confirm that the newest cheep matches the one written in the test. Another Integration test is planned to check if an exception gets thrown when trying to write a cheep that isn't compatiable with the program. Although for now a user in only able to cheep a String and there are no constraints on how a Cheep message should look like, other that it being a string so therefore a exception for now can't be raised. We are discussing wether to implement checks for now. <br />
(5) -  <br />

### Next assignments
- Marius will finish Unit test
- Oliver work on the workflow for publish of executable file
