ForeverDeploy
==============
A web-based deployment tool.

----
## What is ForeverDeploy?
ForeverDeploy is a web-based deployment tool, used to ease the deployment process for developers.
It integrates with your standard git work-flow via commit messages.  Simply add #deploy to your commit message, and ForeverDeploy will take it from there.

----
## How to use/How it works
[Click here to find out!](http://foreverdeploy.jaredprather.com/GetStarted)

----
## Current repositories supported:
* Bitbucket

----
## Current builders supported:
* MsBuild

----
## Code Links:
Server:

* [Deployment Manager](https://bitbucket.org/Mr_Mint/foreverdeploy/src/master/Utilities/DeploymentManager.cs?at=master)
* [Server Status Manager](https://bitbucket.org/Mr_Mint/foreverdeploy/src/master/Utilities/ServerStatusManager.cs?at=master)
* [POST Endpoint](https://bitbucket.org/Mr_Mint/foreverdeploy/src/master/Controllers/GitHooksController.cs?at=master)
* [Git utilities](https://bitbucket.org/Mr_Mint/foreverdeploy/src/master/Utilities/GitUtilities.cs?at=master)
* [Deployment utilities](https://bitbucket.org/Mr_Mint/foreverdeploy/src/master/Utilities/DeploymentUtilities.cs?at=master)
* [WebSocket broadcaster](https://bitbucket.org/Mr_Mint/foreverdeploy/src/master/Hubs/Broadcaster.cs?at=master)

Client:

* [Application init/router](https://bitbucket.org/Mr_Mint/foreverdeploy/src/master/Content/js/foreverDeploy.js?at=master)
* [Layout template](https://bitbucket.org/Mr_Mint/foreverdeploy/src/master/Views/Shared/Layout.cshtml?at=master)
* [Deployment template](https://bitbucket.org/Mr_Mint/foreverdeploy/src/master/Views/Shared/_DeploymentTemplatePartial.cshtml?at=master)
* [Server Status template](https://bitbucket.org/Mr_Mint/foreverdeploy/src/master/Views/Shared/_ServerStatusTemplatePartial.cshtml?at=master)
* [Controllers](https://bitbucket.org/Mr_Mint/foreverdeploy/src/master/Content/js/controllers?at=master)
* [WebSocket handlers](https://bitbucket.org/Mr_Mint/foreverdeploy/src/master/Content/js/webSockets.js?at=master)

----
##Want to contribute?
Fork the repo and create pull requests!