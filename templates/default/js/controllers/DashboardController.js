var RESULT_TYPE_ANY = "Any";

function DashboardController($scope, $location, $routeParams, StateStorage, reportAdapter) {
	
	$scope.testCaseFilter = function(testCase) {
        return $scope.isResultTypeActive(testCase.result) || $scope.isResultTypeAny();
    }
    $scope.textFilter = function(testCase) {
        matches = testCase.name.indexOf($scope.query) !== -1 || testCase.getMessage().indexOf($scope.query) !== -1
        return $scope.excludeFilter ? !matches : matches;    
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

    $scope.getlistbyMessage 
    $scope.openTestCase = function(testCase) {
        StateStorage.put('query', $scope.query);
        StateStorage.put('exclude', $scope.excludeFilter);
        $location.path('testcase/' + testCase.id);
    };

    $scope.setQueryFilterValue = function(item){
        $scope.query = item;
    };

    $scope.testCases = reportAdapter.testCases();
    $scope.summary = reportAdapter.summary();

    $scope.eventTreeOptions = {
        nodeChildren: 'children',
        injectClasses: {
            ul: 'list-group',
            li: 'list-group-item',
            iCollapsed: 'glyphicon glyphicon-plus',
            iExpanded: 'glyphicon glyphicon-minus',
        }
    };

    $scope.currentTestCase = $routeParams.id 
        ? reportAdapter.findTestCaseById($routeParams.id)
        : $scope.testCases[0]; 

    $scope.query = StateStorage.get('query') || '';
    $scope.excludeFilter = StateStorage.get('exclude') || false;
    console.log($scope.query);
}

module.exports = ['$scope', '$location', '$routeParams', 'StateStorage', 'ReportAdapter', DashboardController];