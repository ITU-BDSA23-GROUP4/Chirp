## Build, test, release, and deployment
<!-- Illustrate with a UML activity diagram how your Chirp! applications are build, tested, released, and deployed. That is, illustrate the flow of activities in your respective GitHub Actions workflows. -->

<!-- Describe briefly the illustration, i.e., how you application is built, tested, released, and deployed. -->
### GitHub workflows
To ensure the flow of the project, we use a tool developed by GitHub known as. GitHub Action, otherwise known as workflow. This will also entail when the workflows are activated and used.
#### Build and Test
The build and test workflow can be found in (appendix?). The activity diagram shows how GitHub ensures what is merged into main. This workflow is run on a pull request every time a commit is made to the branch in the pull request. This is to ensure that main will still work by building the project with dotnet and tests made for the project.
 Because it runs the tests as well, it ensures that any incoming changes do not affect the functionality. If anything fails, it will stop and prevent the branch from merging into main.
#### Publish and release
This workflow is made to automate the creation of a GitHub release when a tag is added (Appendix?). It will create a release of the tag. But first, the workflow builds a version for Windows, MacOS and Linux. After that, it will zip the files and add them to the release if a release was made. 
#### Build and deploy
This workflow can be seen here (Appendix?). The workflow is made so it will build the program and run the "publish" command to build a version for Linux to be run on the Azure web app. After the publish command, it uploads the artifacts so the next job can use the files. The deploy job will download the artefact and use the files to deploy to our Azure web app.
<br>
...............................................
<br>
<!-- Before putting anything into the workflow actions, we create test manually to run on the computer with the "dotnet test" command. There has been created an activity diagram showing this. For most test we try to implement it going how we expect the method or feature to behave, and after we've concluded that it works, we create a test to challenge this method. By example we can look at the Create(CreateCheepDTO)'s tests in the unit tests. <br> -->
<!-- This can be found in the infrastructure tests in the tests for Cheep Repository.  -->
<!-- We start by testing that what we want it to will work, and then we challenge it, by giving it some input that should throw validation exceptions. When we know both of these will pass, we can then move onto the workflows.  -->