var testResults = require('../../../TestResult.json');

function ReportProvider() {
    return window.NUnitTestResult = testResults;
}

module.exports = ReportProvider;