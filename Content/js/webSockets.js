
var deploymentStatusHub = $.connection.deploymentStatusHub;

deploymentStatusHub.client.updateDeployment = function (deployment) {
    broadcastAngularEvent("DEPLOYMENT_STATUS_UPDATE", deployment);
};

deploymentStatusHub.client.oldDeployments = function (deployments) {
    broadcastAngularEvent("OLD_DEPLOYMENTS_RECEIVED", deployments);
};

$.connection.hub.start().done(function () {
    deploymentStatusHub.server.connect();
    broadcastAngularEvent("WEBSOCKETS_CONNECTED");
}); 