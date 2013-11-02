function DashboardCtrl($scope, $location, $http) {
    $scope.pageTitle = "Dashboard";
    $scope.deployments = [];
    
    $scope.$on("DEPLOYMENT_STATUS_UPDATE", function (event, deployment) {
        $scope.deployments.push(deployment);
    });
}


