<!-- Figugres are refered to as SQDX as in Sequence Diagram X -->
## Sequence diagram
In Figure SQD1. A sequence diagram of an unauthorized actor. Henceforth, referred to as UA, accessing our project. It shows the UA sending the HTTP get request to receive the website. After the initial request, the Chirp.Razor starts to build the HTML. Here, an asynchronous object creation message is sent through the interface in the core and onto the repository. The repository returns the same for all actors sending this request. Using Linq, the repository inquires the SQL database for the 32 most recent cheeps. 

The database sends the 32 cheeps to the repository. Which inserts each cheep into a CheepDTO before returning a list of 32 CheepDTOs. This list is sent back through the system, shown in Fig SQD1. Arriving in Chirp.Razor. It is weaved into the HTML, checking the if the user is Authorized. Before the page is returned to the UA. 
<figure align = "center">
    <img title="Sequence Diagram Unauthorized" style="width:70%" alt="Alt text" src="Images\SequenceDiagramUnauthorized.svg">
    <figcaption style="  font-size:11px"><b>Fig. SQD1 - Sequence diagram for an unauthorized user </b></figcaption>
</figure>


Figure SQD2. Show a known actor accessing our site, logging in and sending a Cheep. The first Get request is the same as seen in Fig SQD1. It deviates during the authentication step as the actor presses the login link. As they log in, Microsoft Identity redirects them to Azure OIDC. Which then redirect to GitHub. 

After the actor has logged in, GitHub sends a token back to being logged on Azure. Their token is in the URL. With it confirmed, the Razor page HTML Will change. 

Then the authorized user fills out the desired cheep and Chirps it. When that happens, Chirp.Razor constructs a CheepDTO and sends it through the core, where it is validated and sent to the repository. Afterwards it is committed to the database granted that Validation confirms. 

Then, confirmation of success is sent back until the razorpage redirects to itself to reload. 
<figure align = "center">
    <img title="Sequence Diagram Authorized" style="width:70%" alt="Alt text" src="Images\SequenceDiagramAuthorized.svg">
    <figcaption style="  font-size:11px"><b>Fig. SQD2 - Sequence diagram for an unauthorized user </b></figcaption>
</figure>

