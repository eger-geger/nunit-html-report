function DashboardController($scope, $location, reportAdapter) {
    var displayMap = {};

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

module.exports = ['$scope', '$location', 'ReportAdapter', DashboardController];