## Sequence diagram
as ilustarted in the sequence diagram an actor tries to connect to the website they send a get request to our razor page. Here it access its interface requesting cheepDTO's. The interface in the core calls to the repository which in turn calls to the SQL server hosted on azure. Afterwards the 32 cheeps are returned.

As the Cheeps are returned they are turned into Cheep dto ensuring that only the neccesary data acompanies them. They pass through the repository that manipulates them to the appropriet form and sends the set of 32 CheepDTOs' to the Chirp.Razor package.  



Make sure that your illustration is complete.
That is, likely for many of you there will be different kinds of "calls" and responses.
Some HTTP calls and responses, some calls and responses in C# and likely some more.
(Note the previous sentence is vague on purpose. I want that you create a complete illustration.)
<!-- Not sure it is "complete" -->