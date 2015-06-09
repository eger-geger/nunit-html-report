var angular = require('angular');

var application = angular.module('NUnitReport', [
    require('angular-route'),
    require('angular-animate'),
    'ui.bootstrap',
    'angular.backtop'
]);

application.factory('Report', require('./services/ReportFactory'));
application.service('ReportAdapter', require('./services/ReportAdapter'));
application.service('StateStorage', require('./services/StateStorage'));
application.controller('DashboardController', require('./controllers/DashboardController'));

application.config(['$routeProvider',
    function($routeProvider) {
        $routeProvider.when('/testcase/:id', {
            templateUrl: 'views/DashboardView.html',
            controller: 'DashboardController'
        }).when('/testcase', {
            templateUrl: 'views/DashboardView.html',
            controller: 'DashboardController'
        }).otherwise({
            redirectTo: '/testcase'
        });
    }
]);

require('angular-ui-bootstrap/ui-bootstrap-tpls.min.js');
require('angular-backtop');