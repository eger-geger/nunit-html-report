var displayMapCookieKey = "DashboardController.displayMap";

function DashboardController($scope, $location, StateStorage, reportAdapter) {
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

    $scope.summary = reportAdapter.summary();
    $scope.testCases = reportAdapter.testCases();
}

module.exports = ['$scope', '$location', 'StateStorage', 'ReportAdapter', DashboardController];