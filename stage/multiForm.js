/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, {
/******/ 				configurable: false,
/******/ 				enumerable: true,
/******/ 				get: getter
/******/ 			});
/******/ 		}
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "/build/";
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = "./assets/js/script/multiForm.js");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./assets/js/script/multiForm.js":
/*!***************************************!*\
  !*** ./assets/js/script/multiForm.js ***!
  \***************************************/
/*! dynamic exports provided */
/*! all exports used */
/***/ (function(module, exports, __webpack_require__) {

/* WEBPACK VAR INJECTION */(function($) {//permet de construir le formulaire de creation d'une seance
function getSeanceForm() {
    var id = $(".session").attr('id');
    //permet de récupérer les infos nécessaire afin de créer une seance
    $.ajax({
        url: window.location.origin + "/get/create/seance/modal/session/" + id,
        type: "GET",

        success: function success(data) {
            //voir la fonction createSeance() dans le SessionController
            var session = data[0];
            var ateliers = data[1];
            //reconstruction du fomulaire de création d'une seance
            $("#seanceForm").empty();
            $("#seanceForm").append($("<label>").text("Lieu :")).append($("<input>").attr("type", "text").attr("name", "adresse").attr("id", "adresse").attr("placeholder", "numero rue, code_postal, ville").attr("required", true)).append($("<label>").text("Selectionnez le jour :")).append($("<input>").attr("type", "date").attr("name", "date").attr("id", "date").attr("required", true).attr("min", session[0].date_debut).attr("max", session[0].date_fin)).append($("<label>").text("Heure de debut :")).append($("<input>").attr("type", "time").attr("name", "heure_debut").attr("id", "heure_debut").attr("required", true)).append($("<label>").text("Heure de fin :")).append($("<input>").attr("type", "time").attr("name", "heure_fin").attr("id", "heure_fin").attr("required", true)).append($("<label>").text("Selectionnez un atelier:")).append($("<select>").attr("type", "text").attr("name", "atelier").attr("id", "atelier").attr("required", true).append($("<option>")));
            for (var i in ateliers) {
                $("#atelier").append($("<option>").val(ateliers[i].id).text(ateliers[i].nom));
            }
        }
    });
}

//permet de reconstruir le fomulaire de cration d'un atelier
function newAtelierForm() {
    $("#atelierForm").empty();
    $("#atelierForm").append($("<label>").text("Nom :")).append($("<input>").attr("id", "nom").attr("placeholder", "entrez le nom d'un atelier").attr("required", true).attr("type", "text").val("")).append($("<label>").attr("required", true).text("Description :")).append($("<textarea>").attr("id", "description").attr("required", true).attr("placeholder", "entrez une description").addClass("area").attr("type", "text").attr("rows", "12").val(""));
}

//permet de ajouter un nouvel atelier en base de données
window.addAtelier = function (id) {
    if ($("#nom").val() !== "" && $("#description").val() !== "") {
        console.log(id);
        $.ajax({
            url: window.location.origin + "/create/atelier/session/" + id,
            type: "POST",

            data: {
                nom: $("#nom").val(),
                description: $("#description").val()
            },

            success: function success() {
                //reconstruction des formulaires
                newAtelierForm();
                getSeanceForm();
            }
        });
    }
    //reconstruction du fomulaire de création d'une seance
    getSeanceForm();
};

//permet de ajouter une nouvelle seance en base de données
window.createSeance = function (id) {
    if ($("#date").val() !== "" && $("#heure_debut").val() !== "" && $("#heure_fin").val() !== "" && $("#adresse").val() !== "" && $("#atelier").val() !== "") {
        $.ajax({
            url: window.location.origin + "/create/seance/session/" + id,
            type: "POST",

            data: {
                date: $("#date").val(),
                heure_debut: $("#heure_debut").val(),
                heure_fin: $("#heure_fin").val(),
                adresse: $("#adresse").val(),
                atelier: $("#atelier").val()
            },

            success: function success() {
                //reconstruction du fomulaire de création d'une seance
                getSeanceForm();
            }
        });
    }
};
/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(/*! jquery */ "jquery")))

/***/ }),

/***/ "jquery":
/*!*************************!*\
  !*** external "jQuery" ***!
  \*************************/
/*! dynamic exports provided */
/*! all exports used */
/***/ (function(module, exports) {

module.exports = jQuery;

/***/ })

/******/ });
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly8vd2VicGFjay9ib290c3RyYXAgOGE0ODA4YjA3MDA5ZGNiNWNmNGEiLCJ3ZWJwYWNrOi8vLy4vYXNzZXRzL2pzL3NjcmlwdC9tdWx0aUZvcm0uanMiLCJ3ZWJwYWNrOi8vL2V4dGVybmFsIFwialF1ZXJ5XCIiXSwibmFtZXMiOlsiZ2V0U2VhbmNlRm9ybSIsImlkIiwiJCIsImF0dHIiLCJhamF4IiwidXJsIiwid2luZG93IiwibG9jYXRpb24iLCJvcmlnaW4iLCJ0eXBlIiwic3VjY2VzcyIsImRhdGEiLCJzZXNzaW9uIiwiYXRlbGllcnMiLCJlbXB0eSIsImFwcGVuZCIsInRleHQiLCJkYXRlX2RlYnV0IiwiZGF0ZV9maW4iLCJpIiwidmFsIiwibm9tIiwibmV3QXRlbGllckZvcm0iLCJhZGRDbGFzcyIsImFkZEF0ZWxpZXIiLCJjb25zb2xlIiwibG9nIiwiZGVzY3JpcHRpb24iLCJjcmVhdGVTZWFuY2UiLCJkYXRlIiwiaGV1cmVfZGVidXQiLCJoZXVyZV9maW4iLCJhZHJlc3NlIiwiYXRlbGllciJdLCJtYXBwaW5ncyI6IjtBQUFBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOzs7QUFHQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxhQUFLO0FBQ0w7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQSxtQ0FBMkIsMEJBQTBCLEVBQUU7QUFDdkQseUNBQWlDLGVBQWU7QUFDaEQ7QUFDQTtBQUNBOztBQUVBO0FBQ0EsOERBQXNELCtEQUErRDs7QUFFckg7QUFDQTs7QUFFQTtBQUNBOzs7Ozs7Ozs7Ozs7O0FDN0RBO0FBQ0EsU0FBU0EsYUFBVCxHQUF5QjtBQUNyQixRQUFJQyxLQUFLQyxFQUFFLFVBQUYsRUFBY0MsSUFBZCxDQUFtQixJQUFu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UyxtQkFBVztBQUNoQjtBQUNBVjtBQUNIO0FBZkUsU0FBUDtBQWlCSDtBQUNKLENBcEJELEM7Ozs7Ozs7Ozs7Ozs7QUNsRUEsd0IiLCJmaWxlIjoianMvc2NyaXB0L211bHRpRm9ybS5qcyIsInNvdXJjZXNDb250ZW50IjpbIiBcdC8vIFRoZSBtb2R1bGUgY2FjaGVcbiBcdHZhciBpbnN0YWxsZWRNb2R1bGVzID0ge307XG5cbiBcdC8vIFRoZSByZXF1aXJlIGZ1bmN0aW9uXG4gXHRmdW5jdGlvbiBfX3dlYnBhY2tfcmVxdWlyZV9fKG1vZHVsZUlkKSB7XG5cbiBcdFx0Ly8gQ2hlY2sgaWYgbW9kdWxlIGlzIGluIGNhY2hlXG4gXHRcdGlmKGluc3RhbGxlZE1vZHVsZXNbbW9kdWxlSWRdKSB7XG4gXHRcdFx0cmV0dXJuIGluc3RhbGxlZE1vZHVsZXNbbW9kdWxlSWRdLmV4cG9ydHM7XG4gXHRcdH1cbiBcdFx0Ly8gQ3JlYXRlIGEgbmV3IG1vZHVsZSAoYW5kIHB1dCBpdCBpbnRvIHRoZSBjYWNoZSlcbiBcdFx0dmFyIG1vZHVsZSA9IGluc3RhbGxlZE1vZHVsZXNbbW9kdWxlSWRdID0ge1xuIFx0XHRcdGk6IG1vZHVsZUlkLFxuIFx0XHRcdGw6IGZhbHNlLFxuIFx0XHRcdGV4cG9ydHM6IHt9XG4gXHRcdH07XG5cbiBcdFx0Ly8gRXhlY3V0ZSB0aGUgbW9kdWxlIGZ1bmN0aW9uXG4gXHRcdG1vZHVsZXNbbW9kdWxlSWRdLmNhbGwobW9kdWxlLmV4cG9ydHMsIG1vZHVsZSwgbW9kdWxlLmV4cG9ydHMsIF9fd2VicGFja19yZXF1aXJlX18pO1xuXG4gXHRcdC8vIEZsYWcgdGhlIG1vZHVsZSBhcyBsb2FkZWRcbiBcdFx0bW9kdWxlLmwgPSB0cnVlO1xuXG4gXHRcdC8vIFJldHVybiB0aGUgZXhwb3J0cyBvZiB0aGUgbW9kdWxlXG4gXHRcdHJldHVybiBtb2R1bGUuZXhwb3J0cztcbiBcdH1cblxuXG4gXHQvLyBleHBvc2UgdGhlIG1vZHVsZXMgb2JqZWN0IChfX3dlYnBhY2tfbW9kdWxlc19fKVxuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5tID0gbW9kdWxlcztcblxuIFx0Ly8gZXhwb3NlIHRoZSBtb2R1bGUgY2FjaGVcbiBcdF9fd2VicGFja19yZXF1aXJlX18uYyA9IGluc3RhbGxlZE1vZHVsZXM7XG5cbiBcdC8vIGRlZmluZSBnZXR0ZXIgZnVuY3Rpb24gZm9yIGhhcm1vbnkgZXhwb3J0c1xuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5kID0gZnVuY3Rpb24oZXhwb3J0cywgbmFtZSwgZ2V0dGVyKSB7XG4gXHRcdGlmKCFfX3dlYnBhY2tfcmVxdWlyZV9fLm8oZXhwb3J0cywgbmFtZSkpIHtcbiBcdFx0XHRPYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgbmFtZSwge1xuIFx0XHRcdFx0Y29uZmlndXJhYmxlOiBmYWxzZSxcbiBcdFx0XHRcdGVudW1lcmFibGU6IHRydWUsXG4gXHRcdFx0XHRnZXQ6IGdldHRlclxuIFx0XHRcdH0pO1xuIFx0XHR9XG4gXHR9O1xuXG4gXHQvLyBnZXREZWZhdWx0RXhwb3J0IGZ1bmN0aW9uIGZvciBjb21wYXRpYmlsaXR5IHdpdGggbm9uLWhhcm1vbnkgbW9kdWxlc1xuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5uID0gZnVuY3Rpb24obW9kdWxlKSB7XG4gXHRcdHZhciBnZXR0ZXIgPSBtb2R1bGUgJiYgbW9kdWxlLl9fZXNNb2R1bGUgP1xuIFx0XHRcdGZ1bmN0aW9uIGdldERlZmF1bHQoKSB7IHJldHVybiBtb2R1bGVbJ2RlZmF1bHQnXTsgfSA6XG4gXHRcdFx0ZnVuY3Rpb24gZ2V0TW9kdWxlRXhwb3J0cygpIHsgcmV0dXJuIG1vZHVsZTsgfTtcbiBcdFx0X193ZWJwYWNrX3JlcXVpcmVfXy5kKGdldHRlciwgJ2EnLCBnZXR0ZXIpO1xuIFx0XHRyZXR1cm4gZ2V0dGVyO1xuIFx0fTtcblxuIFx0Ly8gT2JqZWN0LnByb3RvdHlwZS5oYXNPd25Qcm9wZXJ0eS5jYWxsXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLm8gPSBmdW5jdGlvbihvYmplY3QsIHByb3BlcnR5KSB7IHJldHVybiBPYmplY3QucHJvdG90eXBlLmhhc093blByb3BlcnR5LmNhbGwob2JqZWN0LCBwcm9wZXJ0eSk7IH07XG5cbiBcdC8vIF9fd2VicGFja19wdWJsaWNfcGF0aF9fXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLnAgPSBcIi9idWlsZC9cIjtcblxuIFx0Ly8gTG9hZCBlbnRyeSBtb2R1bGUgYW5kIHJldHVybiBleHBvcnRzXG4gXHRyZXR1cm4gX193ZWJwYWNrX3JlcXVpcmVfXyhfX3dlYnBhY2tfcmVxdWlyZV9fLnMgPSBcIi4vYXNzZXRzL2pzL3NjcmlwdC9tdWx0aUZvcm0uanNcIik7XG5cblxuXG4vLyBXRUJQQUNLIEZPT1RFUiAvL1xuLy8gd2VicGFjay9ib290c3RyYXAgOGE0ODA4YjA3MDA5ZGNiNWNmNGEiLCIvL3Blcm1ldCBkZSBjb25zdHJ1aXIgbGUgZm9ybXVsYWlyZSBkZSBjcmVhdGlvbiBkJ3VuZSBzZWFuY2VcclxuZnVuY3Rpb24gZ2V0U2VhbmNlRm9ybSgpIHtcclxuICAgIHZhciBpZCA9ICQoXCIuc2Vzc2lvblwiKS5hdHRyKCdpZCcpO1xyXG4gICAgLy9wZXJtZXQgZGUgcsOpY3Vww6lyZXIgbGVzIGluZm9zIG7DqWNlc3NhaXJlIGFmaW4gZGUgY3LDqWVyIHVuZSBzZWFuY2VcclxuICAgICQuYWpheCh7XHJcbiAgICAgICAgdXJsOiB3aW5kb3cubG9jYXRpb24ub3JpZ2luICsgXCIvZ2V0L2NyZWF0ZS9zZWFuY2UvbW9kYWwvc2Vzc2lvbi9cIiArIGlkLFxyXG4gICAgICAgIHR5cGU6IFwiR0VUXCIsXHJcblxyXG4gICAgICAgIHN1Y2Nlc3M6IGZ1bmN0aW9uKGRhdGEpIHtcclxuICAgICAgICAgICAgLy92b2lyIGxhIGZvbmN0aW9uIGNyZWF0ZVNlYW5jZSgpIGRhbnMgbGUgU2Vzc2lvbkNvbnRyb2xsZXJcclxuICAgICAgICAgICAgdmFyIHNlc3Npb24gPSBkYXRhWzBdO1xyXG4gICAgICAgICAgICB2YXIgYXRlbGllcnMgPSBkYXRhWzFdO1xyXG4gICAgICAgICAgICAvL3JlY29uc3RydWN0aW9uIGR1IGZvbXVsYWlyZSBkZSBjcsOpYXRpb24gZCd1bmUgc2VhbmNlXHJcbiAgICAgICAgICAgICQoXCIjc2VhbmNlRm9ybVwiKS5lbXB0eSgpO1xyXG4gICAgICAgICAgICAkKFwiI3NlYW5jZUZvcm1cIikuYXBwZW5kKCQoXCI8bGFiZWw+XCIpLnRleHQoXCJMaWV1IDpcIikpXHJcbiAgICAgICAgICAgICAgICAuYXBwZW5kKCQoXCI8aW5wdXQ+XCIpLmF0dHIoXCJ0eXBlXCIsIFwidGV4dFwiKS5hdHRyKFwibmFtZVwiLCBcImFkcmVzc2VcIikuYXR0cihcImlkXCIsIFwiYWRyZXNzZVwiKS5hdHRyKFwicGxhY2Vob2xkZXJcIiwgXCJudW1lcm8gcnVlLCBjb2RlX3Bvc3RhbCwgdmlsbGVcIikuYXR0cihcInJlcXVpcmVkXCIsIHRydWUpKVxyXG4gICAgICAgICAgICAgICAgLmFwcGVuZCgkKFwiPGxhYmVsPlwiKS50ZXh0KFwiU2VsZWN0aW9ubmV6IGxlIGpvdXIgOlwiKSlcclxuICAgICAgICAgICAgICAgIC5hcHBlbmQoJChcIjxpbnB1dD5cIikuYXR0cihcInR5cGVcIiwgXCJkYXRlXCIpLmF0dHIoXCJuYW1lXCIsIFwiZGF0ZVwiKS5hdHRyKFwiaWRcIiwgXCJkYXRlXCIpLmF0dHIoXCJyZXF1aXJlZFwiLCB0cnVlKS5hdHRyKFwibWluXCIsIHNlc3Npb25bMF0uZGF0ZV9kZWJ1dCkuYXR0cihcIm1heFwiLCBzZXNzaW9uWzBdLmRhdGVfZmluKSlcclxuICAgICAgICAgICAgICAgIC5hcHBlbmQoJChcIjxsYWJlbD5cIikudGV4dChcIkhldXJlIGRlIGRlYnV0IDpcIikpXHJcbiAgICAgICAgICAgICAgICAuYXBwZW5kKCQoXCI8aW5wdXQ+XCIpLmF0dHIoXCJ0eXBlXCIsIFwidGltZVwiKS5hdHRyKFwibmFtZVwiLCBcImhldXJlX2RlYnV0XCIpLmF0dHIoXCJpZFwiLCBcImhldXJlX2RlYnV0XCIpLmF0dHIoXCJyZXF1aXJlZFwiLCB0cnVlKSlcclxuICAgICAgICAgICAgICAgIC5hcHBlbmQoJChcIjxsYWJlbD5cIikudGV4dChcIkhldXJlIGRlIGZpbiA6XCIpKVxyXG4gICAgICAgICAgICAgICAgLmFwcGVuZCgkKFwiPGlucHV0PlwiKS5hdHRyKFwidHlwZVwiLCBcInRpbWVcIikuYXR0cihcIm5hbWVcIiwgXCJoZXVyZV9maW5cIikuYXR0cihcImlkXCIsIFwiaGV1cmVfZmluXCIpLmF0dHIoXCJyZXF1aXJlZFwiLCB0cnVlKSlcclxuICAgICAgICAgICAgICAgIC5hcHBlbmQoJChcIjxsYWJlbD5cIikudGV4dChcIlNlbGVjdGlvbm5leiB1biBhdGVsaWVyOlwiKSlcclxuICAgICAgICAgICAgICAgIC5hcHBlbmQoJChcIjxzZWxlY3Q+XCIpLmF0dHIoXCJ0eXBlXCIsIFwidGV4dFwiKS5hdHRyKFwibmFtZVwiLCBcImF0ZWxpZXJcIikuYXR0cihcImlkXCIsIFwiYXRlbGllclwiKS5hdHRyKFwicmVxdWlyZWRcIiwgdHJ1ZSlcclxuICAgICAgICAgICAgICAgICAgICAuYXBwZW5kKCQoXCI8b3B0aW9uPlwiKSkpO1xyXG4gICAgICAgICAgICBmb3IgKHZhciBpIGluIGF0ZWxpZXJzKSB7XHJcbiAgICAgICAgICAgICAgICAkKFwiI2F0ZWxpZXJcIikuYXBwZW5kKCQoXCI8b3B0aW9uPlwiKS52YWwoYXRlbGllcnNbaV0uaWQpLnRleHQoYXRlbGllcnNbaV0ubm9tKSk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9LFxyXG4gICAgfSk7XHJcbn1cclxuXHJcbi8vcGVybWV0IGRlIHJlY29uc3RydWlyIGxlIGZvbXVsYWlyZSBkZSBjcmF0aW9uIGQndW4gYXRlbGllclxyXG5mdW5jdGlvbiBuZXdBdGVsaWVyRm9ybSgpIHtcclxuICAgICQoXCIjYXRlbGllckZvcm1cIikuZW1wdHkoKTtcclxuICAgICQoXCIjYXRlbGllckZvcm1cIikuYXBwZW5kKCQoXCI8bGFiZWw+XCIpLnRleHQoXCJOb20gOlwiKSlcclxuICAgICAgICAuYXBwZW5kKCQoXCI8aW5wdXQ+XCIpLmF0dHIoXCJpZFwiLCBcIm5vbVwiKS5hdHRyKFwicGxhY2Vob2xkZXJcIiwgXCJlbnRyZXogbGUgbm9tIGQndW4gYXRlbGllclwiKS5hdHRyKFwicmVxdWlyZWRcIiwgdHJ1ZSkuYXR0cihcInR5cGVcIiwgXCJ0ZXh0XCIpLnZhbChcIlwiKSlcclxuICAgICAgICAuYXBwZW5kKCQoXCI8bGFiZWw+XCIpLmF0dHIoXCJyZXF1aXJlZFwiLCB0cnVlKS50ZXh0KFwiRGVzY3JpcHRpb24gOlwiKSlcclxuICAgICAgICAuYXBwZW5kKCQoXCI8dGV4dGFyZWE+XCIpLmF0dHIoXCJpZFwiLCBcImRlc2NyaXB0aW9uXCIpLmF0dHIoXCJyZXF1aXJlZFwiLCB0cnVlKS5hdHRyKFwicGxhY2Vob2xkZXJcIiwgXCJlbnRyZXogdW5lIGRlc2NyaXB0aW9uXCIpLmFkZENsYXNzKFwiYXJlYVwiKS5hdHRyKFwidHlwZVwiLCBcInRleHRcIikuYXR0cihcInJvd3NcIiwgXCIxMlwiKS52YWwoXCJcIikpO1xyXG59XHJcblxyXG4vL3Blcm1ldCBkZSBham91dGVyIHVuIG5vdXZlbCBhdGVsaWVyIGVuIGJhc2UgZGUgZG9ubsOpZXNcclxud2luZG93LmFkZEF0ZWxpZXIgPSBmdW5jdGlvbihpZCkge1xyXG4gICAgaWYgKCQoXCIjbm9tXCIpLnZhbCgpICE9PSBcIlwiICYmICQoXCIjZGVzY3JpcHRpb25cIikudmFsKCkgIT09IFwiXCIpIHtcclxuICAgICAgICBjb25zb2xlLmxvZyhpZClcclxuICAgICAgICAkLmFqYXgoe1xyXG4gICAgICAgICAgICB1cmw6IHdpbmRvdy5sb2NhdGlvbi5vcmlnaW4gKyBcIi9jcmVhdGUvYXRlbGllci9zZXNzaW9uL1wiICsgaWQsXHJcbiAgICAgICAgICAgIHR5cGU6IFwiUE9TVFwiLFxyXG5cclxuICAgICAgICAgICAgZGF0YToge1xyXG4gICAgICAgICAgICAgICAgbm9tOiAkKFwiI25vbVwiKS52YWwoKSxcclxuICAgICAgICAgICAgICAgIGRlc2NyaXB0aW9uOiAkKFwiI2Rlc2NyaXB0aW9uXCIpLnZhbCgpXHJcbiAgICAgICAgICAgIH0sXHJcblxyXG4gICAgICAgICAgICBzdWNjZXNzOiBmdW5jdGlvbigpIHtcclxuICAgICAgICAgICAgICAgIC8vcmVjb25zdHJ1Y3Rpb24gZGVzIGZvcm11bGFpcmVzXHJcbiAgICAgICAgICAgICAgICBuZXdBdGVsaWVyRm9ybSgpO1xyXG4gICAgICAgICAgICAgICAgZ2V0U2VhbmNlRm9ybSgpO1xyXG4gICAgICAgICAgICB9LFxyXG4gICAgICAgIH0pO1xyXG4gICAgfVxyXG4gICAgLy9yZWNvbnN0cnVjdGlvbiBkdSBmb211bGFpcmUgZGUgY3LDqWF0aW9uIGQndW5lIHNlYW5jZVxyXG4gICAgZ2V0U2VhbmNlRm9ybSgpO1xyXG59O1xyXG5cclxuLy9wZXJtZXQgZGUgYWpvdXRlciB1bmUgbm91dmVsbGUgc2VhbmNlIGVuIGJhc2UgZGUgZG9ubsOpZXNcclxud2luZG93LmNyZWF0ZVNlYW5jZSA9IGZ1bmN0aW9uKGlkKSB7XHJcbiAgICBpZiAoJChcIiNkYXRlXCIpLnZhbCgpICE9PSBcIlwiICYmICQoXCIjaGV1cmVfZGVidXRcIikudmFsKCkgIT09IFwiXCIgJiYgJChcIiNoZXVyZV9maW5cIikudmFsKCkgIT09IFwiXCIgJiYgJChcIiNhZHJlc3NlXCIpLnZhbCgpICE9PSBcIlwiICYmICQoXCIjYXRlbGllclwiKS52YWwoKSAhPT0gXCJcIikge1xyXG4gICAgICAgICQuYWpheCh7XHJcbiAgICAgICAgICAgIHVybDogd2luZG93LmxvY2F0aW9uLm9yaWdpbiArIFwiL2NyZWF0ZS9zZWFuY2Uvc2Vzc2lvbi9cIiArIGlkLFxyXG4gICAgICAgICAgICB0eXBlOiBcIlBPU1RcIixcclxuXHJcbiAgICAgICAgICAgIGRhdGE6IHtcclxuICAgICAgICAgICAgICAgIGRhdGU6ICQoXCIjZGF0ZVwiKS52YWwoKSxcclxuICAgICAgICAgICAgICAgIGhldXJlX2RlYnV0OiAkKFwiI2hldXJlX2RlYnV0XCIpLnZhbCgpLFxyXG4gICAgICAgICAgICAgICAgaGV1cmVfZmluOiAkKFwiI2hldXJlX2ZpblwiKS52YWwoKSxcclxuICAgICAgICAgICAgICAgIGFkcmVzc2U6ICQoXCIjYWRyZXNzZVwiKS52YWwoKSxcclxuICAgICAgICAgICAgICAgIGF0ZWxpZXI6ICQoXCIjYXRlbGllclwiKS52YWwoKSxcclxuICAgICAgICAgICAgfSxcclxuXHJcbiAgICAgICAgICAgIHN1Y2Nlc3M6IGZ1bmN0aW9uKCkge1xyXG4gICAgICAgICAgICAgICAgLy9yZWNvbnN0cnVjdGlvbiBkdSBmb211bGFpcmUgZGUgY3LDqWF0aW9uIGQndW5lIHNlYW5jZVxyXG4gICAgICAgICAgICAgICAgZ2V0U2VhbmNlRm9ybSgpO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfSk7XHJcbiAgICB9XHJcbn07XG5cblxuLy8gV0VCUEFDSyBGT09URVIgLy9cbi8vIC4vYXNzZXRzL2pzL3NjcmlwdC9tdWx0aUZvcm0uanMiLCJtb2R1bGUuZXhwb3J0cyA9IGpRdWVyeTtcblxuXG4vLy8vLy8vLy8vLy8vLy8vLy9cbi8vIFdFQlBBQ0sgRk9PVEVSXG4vLyBleHRlcm5hbCBcImpRdWVyeVwiXG4vLyBtb2R1bGUgaWQgPSBqcXVlcnlcbi8vIG1vZHVsZSBjaHVua3MgPSAyIDMgNCA1IDYgNyA4IDkgMTAgMTEgMTIgMTMgMTQgMTUgMTYgMTcgMTggMTkgMjAgMjEiXSwic291cmNlUm9vdCI6IiJ9