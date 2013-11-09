//Initializes the foreverDeploy application
var foreverDeploy = angular.module('foreverDeploy', ['ngRoute'])
.config(['$routeProvider',
      '$locationProvider', function ($routeProvider, $locationProvider) {

          //configure custom routing
          //Dashboard
          $routeProvider.when('/Dashboard', {
              templateUrl: '/Dashboard/Dashboard',
              controller: DashboardCtrl
          });

          //GetStarted
          $routeProvider.when('/GetStarted', {
              templateUrl: '/GetStarted/GetStarted',
              controller: GetStartedCtrl
          });

          //Reroute bad request to Dashboard
          $routeProvider.otherwise({
              redirectTo: '/Dashboard'
          });

          $locationProvider.html5Mode(true);
      }]);
