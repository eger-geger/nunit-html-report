'use strict';

var gulp = require('gulp');
var gutil = require('gulp-util')
var inject = require('gulp-inject');
var uglify = require('gulp-uglify');

var source = require('vinyl-source-stream');
var buffer = require('vinyl-buffer');
var browserify = require('browserify');

gulp.task('default', ['index-html']);

gulp.task('copy-resources', function(){
    gulp.src('./node_modules/bootstrap/dist/fonts/*')
        .pipe(gulp.dest('./dist/bootstrap/dist/fonts/'));

    gulp.src('./node_modules/bootstrap/dist/css/*.min.css')
        .pipe(gulp.dest('./dist/bootstrap/dist/css/'));
});

gulp.task('index-html', ['copy-resources'], function() {
    var jsStream = browserify('./js/app.js').bundle()
        .pipe(source('app.js'))
        .pipe(buffer()).pipe(uglify());

    var cssStream = gulp.src([
        './node_modules/angular-backtop/dist/angular-backtop.css',
        './css/*'
    ]);

    var viewStream = gulp.src([
        './views/*'
    ]);

    return gulp.src('./index.html')
        .pipe(inject(jsStream, {
            starttag: '<!-- inject:app:js -->',
            transform: embedJavaScript
        })).pipe(inject(cssStream, {
            starttag: '<!-- inject:styles:css -->',
            transform: embedStyleSheet
        })).pipe(inject(viewStream, {
            starttag: '<!-- inject:views:html -->',
            transform: embedView
        })).pipe(gulp.dest('./dist'));
});

function embedJavaScript(filePath, file) {
    return ['<script type="text/javascript">', file.contents.toString('utf8'), '</script>'].join('');
}

function embedStyleSheet(filePath, file) {
    return ['<style type="text/css">', file.contents.toString('utf8'), '</style>'].join('');
}

function embedView(filePath, file) {
    return [
        '<script type="text/ng-template" id="' + filePath.substring(1) + '">',
        file.contents.toString('utf8'),
        '</script>'
    ].join('');
}