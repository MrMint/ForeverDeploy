function MainCtrl($scope, $location, $rootScope, $http) {

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