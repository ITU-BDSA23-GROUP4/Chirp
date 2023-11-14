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

## Wednesday 20-09-2023
Resolved problem and finished work on Unit test branch.
Merged it into development, with only minor conflicts to the data csv file and Program.cs where the ConstructCheep method had been refactored on the incomming changes. The incomming changes were resolved.

### Added issues
The tests should be refactored to create, use and delete their own temporary data files, within the tests project folder. See [Issue #20](/../../issues/20)



## Wednesday 4-10-2023
The SQLite and Razor page created yesterday has been merged into development.
DEVELOPMENT IS NOW UP TO DATE WITH ALL NEW CHANGES TO BOTH SQLite and Razorpage.

All unecessary branhces (that is all other branches than main, chirp_cli and development) has been deleted, except SQLiteImplementation and web_app. Theese should only be kept until develpoment is in a working stage (acorrding to requirement and issues), and not be updated with new work.

All new work, should be done at new short lived branches, created from development.

Project folders and files (except tests, theese are yet to come) regarding the chirp_cli application, is now deleted in development.
That is, development is now cleaned up to only a raazor page and sqlite application. This provides a better overview, in sense of actual code used. The deletion and merging of old branches, so that all new changes resides in develop, provides a better overview in what is the correct version of our code, to work on forward.

### Regarding the old chirp_cli.
Three issues have been closed but not resolved, regarding the CLI application.
Since we have moved forward to a razor page version of the chirp application, the Teaching assistents have advised us not to work on them any more.

They reside in the column "Related to CLI application".
The three issues are: [Issue #23](/../../issues/23), [Issue #24](/../../issues/24), [Issue #27](/../../issues/27)

### Next assignemnts
Theese issues is in progress and should be finished before tomorrow (Thursday 5-10-2023):
- [Issue #39](/../../issues/39)
- [Issue #35](/../../issues/35)
- [Issue #32](/../../issues/32)
- [Issue #34](/../../issues/34)
- [Issue #36](/../../issues/36)

Theese issues is todo and should be finished before tomorrow (Thursday 5-10-2023) or at latests friday 6-10-2023
- [Issue #40](/../../issues/40)
- [Issue #33](/../../issues/33)
- [Issue #37](/../../issues/37)
