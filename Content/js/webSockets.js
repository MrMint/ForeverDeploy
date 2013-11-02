
$.connection.deploymentStatusHub.logging = true;
var deploymentStatusHub = $.connection.deploymentStatusHub;
deploymentStatusHub.client.updateDeployment = function (deployment) {
    broadcastAngularEvent("DEPLOYMENT_STATUS_UPDATE", deployment);
};

$.connection.hub.start().done(function () {
    deploymentStatusHub.server.connect();
});