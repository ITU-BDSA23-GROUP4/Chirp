-- This is the whole paragraph for architecture:


In the Onion Architecture diagram bellow you'll see our applications. In the centre we have our core package. This is the lowest layer of the application. Then we move outwards for the layers with a larger impact, and end with our SQL-Server and razor pages, which interacts with our Azure application. 
<br>
![OnionArchitectureDiagram](Images\OnionArchitectureDiagram.png)

These layers can be seen more detailed in our class diagram. There is one for each package, and they show everything needed to know about our classes. We've chosen to do this for more simplicity in reading the diagrams. The diagrams show each package and how the classes interact with each other. To see how they interact with the other layers, see [OnionClassDiagram](Images\OnionClassDiagram.png) further down.

You will see in our repositories, that we're deleting the author at some point, this was a project demand. We had two possibilities; delete the user in the sense that they will no longer be traceable, that is make everything anonymous and delete their information, or we had the possibility of just deleting everything that the user ever touched. We chose to be sure that the user wouldn't come back complaining that their username/normal name still was in a cheep, so we deleted everything that they touched. This was also the easier approach since we could delete everything that contained that user's id or name, instead of altering everything. 
<br>
![PackageCoreUMLDiagram](Images\PackagesUMLClassDiagrams\PackageCoreUMLDiagram.png)
![PackageInfrastructureUMLDiagram](Images\PackagesUMLClassDiagrams\PackageInfrastructureUMLDiagram.png)
![PackageRazorUMLDiagram](Images\PackagesUMLClassDiagrams\PackageRazorUMLDiagrams.png)
![PackagePagesUMLDiagram](Images\PackagesUMLClassDiagrams\PackagePagesUMLDiagram.png)

The Onion Architecture (otherwise known as Clean Architecture), is great for having low coupling and high cohesion. When looking at the UML in the more specified onion diagram bellow, there is no unnecessary communication between scripts, having low coupling making the readability of the program better, even though some of the repositories contain a fair amount of methods. When moving outward you'll see the packages only use entities further in or in the same layer.

It is worth mentioning that the only way of interacting with the repositories is through their interfaces, which is an important factor in making sure the application has low coupling. The same goes for the CheepService, since every class that needs to access it uses information from the interface, and that interface uses from the other interfaces. 
<br>
![OnionClassDiagram](Images\OnionClassDiagram.png)