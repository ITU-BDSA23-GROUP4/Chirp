## Build, test, release, and deployment
<!-- Illustrate with a UML activity diagram how your Chirp! applications are build, tested, released, and deployed. That is, illustrate the flow of activities in your respective GitHub Actions workflows. -->

<!-- Describe briefly the illustration, i.e., how you application is built, tested, released, and deployed. -->
### Github workflows
In this section we will go over the github workflows and how they work and  use for the flow of the project. This will also entail when the workflows are activated and used.
#### Build and Test
The build and test work flow can be found on (Appendix?). Here we can see an activity diagram that shows how the github make sure what is merged into main. This workflow is ran on a pull request every time a commit is being made to the branch in the pull request. This is to ensure that the main will still work by building the project with dotnet and the test that has been made for the project. By running the test as well will make sure that if the new changes that the new branch is adding still work with the code already on main. 
#### Publish and release
This workflow is made to automate the creation of a github release when a tag is added (Appendix?) it will next time create a release of the tag. But first the workflow builds a version for Windows, MacOS and Linux. After that it will zip the files and adding them to the release if a release was made. 
#### Build and deploy
This workflow can be seen her (Appendix?). The workflow is made so it will build the program and runs the publish command to build a version for linux to be ran on the azure wep app. After the publish part it uploads the artifacts so the next job can use the files. The deploy job will the download the artifact and use the files to deploy to our azure web app.
<br>
...............................................
<br>
<!-- Before putting anything into the workflow actions, we create test manually to run on the computer with the "dotnet test" command. There has been created an activity diagram showing this. For most test we try to implement it going how we expect the method or feature to behave, and after we've concluded that it works, we create a test to challenge this method. By example we can look at the Create(CreateCheepDTO)'s tests in the unit tests. <br> -->
<!-- This can be found in the infrastructure tests in the tests for Cheep Repository.  -->
<!-- We start by testing that what we want it to will work, and then we challenge it, by giving it some input that should throw validation exceptions. When we know both of these will pass, we can then move onto the workflows.  -->