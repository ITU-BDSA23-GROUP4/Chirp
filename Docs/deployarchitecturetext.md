## Architecture of deployed application
In the following figure a deployment diagram can be seen of our Chirp application.

![Deploy diagram](Images/DeploymentDiagram.png)

Chirp is a client-server application hosted on the Azure app service as a Web App. The web app is connected to a Azure SQL server where the database can be found. Furthermore the application makes use of a Azure AD B2C tenant for user-authentication. The different nodes means of communication is represented in the diagram.