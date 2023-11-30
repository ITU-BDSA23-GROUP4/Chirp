---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2023 Group `4`
author:
- "Anna Høybye Johansen <annaj@itu.dk>"
- "Lukas Andersson <lukan@itu.dk>"
- "Marius Thomsen <mariu@itu.dk>"
- "Niels Christian Skov Faber <nfab@itu.dk>"
- "Oliver Asger-Sharp Johansen <oash@itu.dk>"
numbersections: true
---

# Design and Architecture of _Chirp!_

## Domain Model
Provide an illustration of your domain model. Make sure that it is correct and complete. In case you are using ASP.NET Identity, make sure to illustrate that accordingly.
![Illustration of the _Chirp!_ data model as UML class diagram.](docs/images/domain_model.png)

## Architecture — In the small
Illustrate the organization of your code base. That is, illustrate which layers exist in your (onion) architecture. Make sure to illustrate which part of your code is residing in which layer.
<br>
...............................................
<br>
In the Onion Architecture diagram, we can see how ours are presented. In the middle we have our core package, that is our DTO's and our interfaces. This is the lowest layer of the application. If you look at the UML of our whole application, you will se the Core folder doesn't use any types/method from the other packages, this is why its in the middle of the onion. This applies to the next level using Infrastructure. This folder only takes information from itself or from the Core, and the same does the Razor.

The Onion Architecture (or better known as Clean Architecture), is great for having low coupling and high cohesion. When looking at the UML, there is no unnessesary communication between scripts, with so low coupling the readability of the program is better, even though some of the repositories contains a fair amount of methods.

## Architecture of deployed application
Illustrate the architecture of your deployed application. Remember, you developed a client-server application. Illustrate the server component and to where it is deployed, illustrate a client component, and show how these communicate with each other.

## User activities
Illustrate typical scenarios of a user journey through your Chirp! application. That is, start illustrating the first page that is presented to a non-authorized user, illustrate what a non-authorized user can do with your Chirp! application, and finally illustrate what a user can do after authentication.

Make sure that the illustrations are in line with the actual behavior of your application.

## Sequence diagram
With a UML sequence diagram, illustrate the flow of messages and data through your _Chirp!_ application.
Start with an HTTP request that is send by an unauthorized user to the root endpoint of your application and end with the completely rendered web-page that is returned to the user.

Make sure that your illustration is complete.
That is, likely for many of you there will be different kinds of "calls" and responses.
Some HTTP calls and responses, some calls and responses in C# and likely some more.
(Note the previous sentence is vague on purpose. I want that you create a complete illustration.)

# Process
## Build, test, release, and deployment
Illustrate with a UML activity diagram how your Chirp! applications are build, tested, released, and deployed. That is, illustrate the flow of activities in your respective GitHub Actions workflows.

Describe briefly the illustration, i.e., how you application is built, tested, released, and deployed.
<br>
...............................................
<br>
Before putting anything into the workflow actions, we create test manually to run on the computer with the "dotnet test" command. There has been created an activity diagram showing this. For most test we try to implement it going how we expect the method or feature to behave, and after we've concluded that it works, we create a test to challenge this method. By example we can look at the Create(CreateCheepDTO)'s tests in the unit tests. 
This can be found in the infrastructure tests in the tests for Cheep Repository. 
We start by testing that what we want it to will work, and then we challenge it, by giving it some input that should throw validation exceptions. When we know both of these will pass, we can then move onto the workflows. 

## Team work
Show a screenshot of your project board right before hand-in. Briefly describe which tasks are still unresolved, i.e., which features are missing from your applications or which functionality is incomplete.

Briefly describe and illustrate the flow of activities that happen from the new creation of an issue (task description), over development, etc. until a feature is finally merged into the main branch of your repository.

## How to make _Chirp!_ work locally
There has to be some documentation on how to come from cloning your project to a running system. That is, Rasmus or Helge have to know precisely what to do in which order. Likely, it is best to describe how we clone your project, which commands we have to execute, and what we are supposed to see then.

## How to run test suite locally
List all necessary steps that Rasmus or Helge have to execute to execute your test suites. Here, you can assume that we already cloned your repository in the step above.

Briefly describe what kinds of tests you have in your test suites and what they are testing.

# Ethics
## License
State which software license you chose for your application.

## LLMs, ChatGPT, CoPilot, and others
State which LLM(s) were used during development of your project. In case you were not using any, just state so. In case you were using an LLM to support your development, briefly describe when and how it was applied. Reflect in writing to which degree the responses of the LLM were helpful. Discuss briefly if application of LLMs sped up your development or if the contrary was the case.
