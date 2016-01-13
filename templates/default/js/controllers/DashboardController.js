var RESULT_TYPE_ANY = "Any";

function DashboardController($scope, $location, $routeParams, StateStorage, reportAdapter) {
    $scope.testCaseFilter = function(testCase) {
        return $scope.isResultTypeActive(testCase.result) || $scope.isResultTypeAny();
    }

    $scope.isResultTypeActive = function(resultType) {
        return $scope.getActiveResultType() === resultType;
    };

    $scope.isResultTypeAny = function(){
        return $scope.isResultTypeActive(RESULT_TYPE_ANY);
    };

    $scope.setAnyResultType = function(){
        $scope.setActiveResultType(RESULT_TYPE_ANY);
    };

    $scope.setActiveResultType = function(resultType) {
        StateStorage.put('ActiveResultType', resultType || RESULT_TYPE_ANY);
    };

    $scope.getActiveResultType = function(){
        return StateStorage.get('ActiveResultType') || RESULT_TYPE_ANY;
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