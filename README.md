# webappprojectv4
This is my resume! It's a static website hosted on Azure Storage with a visitor counter powered by .NET and Azure Functions. Developed with HTML, CSS, and JavaScript; it features CI/CD workflows managed via GitHub Actions and integrates Azure services like Blob Storage and CosmosDB for a serverless web app with a dynamic database. 

If you'd like to create your own, here is the Youtube tutorial I took inspriation from [ACloudGuru:BuildYourResume](https://youtu.be/ieYrBWmkfno?si=iuMKmuw_OTyR2v70)

> [!IMPORTANT]
Please note this tutorial was published in May 2021 and the outdated aspects will require some troubleshooting. 

## Demo
[View it live here](https://www.ellamaggs.com/)

## Structure
- 'frontend/': Folder contains HTML for website. 
    - 'main.js': Folder contains visitor counter JavaScript. 
- 'backend/': Folder defines Azure Function for storage in CosmosDB.
    - 'api/': Folder Contains the dontnet API deployed on Azure Functions.
    - 'Counter.cs': Contains the visitor counter code. 
- 'github/workflows/': Folder contains CI/CD workflow configurations. 

## Architecture
! [Architecture Diagram](EMWebAppArchitectureDiagram.drawio)

## Deployment (Explain CICD, Github Actions and component stages)
## Troubleshooting
## Roadmap
- 




