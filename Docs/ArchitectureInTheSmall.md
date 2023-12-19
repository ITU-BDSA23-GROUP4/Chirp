-- This is the whole paragraph for architecture:


In the Onion Architecture diagram, we can see how ours are presented. In the middle we have our core package, that is our DTO's and our interfaces. This is the lowest layer of the application. If you look at the UML of our whole application, you will se the Core folder doesn't use any types/method from the other packages, this is why its in the middle of the onion. This applies to the next level using Infrastructure. This folder only takes information from itself or from the Core, and the same does the Razor.
--OnionClassDiagram.drawio

These layers can be better seen in our class diagram, there's one made for each package, they show everything that is needed to know about our classes (to see how they work together look at the next paragraph). We've choosen to do this for the packages, for more simplicity in reading the diagrams.

The Onion Architecture (or better known as Clean Architecture), is great for having low coupling and high cohesion. When looking at the UML in the more specified onion diagram, there is no unnessesary communication between scripts, with so low coupling the readability of the program is better, even though some of the repositories contains a fair amount of methods.
-- NewOnionClassDiagram.drawio (Not made yet??)
