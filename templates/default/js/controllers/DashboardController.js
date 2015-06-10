var displayMapCookieKey = "DashboardController.displayMap";

function DashboardController($scope, $location, $routeParams, StateStorage, reportAdapter) {
    var displayMap = StateStorage.get(displayMapCookieKey) || {};

    $scope.testCaseFilter = function(testCase) {
        return $scope.shouldDisplayResult(testCase.result);
    }

    $scope.shouldDisplayResult = function(resultType) {
        if (!displayMap.hasOwnProperty(resultType)) {
            displayMap[resultType] = true;
        }

        return displayMap[resultType];
    };

    $scope.toggleDisplayResult = function(resultType) {
        displayMap[resultType] = !$scope.shouldDisplayResult(resultType);
        StateStorage.put(displayMapCookieKey, displayMap);
    };

    $scope.openTestCase = function(testCase) {
        $location.path('testcase/' + testCase.id);
    };

    $scope.setQueryFilterValue = function(item){
        $scope.query = item;
    };

    $scope.testCases = reportAdapter.testCases();
    $scope.summary = reportAdapter.summary();

    $scope.currentTestCase = $routeParams.id 
        ? reportAdapter.findTestCaseById($routeParams.id)
        : $scope.testCases[0];
}

module.exports = ['$scope', '$location', '$routeParams', 'StateStorage', 'ReportAdapter', DashboardController];