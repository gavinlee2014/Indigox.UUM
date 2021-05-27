require(
    [
        "Indigox.Web.JsLib.System.Application",
        "Indigox.Web.JsLib.Collection.List",
        "Indigox.Web.JsLib.Utils.Util",
        "Indigox.Web.JsLib.Utils.DOMUtil",
        "Indigox.Web.JsLib.Controls.Control",
        "Indigox.Web.JsLib.Controls.UIEngine",
        "Indigox.Web.JsLib.Controls",
        "Indigox.Web.JsLib.Controls.Grid",
        "Indigox.Web.JsLib.Configuration.ControlUIsConfiguration",
        "Indigox.Web.JsLib.Configuration.ValidatorRuleConfiguration",
        "Indigox.Web.JsLib.Configuration.SchemasConfiguration",
        "Indigox.Web.JsLib.Configuration.MappingsConfiguration",
        "Indigox.Web.JsLib.Configuration.MediatorConfiguration",
        "Indigox.Web.JsLib.Configuration.PluginConfiguration",
        "Indigox.Web.JsLib.Controls.Html",
        "Indigox.Web.JsLib.DOM.Accessors.JQueryAccessor",
        "Indigox.Web.JsLib.Utils.UrlAdapter"
    ],
    function (
        Application,
        List,
        Util,
        DOMUtil,
        Control,
        UIEngine,
        Controls
    ) {
        var ControlsNamespace = Controls;

        /**
        * Usage：
        *
        * (1) $( controlName )
        *     返回 UIEngine，等同 uiEngine.Control( controlName )
        *     例如：$('textbox1')
        *
        * (2) $.{controlType}( controlName )
        *     返回 UIEngine，等同 uiEngine.{controlType}( controlName )。
        *     例如：$.TextBox('textbox1')
        *
        * (3) $(control)
        *     返回 UIEngine，等同 new UIEngine( [control], controlType )
        *
        * (4) $(callback)
        *     页面加载完成后执行 callback
        *
        */

        /**
        * Samples:
        *
        * (1) 查找控件
        *   (a) 查找 name='textbox1' 的任意类型的控件
        *       var textbox1 = $('textbox1').first();
        *
        *   (b) 查找 name='textbox1' 的 TextBox
        *       var textbox1 = $.TextBox('textbox1').first();
        *
        *   (c) 已知 container 对象，在 container 所有子控件中，查找 name='textbox1' 的 TextBox
        *       var textbox1 = $(container).TextBox('textbox1').first();
        *
        * (2) 设置控件的属性
        *   (a) 设置 name='textbox1' 的任意类型的控件的 value 属性
        *       $('textbox1').setValue(value);
        *
        *   (b) 设置 name='textbox1' 的 TextBox 的 text 属性
        *       $.TextBox('textbox1').setText(text);
        *
        *   (c) 已知 container 对象，设置 container 子控件中 name='textbox1' 的 TextBox 的 text 属性
        *       $(container).TextBox('textbox1').setText(text);
        *
        *   (d) 已知 control 对象，设置 control 的 value 属性
        *       $(control).setValue(value);
        *
        * (3) 获取控件的属性
        *   (a) 获取 name='textbox1' 的任意类型的控件的 value 属性
        *       var value = $('textbox1').getValue();
        *
        *   (b) 获取 name='textbox1' 的 TextBox 的 text 属性
        *       var text = $.TextBox('textbox1').getText();
        *
        *   (c) 已知 container 对象，获取 container 子控件中 name='textbox1' 的 TextBox 的 text 属性
        *       var text = $(container).TextBox('textbox1').getText();
        *
        *   (d) 已知 control 对象，获取 control 的 value 属性
        *       var value = $(control).getValue();
        *
        */

        var init = function (arg) {
            if (isString(arg)) {
                return UIEngine.find(arg, Control);
            }
            else if (isObject(arg)) {
                var list = new List();
                list.add(arg);
                return new UIEngine(list, arg.constructor);
            }
            else if (isFunction(arg)) {
                return Page().execute(arg);
            }
        };

        var finder = {};
        finder.Control = function (name) {
            return UIEngine.find(name, Control);
        };
        var buildFinder = function (namespace) {
            for (var cls in namespace) {
                if (namespace[cls].prototype instanceof Control) {
                    finder[cls] = (function (cls) {
                        return function (name) {
                            return UIEngine.find(name, namespace[cls]);
                        };
                    } (cls));
                }
                else if (isObject(namespace[cls])) {
                    buildFinder(namespace[cls]);
                }
            }
        };
        buildFinder(ControlsNamespace);

        var engine = UIEngine.prototype;
        engine.Control = function (name) {
            return this.searchDescendants(name, Control);
        };
        var buildFindEngine = function (namespace) {
            for (var cls in namespace) {
                if (namespace[cls].prototype instanceof Control) {
                    engine[cls] = (function (cls) {
                        return function (name) {
                            return this.searchDescendants(name, namespace[cls]);
                        };
                    } (cls));
                }
                else if (isObject(namespace[cls])) {
                    buildFindEngine(namespace[cls]);
                }
            }
        };
        buildFindEngine(ControlsNamespace);

        Util.copy(init, finder);
        window.$ = init;

        (function () {
            var app = new Application();

            ////@debugger
            //app.watcher = {
            //    logs: [],
            //    start: {},  // <id, datetime>
            //    total: 0,   // ms
            //    time: function (id) {
            //        this.start[id] = new Date();
            //    },
            //    timeEnd: function (id) {
            //        var spend = new Date() - this.start[id];
            //        this.logs.push([id, ": " + spend].join(""));
            //        this.total += spend;
            //        delete this.start[id];
            //    },
            //    reset: function () {
            //        this.logs = [];
            //        this.start = {};
            //        this.total = 0;
            //    },
            //    print: function () {
            //        debug.log([this.total, "ms\tcontrol load/insert processed."].join(""));
            //    }
            //};
            //
            //var page = Page();
            //page.on("loading", function () {
            //    app.watcher.reset();
            //});
            //page.on("loaded", function () {
            //    app.watcher.print();
            //});

            window.Application = app;

            if (document.readyState != 'complete') {
                DOMUtil.attachEvent(document, "ready", function () {
                    app.onDomReady();
                });
                DOMUtil.attachEvent(window, "load", function () {
                    app.onDocumentLoad();
                });
            }
            else {
                app.onDomReady();
                app.onDocumentLoad();
            }
            DOMUtil.attachEvent(window, "unload", function () {
                app.onDocumentUnload();
            });
        } ());
    });