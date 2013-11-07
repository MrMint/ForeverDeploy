var deploymentStatusHub = $.connection.deploymentStatusHub;

//Handle receiving a deployment status update
deploymentStatusHub.client.updateDeployment = function (deployment) {
    broadcastAngularEvent("DEPLOYMENT_STATUS_UPDATE", deployment);
};

//Handle recieving old deployments
deploymentStatusHub.client.oldDeployments = function (deployments) {
    broadcastAngularEvent("OLD_DEPLOYMENTS_RECEIVED", deployments);
};

//Handle receiving a server status update
deploymentStatusHub.client.updateServer = function (server) {
    broadcastAngularEvent("SERVER_STATUS_UPDATE", server);
};

//Handle receiving list of servers
deploymentStatusHub.client.servers = function (servers) {
    broadcastAngularEvent("SERVERS_RECEIVED", servers);
};

//Connect to the hub
$.connection.hub.start().done(function () {
    //On connection, request deployments/servers
    deploymentStatusHub.server.connect();
    broadcastAngularEvent("WEBSOCKETS_CONNECTED");
}); 