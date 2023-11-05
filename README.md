# PINChat

## Summary:
- WebChat application using .NET Technology
- Hosted on private Kubernetes server using Argo CICD
- Using GitHub Actions to build Docker Image, tag it with new version and notify ArgoApps
  repository to update hosted application to latest version

### Projects:
1. PINChat.Api
   - https://github.com/anmaletic/PINChat.Api
2. PINChat.App
   - https://github.com/anmaletic/PINChat.App
3. PINChat.Data
   - https://github.com/anmaletic/PINChat.Data

### Project board:
- https://github.com/users/anmaletic/projects/6


## 1. PINChat.Api
- ##### Tech stack:
	- ASP.NET Core Web App (MVC) with Individual Identification
	- EntityFramework for connection with Authentication Database
	- Dapper for connection with Application Database
- ##### Summary:
	- Web application used for accessing database

## 2. PINChat.App
- ##### Tech stack:
	- ASP Hosted Blazor WebAssembly App
	- On server side used SignalR for ChatHub
- ##### Summary:
	- On client side progressive application used to connect to database through PINChat.Api
	  hosted on another location and to connect to ChatHub hosted on server side of application

## 3. PINChat.Data
- ##### Tech stack:
	- Visual Studio SQL project
	- MS SQL Server
- ##### Summary:
	- Project with all the tables and stored procedures for publishing to SQL Server
	  
