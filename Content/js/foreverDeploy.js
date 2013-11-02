////Initializes the ArrowAlert application
//var foreverDeploy = angular.module('foreverDeploy', ['ngRoute', '$httpProvider',
//      'myHttpInterceptor', function ($routeProvider, $locationProvider) {

//    //configure custom routing
//    $routeProvider.when('/Dashboard', {
//        templateUrl: '/Dashboard/Dashboard',
//        controller: DashboardCtrl
//    });

//    $routeProvider.otherwise({
//        redirectTo: '/Dashboard'
//    });

//    $locationProvider.html5Mode(true);
//}]);
//Initializes the ArrowAlert application

var foreverDeploy = angular.module('foreverDeploy', ['ngRoute'])
.config(['$routeProvider',
      '$locationProvider', function ($routeProvider, $locationProvider) {

          //configure custom routing
          $routeProvider.when('/Dashboard', {
              templateUrl: '/Dashboard/Dashboard',
              controller: DashboardCtrl
          });

          $routeProvider.otherwise({
              redirectTo: '/Dashboard'
          });

          $locationProvider.html5Mode(true);
      }]);
