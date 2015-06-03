function TestCaseController($scope, $routeParams, $location, reportAdapter) {
	var testCase = reportAdapter.findTestCaseById($routeParams.id);

    $scope.openDashboard = function() {
        $location.path('/dashboard');
    };

    $scope.name = testCase.name;
    $scope.output = testCase.output;
    $scope.result = testCase.result;
    $scope.images = testCase.images;
    $scope.message = getMessage(testCase);
    $scope.duration = testCase.duration;
    $scope.classname = testCase.classname;
}

function getMessage(testCase){
	if(testCase.reason && testCase.reason.message){
		return testCase.reason.message;
	}

	if(testCase.failure && testCase.failure.message){
    	return testCase.failure.message;
    }

    if(testCase.errorMessage && testCase['stack-trace']){
    	return testCase.errorMessage + "\n" + testCase['stack-trace'];
    }
}

module.exports = ['$scope', '$routeParams', '$location', 'ReportAdapter', TestCaseController];