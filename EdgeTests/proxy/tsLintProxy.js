
// global, stuff that doesn't change per function call
var Linter = require("tslint");
var fs = require('fs');
var options = {options};

// edge.js function
return function(data, callback) {

  // data object for any arguments required can be complex .net type
  var fileName = data.fileName;
  var contents = data.contents;
  var result = null;
  try {
    var tsLinter = new Linter(fileName, contents, options);
    result = tsLinter.lint();
  } catch (e) {
    callback(e, result);
  }

  callback(null, result);
}


