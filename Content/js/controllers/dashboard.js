function DashboardCtrl($scope) {
    $scope.pageTitle = "Dashboard";
    $scope.deployments = [];
    $scope.servers = [];

    //If connected, load the dashboard
    if ($scope.connected) {
        deploymentStatusHub.server.connect();
    }

    //Handle the deployment status update event 
    $scope.$on("DEPLOYMENT_STATUS_UPDATE", function (event, deployment) {
        var found = false;

        //Search through current deployments for the updated deployment
        $.each($scope.deployments, function (index, value) {
            if (value.commit != null && deployment.commit != null) {
                //Compare commit nodes (if present)
                if (value.commit.rawNode == deployment.commit.rawNode) {
                    $scope.deployments[index] = deployment;
                    found = true;
                }
            }
            else if (value.commit == null && deployment.commit == null) {
                found = true;
                $scope.deployments[index] = deployment;
            }
            else if (value.commit == null && deployment.commit != null) {
                found = true;
                $scope.deployments[index] = deployment;
            }
        });
        if (!found) {
            //Add new deployment to top
            $scope.deployments.unshift(deployment);

            //Prevent more than 3 deployments being shown at once
            if ($scope.deployments.length > 3) {
                //Wait until animation finishes, then remove from array
                //TODO: Listen for animation finish instead
                setTimeout(function () {
                    $scope.$apply(function () {
                        $scope.deployments.pop();
                    })
                }, 650);
            }
        }
    });

    //Handle the old deployments received event
    $scope.$on("OLD_DEPLOYMENTS_RECEIVED", function (event, deployments) {
        $scope.deployments = deployments;
    });

    //Handle the servers received event
    $scope.$on("SERVERS_RECEIVED", function (event, servers) {
        $scope.servers = servers;
    });

    //Handle the server status update event
    $scope.$on("SERVER_STATUS_UPDATE", function (event, server) {
        
        //Search through current servers for the updated server
        $.each($scope.servers, function (index, value) {
            if (value.name == server.name) {
                value = server;
            }
        });
    });
    
    //Helper function for determining dom elements visibility
    $scope.showElement = function (name, index) {
        return showElement(name, $scope.deployments[index].deploymentStatus);
    }

    //Helper function that returns the status bars color
    $scope.statusBar = function (index) {
        return statusBar[$scope.deployments[index].deploymentStatus];
    }
}

DashboardCtrl.$inject = ['$scope']
