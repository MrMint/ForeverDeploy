function MainCtrl($scope) {
    $scope.connected = false;
    $scope.loading = true;
    $scope.pageTitle = "Connecting";
    $scope.$on("WEBSOCKETS_CONNECTED", function (event) {
        $scope.connected = true;
    });

    $scope.$on("LOADING", function (event, value) {
        $scope.loading = value;
    });


    //Helper function for safely broadcasting events from outside js
    $scope.broadcastEventSafe = function (eventType, value) {
        var phase = this.$root.$$phase;
        if (phase == '$apply' || phase == '$digest') {
            $scope.$broadcast(eventType, value);
        } else {
            this.$apply(function () {
                $scope.$broadcast(eventType, value);
            });
        }
    }

}
MainCtrl.$inject = ['$scope']