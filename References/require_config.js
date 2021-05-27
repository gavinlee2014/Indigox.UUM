(function () {
    function getCookie(name) {
        var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
        if (arr != null) return unescape(arr[2]); return null;
    }

    function setCookie(name, value) {
        document.cookie = name + "=" + escape(value) + ";path=/";
    }

    function getMode() {
        var mode = 'min';

        var cookie = getCookie('jslib-mode'),
        hostname = window.location.hostname,
        queryString = window.location.search;

        if (cookie) {
            mode = cookie;
        }
        //else if (/localhost/.test(hostname) || /.*\.dev\.indigox\.net/.test(hostname)) {
        //    mode = 'dev';
        //    setCookie('jslib-mode', mode);
        //}

        if (/(\?|&)debug/.test(queryString)) {
            mode = 'debug';
            setCookie('jslib-mode', mode);
        }
        else if (/(\?|&)min/.test(queryString)) {
            mode = 'min';
            setCookie('jslib-mode', mode);
        }
        else if (/(\?|&)dev/.test(queryString)) {
            mode = 'dev';
            setCookie('jslib-mode', mode);
        }

        return mode;
    }

    var mode = getMode();

    require.config({
        baseUrl: "/res/js",
        paths: {
            "jquery":                   "./jquery/jquery-1.9.1.min.js",
            "jquery-datepicker":        "./jquery/ui.datepicker.js",
            "jquery-datepicker-lang":   "./jquery/ui.datepicker-zh-CN.js",
            "jquery-history":           "./jquery/jquery.history.js",
            "jquery-slides":            "./jquery/slides.min.jquery.js",
            "jquery-resize":            "./jquery/jquery.ba-resize.min.js",
            "iscroll":                  "./iscroll/iscroll5.js",
            "swfobject":                "./swfobject/swfobject.js",
            "json2":                    "./json/json2.min.js",
            "highcharts":               "./highcharts/highcharts.js",
            "highcharts-more":          "./highcharts/highcharts-more.js",
            "highcharts-exporting":     "./highcharts/modules/exporting.js",
            "flowplayer":               "./flowplayer/flowplayer.min.js",
            "tinymce":                  "/editor/tinymce/tiny_mce.js",
            "tinymce-config":           "/editor/tinymce/config.js"
        },
        libs: {
            "Indigox.Web.JsLib": {
                path: "Indigox/Web/JsLib",
                combiedPath: (mode !== 'dev') ? ("jslib-" + mode) : null
            }
        },
        packages: [
            "Indigox.Web.JsLib.Core",
            "Indigox.Web.JsLib.Controls",
            "Indigox.Web.JsLib.Controls.Grid",
            "Indigox.Web.JsLib.Controls.Html",
            "Indigox.Web.JsLib.Controls.Plugins",
            "Indigox.Web.JsLib.Controls.Validation.Rules",
            "Indigox.Web.JsLib.DOM.Adapters",
            "Indigox.Web.JsLib.Formatters",
            "Indigox.Web.JsLib.UI.ControlUIs",
            "Indigox.Web.JsLib.UI.Mediators",
            "Indigox.Web.JsLib.UI.Schemas"
        ]
    });
} ());