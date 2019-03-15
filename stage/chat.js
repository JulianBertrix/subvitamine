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
/******/ 	return __webpack_require__(__webpack_require__.s = "./assets/js/script/chat.js");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./assets/js/script/chat.js":
/*!**********************************!*\
  !*** ./assets/js/script/chat.js ***!
  \**********************************/
/*! dynamic exports provided */
/*! all exports used */
/***/ (function(module, exports, __webpack_require__) {

/* WEBPACK VAR INJECTION */(function($) {$(document).ready(function () {
    var id = $(".seance").attr("id");
    affichage_messages(id);
});

var refreshMsg;
//permet d'afficher les messages du chat
window.affichage_messages = function (id) {
    $.ajax({
        type: "GET",
        url: window.location.origin + "/chat/get/message/seance/" + id,
        success: function success(data) {
            creation_chat(data, id);
            clearTimeout(refreshMsg);
            //refresh tout les 500 milisecondes
            refreshMsg = setTimeout(function () {
                affichage_messages(id);
            }, 500);
        },
        error: function error() {
            console.log("error");
        }
    });
};

//permet d'envoyer un message en base de données
window.save_message = function (id) {
    if ($("#textZone").val() !== "") {
        $.ajax({
            type: "POST",
            url: window.location.origin + "/chat/post/message/seance/" + id,
            dataType: "json",
            data: {
                msg: $("#textZone").val()
            },
            success: function success() {
                //permet de fixer la scroll bar à chaque refresh
                var height = $('#msg_liste').prop("scrollHeight");
                $('div p').each(function () {
                    height += parseInt($(this).height());
                });

                height += '';

                $('div').animate({ scrollTop: height });

                $("#textZone").val("");
            }
        });
    }
};

//permet de construir le chat à chaque refresh
window.creation_chat = function (datas) {
    var idF = $(".user").attr("id");
    $("#msg_liste").empty();
    for (var i in datas) {
        var message = datas[i];
        if (idF == message.idFacilitateur) {
            $("#msg_liste").append($("<div>").addClass("d-flex justify-content-start mb-4")).append($("<div>").addClass("msg_cotainer").append($("<p>").text(message.prenom + " " + message.nom).addClass("font-weight-bold blockquote-footer").css("color", "white")).append($("<p>").text(message.text)));
        } else {
            $("#msg_liste").append($("<div>").addClass("d-flex justify-content-end mb-4")).append($("<div>").addClass("msg_cotainer_send").append($("<p>").text(message.prenom + " " + message.nom).addClass("font-weight-bold blockquote-footer").css("color", "black")).append($("<p>").text(message.text)));
        }
    }

    $('#msg_liste').scrollTop($('#msg_liste').prop("scrollHeight"));
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
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly8vd2VicGFjay9ib290c3RyYXAgOGE0ODA4YjA3MDA5ZGNiNWNmNGEiLCJ3ZWJwYWNrOi8vLy4vYXNzZXRzL2pzL3NjcmlwdC9jaGF0LmpzIiwid2VicGFjazovLy9leHRlcm5hbCBcImpRdWVyeVwiIl0sIm5hbWVzIjpbIiQiLCJkb2N1bWVudCIsInJlYWR5IiwiaWQiLCJhdHRyIiwiYWZmaWNoYWdlX21lc3NhZ2VzIiwicmVmcmVzaE1zZyIsIndpbmRvdyIsImFqYXgiLCJ0eXBlIiwidXJsIiwibG9jYXRpb24iLCJvcmlnaW4iLCJzdWNjZXNzIiwiZGF0YSIsImNyZWF0aW9uX2NoYXQiLCJjbGVhclRpbWVvdXQiLCJzZXRUaW1lb3V0IiwiZXJyb3IiLCJjb25zb2xlIiwibG9nIiwic2F2ZV9tZXNzYWdlIiwidmFsIiwiZGF0YVR5cGUiLCJtc2ciLCJoZWlnaHQiLCJwcm9wIiwiZWFjaCIsInBhcnNlSW50IiwiYW5pbWF0ZSIsInNjcm9sbFRvcCIsImRhdGFzIiwiaWRGIiwiZW1wdHkiLCJpIiwibWVzc2FnZSIsImlkRmFjaWxpdGF0ZXVyIiwiYXBwZW5kIiwiYWRkQ2xhc3MiLCJ0ZXh0IiwicHJlbm9tIiwibm9tIiwiY3NzIl0sIm1hcHBpbmdzIjoiO0FBQUE7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7OztBQUdBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLGFBQUs7QUFDTDtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBLG1DQUEyQiwwQkFBMEIsRUFBRTtBQUN2RCx5Q0FBaUMsZUFBZTtBQUNoRDtBQUNBO0FBQ0E7O0FBRUE7QUFDQSw4REFBc0QsK0RBQStEOztBQUVySDtBQUNBOztBQUVBO0FBQ0E7Ozs7Ozs7Ozs7Ozs7QUM3REFB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QUFGLEVBQVdzQyxRQUFYLENBQW9CLGlDQUFwQixDQUF2QixFQUNLRCxNQURMLENBQ1lyQyxFQUFFLE9BQUYsRUFBV3NDLFFBQVgsQ0FBb0IsbUJBQXBCLEVBQ0hELE1BREcsQ0FDSXJDLEVBQUUsS0FBRixFQUFTdUMsSUFBVCxDQUFjSixRQUFRSyxNQUFSLEdBQWlCLEdBQWpCLEdBQXVCTCxRQUFRTSxHQUE3QyxFQUFrREgsUUFBbEQsQ0FBMkQsb0NBQTNELEVBQWlHSSxHQUFqRyxDQUFxRyxPQUFyRyxFQUE4RyxPQUE5RyxDQURKLEVBRUhMLE1BRkcsQ0FFSXJDLEVBQUUsS0FBRixFQUFTdUMsSUFBVCxDQUFjSixRQUFRSSxJQUF0QixDQUZKLENBRFo7QUFJSDtBQUNKOztBQUVEdkMsTUFBRSxZQUFGLEVBQWdCOEIsU0FBaEIsQ0FBMEI5QixFQUFFLFlBQUYsRUFBZ0IwQixJQUFoQixDQUFxQixjQUFyQixDQUExQjtBQUNILENBcEJELEM7Ozs7Ozs7Ozs7Ozs7QUNyREEsd0IiLCJmaWxlIjoianMvc2NyaXB0L2NoYXQuanMiLCJzb3VyY2VzQ29udGVudCI6WyIgXHQvLyBUaGUgbW9kdWxlIGNhY2hlXG4gXHR2YXIgaW5zdGFsbGVkTW9kdWxlcyA9IHt9O1xuXG4gXHQvLyBUaGUgcmVxdWlyZSBmdW5jdGlvblxuIFx0ZnVuY3Rpb24gX193ZWJwYWNrX3JlcXVpcmVfXyhtb2R1bGVJZCkge1xuXG4gXHRcdC8vIENoZWNrIGlmIG1vZHVsZSBpcyBpbiBjYWNoZVxuIFx0XHRpZihpbnN0YWxsZWRNb2R1bGVzW21vZHVsZUlkXSkge1xuIFx0XHRcdHJldHVybiBpbnN0YWxsZWRNb2R1bGVzW21vZHVsZUlkXS5leHBvcnRzO1xuIFx0XHR9XG4gXHRcdC8vIENyZWF0ZSBhIG5ldyBtb2R1bGUgKGFuZCBwdXQgaXQgaW50byB0aGUgY2FjaGUpXG4gXHRcdHZhciBtb2R1bGUgPSBpbnN0YWxsZWRNb2R1bGVzW21vZHVsZUlkXSA9IHtcbiBcdFx0XHRpOiBtb2R1bGVJZCxcbiBcdFx0XHRsOiBmYWxzZSxcbiBcdFx0XHRleHBvcnRzOiB7fVxuIFx0XHR9O1xuXG4gXHRcdC8vIEV4ZWN1dGUgdGhlIG1vZHVsZSBmdW5jdGlvblxuIFx0XHRtb2R1bGVzW21vZHVsZUlkXS5jYWxsKG1vZHVsZS5leHBvcnRzLCBtb2R1bGUsIG1vZHVsZS5leHBvcnRzLCBfX3dlYnBhY2tfcmVxdWlyZV9fKTtcblxuIFx0XHQvLyBGbGFnIHRoZSBtb2R1bGUgYXMgbG9hZGVkXG4gXHRcdG1vZHVsZS5sID0gdHJ1ZTtcblxuIFx0XHQvLyBSZXR1cm4gdGhlIGV4cG9ydHMgb2YgdGhlIG1vZHVsZVxuIFx0XHRyZXR1cm4gbW9kdWxlLmV4cG9ydHM7XG4gXHR9XG5cblxuIFx0Ly8gZXhwb3NlIHRoZSBtb2R1bGVzIG9iamVjdCAoX193ZWJwYWNrX21vZHVsZXNfXylcbiBcdF9fd2VicGFja19yZXF1aXJlX18ubSA9IG1vZHVsZXM7XG5cbiBcdC8vIGV4cG9zZSB0aGUgbW9kdWxlIGNhY2hlXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLmMgPSBpbnN0YWxsZWRNb2R1bGVzO1xuXG4gXHQvLyBkZWZpbmUgZ2V0dGVyIGZ1bmN0aW9uIGZvciBoYXJtb255IGV4cG9ydHNcbiBcdF9fd2VicGFja19yZXF1aXJlX18uZCA9IGZ1bmN0aW9uKGV4cG9ydHMsIG5hbWUsIGdldHRlcikge1xuIFx0XHRpZighX193ZWJwYWNrX3JlcXVpcmVfXy5vKGV4cG9ydHMsIG5hbWUpKSB7XG4gXHRcdFx0T2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIG5hbWUsIHtcbiBcdFx0XHRcdGNvbmZpZ3VyYWJsZTogZmFsc2UsXG4gXHRcdFx0XHRlbnVtZXJhYmxlOiB0cnVlLFxuIFx0XHRcdFx0Z2V0OiBnZXR0ZXJcbiBcdFx0XHR9KTtcbiBcdFx0fVxuIFx0fTtcblxuIFx0Ly8gZ2V0RGVmYXVsdEV4cG9ydCBmdW5jdGlvbiBmb3IgY29tcGF0aWJpbGl0eSB3aXRoIG5vbi1oYXJtb255IG1vZHVsZXNcbiBcdF9fd2VicGFja19yZXF1aXJlX18ubiA9IGZ1bmN0aW9uKG1vZHVsZSkge1xuIFx0XHR2YXIgZ2V0dGVyID0gbW9kdWxlICYmIG1vZHVsZS5fX2VzTW9kdWxlID9cbiBcdFx0XHRmdW5jdGlvbiBnZXREZWZhdWx0KCkgeyByZXR1cm4gbW9kdWxlWydkZWZhdWx0J107IH0gOlxuIFx0XHRcdGZ1bmN0aW9uIGdldE1vZHVsZUV4cG9ydHMoKSB7IHJldHVybiBtb2R1bGU7IH07XG4gXHRcdF9fd2VicGFja19yZXF1aXJlX18uZChnZXR0ZXIsICdhJywgZ2V0dGVyKTtcbiBcdFx0cmV0dXJuIGdldHRlcjtcbiBcdH07XG5cbiBcdC8vIE9iamVjdC5wcm90b3R5cGUuaGFzT3duUHJvcGVydHkuY2FsbFxuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5vID0gZnVuY3Rpb24ob2JqZWN0LCBwcm9wZXJ0eSkgeyByZXR1cm4gT2JqZWN0LnByb3RvdHlwZS5oYXNPd25Qcm9wZXJ0eS5jYWxsKG9iamVjdCwgcHJvcGVydHkpOyB9O1xuXG4gXHQvLyBfX3dlYnBhY2tfcHVibGljX3BhdGhfX1xuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5wID0gXCIvYnVpbGQvXCI7XG5cbiBcdC8vIExvYWQgZW50cnkgbW9kdWxlIGFuZCByZXR1cm4gZXhwb3J0c1xuIFx0cmV0dXJuIF9fd2VicGFja19yZXF1aXJlX18oX193ZWJwYWNrX3JlcXVpcmVfXy5zID0gXCIuL2Fzc2V0cy9qcy9zY3JpcHQvY2hhdC5qc1wiKTtcblxuXG5cbi8vIFdFQlBBQ0sgRk9PVEVSIC8vXG4vLyB3ZWJwYWNrL2Jvb3RzdHJhcCA4YTQ4MDhiMDcwMDlkY2I1Y2Y0YSIsIiQoZG9jdW1lbnQpLnJlYWR5KGZ1bmN0aW9uICgpIHtcclxuICAgIHZhciBpZCA9ICQoXCIuc2VhbmNlXCIpLmF0dHIoXCJpZFwiKTtcclxuICAgIGFmZmljaGFnZV9tZXNzYWdlcyhpZCk7XHJcbn0pO1xyXG5cclxudmFyIHJlZnJlc2hNc2c7XHJcbi8vcGVybWV0IGQnYWZmaWNoZXIgbGVzIG1lc3NhZ2VzIGR1IGNoYXRcclxud2luZG93LmFmZmljaGFnZV9tZXNzYWdlcyA9IGZ1bmN0aW9uKGlkKSB7XHJcbiAgICAkLmFqYXgoe1xyXG4gICAgICAgIHR5cGU6IFwiR0VUXCIsXHJcbiAgICAgICAgdXJsOiAgd2luZG93LmxvY2F0aW9uLm9yaWdpbiArIFwiL2NoYXQvZ2V0L21lc3NhZ2Uvc2VhbmNlL1wiICsgaWQsXHJcbiAgICAgICAgc3VjY2VzczogZnVuY3Rpb24gKGRhdGEpIHtcclxuICAgICAgICAgICAgY3JlYXRpb25fY2hhdChkYXRhLCBpZCk7XHJcbiAgICAgICAgICAgIGNsZWFyVGltZW91dChyZWZyZXNoTXNnKTtcclxuICAgICAgICAgICAgLy9yZWZyZXNoIHRvdXQgbGVzIDUwMCBtaWxpc2Vjb25kZXNcclxuICAgICAgICAgICAgcmVmcmVzaE1zZyA9IHNldFRpbWVvdXQoZnVuY3Rpb24gKCkge1xyXG4gICAgICAgICAgICAgICAgYWZmaWNoYWdlX21lc3NhZ2VzKGlkKTtcclxuICAgICAgICAgICAgfSwgNTAwKTtcclxuICAgICAgICB9LFxyXG4gICAgICAgIGVycm9yOiBmdW5jdGlvbiAoKSB7XHJcbiAgICAgICAgICAgIGNvbnNvbGUubG9nKFwiZXJyb3JcIik7XHJcbiAgICAgICAgfVxyXG4gICAgfSk7XHJcbn07XHJcblxyXG4vL3Blcm1ldCBkJ2Vudm95ZXIgdW4gbWVzc2FnZSBlbiBiYXNlIGRlIGRvbm7DqWVzXHJcbndpbmRvdy5zYXZlX21lc3NhZ2UgPSBmdW5jdGlvbihpZCkge1xyXG4gICAgaWYgKCQoXCIjdGV4dFpvbmVcIikudmFsKCkgIT09IFwiXCIpIHtcclxuICAgICAgICAkLmFqYXgoe1xyXG4gICAgICAgICAgICB0eXBlOiBcIlBPU1RcIixcclxuICAgICAgICAgICAgdXJsOiB3aW5kb3cubG9jYXRpb24ub3JpZ2luICsgXCIvY2hhdC9wb3N0L21lc3NhZ2Uvc2VhbmNlL1wiICsgaWQsXHJcbiAgICAgICAgICAgIGRhdGFUeXBlOiBcImpzb25cIixcclxuICAgICAgICAgICAgZGF0YToge1xyXG4gICAgICAgICAgICAgICAgbXNnOiAkKFwiI3RleHRab25lXCIpLnZhbCgpXHJcbiAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgIHN1Y2Nlc3M6IGZ1bmN0aW9uICgpIHtcclxuICAgICAgICAgICAgICAgIC8vcGVybWV0IGRlIGZpeGVyIGxhIHNjcm9sbCBiYXIgw6AgY2hhcXVlIHJlZnJlc2hcclxuICAgICAgICAgICAgICAgIHZhciBoZWlnaHQgPSAkKCcjbXNnX2xpc3RlJykucHJvcChcInNjcm9sbEhlaWdodFwiKTtcclxuICAgICAgICAgICAgICAgICQoJ2RpdiBwJykuZWFjaChmdW5jdGlvbigpe1xyXG4gICAgICAgICAgICAgICAgICAgIGhlaWdodCArPSBwYXJzZUludCgkKHRoaXMpLmhlaWdodCgpKTtcclxuICAgICAgICAgICAgICAgIH0pO1xyXG5cclxuICAgICAgICAgICAgICAgIGhlaWdodCArPSAnJztcclxuXHJcbiAgICAgICAgICAgICAgICAkKCdkaXYnKS5hbmltYXRlKHtzY3JvbGxUb3A6IGhlaWdodH0pO1xyXG5cclxuICAgICAgICAgICAgICAgICQoXCIjdGV4dFpvbmVcIikudmFsKFwiXCIpO1xyXG4gICAgICAgICAgICB9LFxyXG4gICAgICAgIH0pO1xyXG4gICAgfVxyXG59O1xyXG5cclxuLy9wZXJtZXQgZGUgY29uc3RydWlyIGxlIGNoYXQgw6AgY2hhcXVlIHJlZnJlc2hcclxud2luZG93LmNyZWF0aW9uX2NoYXQgPSBmdW5jdGlvbihkYXRhcykge1xyXG4gICAgdmFyIGlkRiA9ICQoXCIudXNlclwiKS5hdHRyKFwiaWRcIik7XHJcbiAgICAkKFwiI21zZ19saXN0ZVwiKS5lbXB0eSgpO1xyXG4gICAgZm9yICh2YXIgaSBpbiBkYXRhcykge1xyXG4gICAgICAgIHZhciBtZXNzYWdlID0gZGF0YXNbaV07XHJcbiAgICAgICAgaWYoaWRGID09IG1lc3NhZ2UuaWRGYWNpbGl0YXRldXIpIHtcclxuICAgICAgICAgICAgJChcIiNtc2dfbGlzdGVcIikuYXBwZW5kKCQoXCI8ZGl2PlwiKS5hZGRDbGFzcyhcImQtZmxleCBqdXN0aWZ5LWNvbnRlbnQtc3RhcnQgbWItNFwiKSlcclxuICAgICAgICAgICAgICAgIC5hcHBlbmQoJChcIjxkaXY+XCIpLmFkZENsYXNzKFwibXNnX2NvdGFpbmVyXCIpXHJcbiAgICAgICAgICAgICAgICAgICAgLmFwcGVuZCgkKFwiPHA+XCIpLnRleHQobWVzc2FnZS5wcmVub20gKyBcIiBcIiArIG1lc3NhZ2Uubm9tKS5hZGRDbGFzcyhcImZvbnQtd2VpZ2h0LWJvbGQgYmxvY2txdW90ZS1mb290ZXJcIikuY3NzKFwiY29sb3JcIiwgXCJ3aGl0ZVwiKSlcclxuICAgICAgICAgICAgICAgICAgICAuYXBwZW5kKCQoXCI8cD5cIikudGV4dChtZXNzYWdlLnRleHQpKSk7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIGVsc2Uge1xyXG4gICAgICAgICAgICAkKFwiI21zZ19saXN0ZVwiKS5hcHBlbmQoJChcIjxkaXY+XCIpLmFkZENsYXNzKFwiZC1mbGV4IGp1c3RpZnktY29udGVudC1lbmQgbWItNFwiKSlcclxuICAgICAgICAgICAgICAgIC5hcHBlbmQoJChcIjxkaXY+XCIpLmFkZENsYXNzKFwibXNnX2NvdGFpbmVyX3NlbmRcIilcclxuICAgICAgICAgICAgICAgICAgICAuYXBwZW5kKCQoXCI8cD5cIikudGV4dChtZXNzYWdlLnByZW5vbSArIFwiIFwiICsgbWVzc2FnZS5ub20pLmFkZENsYXNzKFwiZm9udC13ZWlnaHQtYm9sZCBibG9ja3F1b3RlLWZvb3RlclwiKS5jc3MoXCJjb2xvclwiLCBcImJsYWNrXCIpKVxyXG4gICAgICAgICAgICAgICAgICAgIC5hcHBlbmQoJChcIjxwPlwiKS50ZXh0KG1lc3NhZ2UudGV4dCkpKTtcclxuICAgICAgICB9XHJcbiAgICB9XHJcblxyXG4gICAgJCgnI21zZ19saXN0ZScpLnNjcm9sbFRvcCgkKCcjbXNnX2xpc3RlJykucHJvcChcInNjcm9sbEhlaWdodFwiKSk7XHJcbn07XG5cblxuLy8gV0VCUEFDSyBGT09URVIgLy9cbi8vIC4vYXNzZXRzL2pzL3NjcmlwdC9jaGF0LmpzIiwibW9kdWxlLmV4cG9ydHMgPSBqUXVlcnk7XG5cblxuLy8vLy8vLy8vLy8vLy8vLy8vXG4vLyBXRUJQQUNLIEZPT1RFUlxuLy8gZXh0ZXJuYWwgXCJqUXVlcnlcIlxuLy8gbW9kdWxlIGlkID0ganF1ZXJ5XG4vLyBtb2R1bGUgY2h1bmtzID0gMiAzIDQgNSA2IDcgOCA5IDEwIDExIDEyIDEzIDE0IDE1IDE2IDE3IDE4IDE5IDIwIDIxIl0sInNvdXJjZVJvb3QiOiIifQ==