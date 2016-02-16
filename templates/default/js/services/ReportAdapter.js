function getXmlDataSection(xmlDataSectionOrString){
    return xmlDataSectionOrString["#cdata-section"] || xmlDataSectionOrString;
}

function ReportAdapter(report) {
    this.report = new TestSuiteWrapper(report['test-run']);
}

ReportAdapter.prototype.summary = function() {
    return {
        'total': this.report['total'] || 0,
        'passed': this.report['passed'] || 0,
        'failed': this.report['failed'] || 0,
        'inconclusive': this.report['inconclusive'] || 0,
        'skipped': this.report['skipped'] || 0
    };
};

ReportAdapter.prototype.testFixtures = function() {
    var testFixtures = [];

    angular.forEach(this.report.getTestSuites(), function(testSuite) {
        if (testSuite.isTestFixture()) testFixtures.push(testSuite);
    });

    return testFixtures;
};

ReportAdapter.prototype.testCases = function() {
    return this.report.getTestCases();
};

ReportAdapter.prototype.findTestCaseById = function(id) {
    var testCase;

    angular.forEach(this.testCases(), function(tc) {
        if (tc.id === id) {
            testCase = tc;
            return;
        }
    });

    return testCase;
};

function TestSuiteWrapper(rawTestSuite) {
    var self = this;

    angular.forEach(rawTestSuite, function(value, key) {
        self[key.indexOf('@') === -1 ? key : key.substring(1)] = value;
    });
}

TestSuiteWrapper.prototype.isTestFixture = function() {
    return this.testSuite.type === 'TestFixture';
};

TestSuiteWrapper.prototype.getTestSuites = function() {
    var testSuites = [];

    angular.forEach(toArray(this['test-suite']), function(ts) {
        testSuites.push(new TestSuiteWrapper(ts));
    });

    return testSuites;
};

TestSuiteWrapper.prototype.getTestCases = function() {
    var categories = this.getCategories();
    
    var testCases = [];

    angular.forEach(toArray(this['test-case']), function(testCase) {
        testCases.push(new TestCaseWrapper(testCase));
    });

    angular.forEach(toArray(this.getTestSuites()), function(ts) {
        testCases = testCases.concat(ts.getTestCases());
    });

    angular.forEach(testCases, function(tc){
        tc.setCategories(categories);
    });

    return testCases;
};

TestSuiteWrapper.prototype.getCategories = getTestCategories;

function TestCaseWrapper(rawTestCase) {
    var self = this;

    angular.forEach(rawTestCase, function(value, key) {
        self[key.indexOf('@') === -1 ? key : key.substring(1)] = value;
    });

    if (this.images && this.images.image) {
        this.images = toArray(this.images.image);
    }

    this.categories = [];
}

TestCaseWrapper.prototype.setCategories = function(categories){
    this.categories = this.categories.concat(categories);
};

TestCaseWrapper.prototype.getCategories = function(){
    return this.categories.concat(getTestCategories.apply(this));
}

TestCaseWrapper.prototype.getMessage = function(){
    var message = [];

    if(this.reason && this.reason.message){
        message.push(getXmlDataSection(this.reason.message));
    }

    if(this.failure && this.failure.message){
        message.push(getXmlDataSection(this.failure.message));
    }

    if(this['stack-trace']){
        message.push(getXmlDataSection(this['stack-trace']));
    }

    return message.join("\n");
};

TestCaseWrapper.prototype.getOutput = function(){
    if(this.output){
        return getXmlDataSection(this.output);
    }
};

function getTestCategories(){
    var categories = [];

    angular.forEach(toArray(this.properties && this.properties.property), function(prop){
        if(prop['@name'] === 'Category' && prop['@value']){
            categories.push(prop['@value']);
        }
    });

    return categories;
}

function toArray(value) {
    return value ? angular.isArray(value) ? value : [value] : [];
}

module.exports = ['Report', ReportAdapter];