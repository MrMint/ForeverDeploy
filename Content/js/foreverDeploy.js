//Initializes the ArrowAlert application
var foreverDeploy = angular.module('foreverDeploy', ['ngRoute'])
.config(['$routeProvider',
      '$locationProvider', function ($routeProvider, $locationProvider) {

          //configure custom routing
          $routeProvider.when('/Dashboard', {
              templateUrl: '/Dashboard/Dashboard',
              controller: DashboardCtrl
          });

          //configure custom routing
          $routeProvider.when('/GetStarted', {
              templateUrl: '/GetStarted/GetStarted',
              controller: GetStartedCtrl
          });

          $routeProvider.otherwise({
              redirectTo: '/Dashboard'
          });

          $locationProvider.html5Mode(true);
      }]);
