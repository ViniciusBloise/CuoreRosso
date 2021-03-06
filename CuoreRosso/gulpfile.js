﻿/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
  rimraf = require("rimraf"),
  concat = require("gulp-concat"),
  cssmin = require("gulp-cssmin"),
  uglify = require("gulp-uglify"),
  gutil  = require("gulp-util"),
  rename = require("gulp-rename");


var paths = {
  webroot: "./wwwroot/",
  theme: "theme/layout/",
  plugins: "plugins/"
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";


var module_path = {
  node_modules: "./node_modules/",
  themes: "./Themes/Default/",
  materialize: {},
  plugins_path: "plugins/",
  plugins: ["jquery/dist/**/*.js","tooltip/dist/**/*.js"],
  content: "./Content/",
  themes_metronic: "./Themes/layouts/layout/",
  themes_global: "./Themes/global/"
};

module_path.materialize.base = module_path.node_modules + "materialize-css/";
module_path.materialize.js = module_path.materialize.base + "dist/**/*.js";
module_path.materialize.css = module_path.materialize.base + "dist/**/*.css";
module_path.themes_plugin = module_path.themes_global + "plugins/";


var module_theme = {
    "css" : module_path.themes_metronic + "css/{themes/,}*.css",
    "img" : module_path.themes_metronic + "img/*.{jpg,png,gif}",
    "js"  : module_path.themes_metronic + "scripts/*.js",
    "dest" : paths.webroot + "themes/alfa/"
}

function appendToArray(fixed, array)
{
  var list = [];
  array.forEach( function(elem) {
    list.push(fixed + elem);
  })
  return list;
}



gulp.task("clean:js", function (cb) {
  rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
  rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
  return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
    .pipe(concat(paths.concatJsDest))
    .pipe(uglify())
    .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
  return gulp.src([paths.css, "!" + paths.minCss])
    .pipe(concat(paths.concatCssDest))
    .pipe(cssmin())
    .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css"]);

gulp.task("copy-theme", function() {
  return gulp.src([module_path.materialize.js, module_path.materialize.css])
    .pipe(gulp.dest(module_path.themes))
    .pipe(gulp.dest(paths.webroot + paths.theme));
});

/*, module_theme.js, module_theme.img*/
gulp.task("copy-theme-metro", function() {
    gutil.log('== Source = ' + module_theme.css);
    gulp.src(module_theme.css)
    .pipe(gulp.dest(module_theme.dest + "css"));
    gulp.src(module_theme.img)
    .pipe(gulp.dest(module_theme.dest + "img"));
    gulp.src(module_theme.js)
    .pipe(gulp.dest(module_theme.dest + "js"));
    gulp.src(module_path.themes_global + "css//*.css")
    .pipe(gulp.dest(paths.webroot + "themes/global/css/"));
    gulp.src(module_path.themes_global + "scripts//*.js")
    .pipe(gulp.dest(paths.webroot + "themes/global/js/"));
    gutil.log('== Source = ' + module_path.themes_plugin);
    gulp.src(module_path.themes_plugin + "bootstrap//**")
    .pipe(gulp.dest(paths.webroot + "themes/global/plugins/bootstrap"));
});

gulp.task("copy-plugins", function() {
    gulp.src(appendToArray(module_path.themes_plugin, [ "bootstrap/js//*.js"]))
    .pipe(gulp.dest(paths.webroot + "themes/global/plugins/"));
  return gulp.src(appendToArray(module_path.node_modules, module_path.plugins))
    .pipe(gulp.dest(paths.webroot + module_path.plugins_path));
})

gulp.task("copy-content", function() {
  return gulp.src(appendToArray(module_path.content, ["css/**/*.css"]))
    .pipe(gulp.dest(paths.webroot + "css/"))
    .pipe(cssmin())
    .pipe(rename({suffix: '.min'}))
    .pipe(gulp.dest(paths.webroot + "css/"))
})

gulp.task("pre-build", ["copy-content"]);
