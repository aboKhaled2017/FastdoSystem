var gulp = require('gulp-v4'),
	browserify = require('gulp-browserify'),
	uglify = require('gulp-uglify'),
	watch = require('gulp-watch'),
	tasks = [];


gulp.task('scripts', function () {
	gulp.src('Script/loginPageScript.js')
		.pipe(browserify({
			insertGlobals: false,
			debug: false
		})).pipe(gulp.dest('./dest'));

	/*gulp.src('about.js')
		.pipe(browserify({
			insertGlobals: false,
			debug: false
		})).pipe(gulp.dest('./build'));*/
});


/*gulp.task('minify', function () {
	gulp.src('./build/start.js')
		.pipe(uglify({
			outSourceMap: true
		})).pipe(gulp.dest('./dist'));

	gulp.src('./build/about.js')
		.pipe(uglify({
			outSourceMap: true
		})).pipe(gulp.dest('./dist'));
});

gulp.task('watch', function () {
	watch({ glob: '*.js' }, function () {
		gulp.start('scripts');
	});
});*/

tasks.push('scripts');

if (process.env.NODE_ENV === 'Release') {
	tasks.push('minify');
} else if (process.env.NODE_ENV === 'Dev') {
	tasks.push('watch');
}

gulp.task('default', gulp.series('scripts'));