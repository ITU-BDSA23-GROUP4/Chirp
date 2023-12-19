-- This is the whole paragraph for architecture:


In the Onion Architecture diagram, we can see how ours are presented. In the middle we have our core package, that is our DTO's and our interfaces. This is the lowest layer of the application. Then we move outwards for the more advanced layers, and we end with our SQL-Server and our razor pages, that is what interact with our Azure application. 
--OnionArchitectureDiagram.png      Image text: Onion diagram of each layer in the application

These layers can be better seen in our class diagram, there's one made for each package, they show everything that is needed to know about our classes. We've chosen to do this for the packages, for more simplicity in reading the diagrams. The package diagrams shows for each package, how the classes interact with each other, to see how they interact with the other layers, see OnionClassDiagram.png(FIGURE SOMETHING??).

You will see in our repositories, that we're deleting the author at some point, this was a project demand. We had two possibilities; delete the user in the sense that they will no longer be traceable, that is make everything anonymous and delete their information, or we had the possibility of just deleting everything that the user ever touched. We chose to be sure that the user wouldn't come back complaining that their username/normal name still was in a cheep, so we deleted everything that they touched. This was also the more easy approach, since it just came down to us deleting everything that contained that users id or name. 
-- package diagrams      Image text for each one: Internal interaction in PACKAGE

The Onion Architecture (or better known as Clean Architecture), is great for having low coupling and high cohesion. When looking at the UML in the more specified onion diagram, there is no unnecessary communication between scripts, with so low coupling the readability of the program is better, even though some of the repositories contains a fair amount of methods. You will se the Core folder doesn't use any types/method from the other packages, this is why its in the middle of the onion. This applies to the next level using Infrastructure. This folder only takes information from itself or from the Core, and the same does the Razor.

It is worth mentioning that the only way we're interacting with the repositories is through their interfaces, which is an important factor in keeping the application with low coupling. The same goes for the CheepService, nothing interferes with it, since every class that needs to just uses information from its interface, and that interface uses from the other interfaces. 
-- OnionClassDiagram.png    Image text: Shows our classes in the onion diagram and how the layers interact with each other
