(function () {
    var console = window.console || {
        log: function () { },
        warn: function () { },
        error: function () { }
    }

    function isArray(value) {
        return (value instanceof Array);
    }

    function isFunction(value) {
        return (typeof value === "function");
    }

    function isString(value) {
        return (typeof value === "string");
    }

    function bind(obj, fn) {
        return function () {
            return fn.apply(obj, arguments);
        };
    }

    function contains(array, item) {
        for (var i = 0, length = array.length; i < length; i++) {
            if (array[i] == item) {
                return true;
            }
        }
        return false;
    }

    function forEach(array, func) {
        for (var i = 0, length = array.length; i < length; i++) {
            if (func(array[i], i, array)) {
                return;
            }
        }
    }

    function eachProp(obj, func) {
        for (var p in obj) {
            if (p in (obj)) {
                if (func(obj[p], p, obj)) {
                    return;
                }
            }
        }
    }

    var isIE = function (userAgent) {
        return userAgent.match(/(?:MSIE\s)([\d\._]+)/) ? true : false;
    } (navigator.userAgent);

    function combiePath(baseUrl, url) {
        if (typeof baseUrl === "undefined" || baseUrl === null || baseUrl === "") {
            return url;
        }

        var ar = [baseUrl];
        if (baseUrl.charAt(baseUrl.length - 1) !== "/") {
            ar.push("/");
        }
        ar.push(url);
        return ar.join("");
    }

    var hasAttachEvent;

    function addScriptNodeEvent(node, evt, obj, fn) {
        if (typeof hasAttachEvent === "undefined") {
            hasAttachEvent = node.attachEvent
                && !(node.attachEvent.toString && node.attachEvent.toString().indexOf("[native code") < 0);
        }

        if (fn == null) {
            fn = obj;
            obj = null;
        }

        if (hasAttachEvent) {
            //node.attachEvent("onreadystatechange", callback);
        } else {
            node.addEventListener(evt, bind(obj, fn), false);
        }
    }

    var globalDefQueue = [];

    var moduleManager, define, require, scriptLoader;

    var ScriptFile = function (url) {
        // the module name for noname module define
        this.moduleName = [];
        // all referer module names
        // when load script file error, set all referer module export as undefined
        this.refererModuleNames = [];
        this.url = url;
        this.state = ScriptFile.State.Create;
        this.events = {};
        this.node = null;
    };

    ScriptFile.State = {
        Create: 0,
        Loading: 1,
        Loaded: 2
    };

    var emptyDependencies = [];

    var emptyModuleFactory = function () {
        //return undefined;
    };

    ScriptFile.prototype = {
        onLoad: function () {
            if (this.loaded === true) {
                return;
            }

            var nonameModule = moduleManager.takeNonameModule();
            if (nonameModule != null) {
                var module = moduleManager.get(this.getModuleName());
                module.save(nonameModule.deps, nonameModule.factory);
            }

            this.loaded = true;
        },

        onError: function () {
            console.warn("Load script[" + this.url + "] error!");
            forEach(this.refererModuleNames, function (refererModuleName) {
                console.warn('Load module ' + refererModuleName + ' failed.');
                var module = moduleManager.get(refererModuleName);
                module.save(emptyDependencies, emptyModuleFactory);
            });
        },

        setModuleName: function (value) {
            this.moduleName = value;
        },

        getModuleName: function () {
            return this.moduleName;
        },

        addRefererModuleName: function (value) {
            this.refererModuleNames.push(value);
            //console.log(this.refererModuleNames);
        },

        load: function (module) {
            if (this.state != ScriptFile.State.Create) {
                return;
            }
            this.state = ScriptFile.State.Loading;
            var head = document.head || document.getElementsByTagName("head")[0] || document.documentElement;
            var node = document.createElement("script");
            node.src = this.url;
            node.type = "text/javascript";
            //node.charset = "utf-8";
            //node.async = true;
            addScriptNodeEvent(node, "load", this, this.onLoad);
            addScriptNodeEvent(node, "error", this, this.onError);
            this.node = node;
            head.appendChild(node);
        }
    };

    var Module = function (name, depends, factory) {
        this.name = name;
        this.depends = depends;
        this.factory = factory;
        this.state = 0;
        this.callbacks = [];
        this.exports = undefined;
        this.usingExports = false;
        this.events = {};
        this.defQueue = [];
    };

    Module.LISTENERS = {
        COMPILED: "onModuleCompiled",
        SAVED: "onModuleSaved"
    };

    Module.STATUS = {
        "FETCHING": 1,  // The module file is fetching now.
        "FETCHED": 2,   // The module file has been fetched.
        "SAVED": 3,     // The module info has been saved.
        "READY": 4,     // All dependencies and self are ready to compile.
        "COMPILING": 5, // The module is in compiling now.
        "COMPILED": 6   // The module is compiled and module.exports is available.
    };

    Module.prototype = {
        compile: function () {
            var me = this;
            me.state = Module.STATUS.COMPILING;

            var require = new Require(this.getDepends(), function () {
                var factory = me.getFactory();

                if (isFunction(factory)) {
                    if (me.usingExports === true) {
                        factory.apply(factory, arguments);
                    }
                    else {
                        var val = factory.apply(factory, arguments);
                        if (typeof val !== "undefined") {
                            me.exports = val;
                        }
                    }
                }
                else {
                    me.exports = factory;
                }
                me.state = Module.STATUS.COMPILED;
                me.fireListener(Module.LISTENERS.COMPILED);
            });
            require.module = me;

            if (require.check()) {
                window.setTimeout(function () {
                    require.execCallback(me);
                }, 0);
            }
        },

        save: function (dependencies, factory) {
            this.depends = dependencies;
            if (contains(this.depends, 'exports')) {
                this.usingExports = true;
                this.exports = {};
            }
            this.factory = factory;
            this.state = Module.STATUS.SAVED;
            this.fireListener(Module.LISTENERS.SAVED);
        },

        getName: function () {
            return this.name;
        },

        getDepends: function () {
            return this.depends;
        },

        getFactory: function () {
            return this.factory;
        },

        isCompiled: function () {
            return this.state === Module.STATUS.COMPILED;
        },

        isSaved: function () {
            return this.state === Module.STATUS.SAVED;
        },

        getDefName: function (node) {
            for (var i = 0, length = this.defQueue.length; i < length; i++) {
                var def = this.defQueue[i];
                if (def.node == node) {
                    return def.name;
                }
            }
        },

        addListener: function (event, listener) {
            if (!this.events.hasOwnProperty(event)) {
                this.events[event] = [];
            }
            if (isFunction(listener[event])) {
                this.events[event].push(listener);
            }
        },

        removeListener: function (event, listener) {
            if (this.events.hasOwnProperty(event)) {
                var events = this.events[event],
                    length = events.length;
                for (var i = length - 1; i >= 0; i--) {
                    if (events[i] == listener) {
                        events.splice(i, 1);
                    }
                }
            }
        },

        fireListener: function (event) {
            if (!(event in this.events)) {
                return;
                //throw new Error(["the listener \"", event, "\" not exist. "].join(""));
            }

            var events = this.events[event];
            var i = null, length = events.length;
            for (i = length - 1; i >= 0; i--) {
                events[i][event].apply(events[i], [this]);
            }
        }
    };

    var ModuleManager = function () {
        this.modules = {};
        this.scripts = {};
        this.config();
        this.nonameModule = null;
    };

    ModuleManager.prototype = {
        config: function (option) {
            option = option || {};

            this.baseUrl = option.baseUrl || "/res/js";
            this.paths = option.paths || {};
            this.libs = option.libs || {};
            this.packages = option.packages || [];

            eachProp(this.libs, function (value, name, libs) {
                if (typeof value === "string") {
                    libs[name] = {
                        path: name
                    };
                }
            });

            forEach(this.packages, function (item, i, packages) {
                if (typeof item === "string") {
                    packages[i] = {
                        name: item,
                        location: item
                    };
                }
            });
        },

        getUrl: function (name) {
            var url = name;
            if (url in this.paths) {
                url = this.paths[url];
            }
            var pkg = this.findPackage(url);
            if (pkg) {
                var location = pkg.location;
                if (this.isClassName(location)) {
                    url = this.classNameToPath(location, "package.js");
                }
            }
            if (this.isClassName(url)) {
                url = this.classNameToPath(url);
            }
            if (url.charAt(0) !== "/") { // absolute path
                url = combiePath(this.baseUrl, url);
            }
            if (!(/(\.(js|aspx|ashx)$)|\?|#/.test(url))) { // url
                url = url + ".js";
            }
            return url;
        },

        getScript: function (url) {
            if (url in this.scripts) {
                return this.scripts[url];
            }
            else {
                //console.log("Add script url :" + url);
                var scriptFile = new ScriptFile(url);
                this.scripts[url] = scriptFile;
                return scriptFile;
            }
        },

        getInteractiveScript: function () {
            var scripts = this.scripts;
            for (var url in scripts) {
                if (scripts[url].node.readyState === 'interactive') {
                    return scripts[url];
                }
            }
            return null;
        },

        isClassName: function (name) {
            return /^([a-zA-Z0-9_]+)(\.[a-zA-Z0-9_]+)+$/.test(name);
        },

        classNameToPath: function (name, mainfile) {
            eachProp(this.libs, function (lib, ns) {
                if (name.indexOf(ns) === 0) {
                    var path = lib.path;
                    var combiedPath = lib.combiedPath;
                    if (combiedPath) {
                        name = combiePath(path, combiedPath);
                    }
                    else {
                        name = combiePath(path, name.substring(ns.length + 1).replace(/\./g, "/"));
                        if (mainfile) {
                            name = combiePath(name, mainfile);
                        }
                    }
                    return true;
                }
            });
            return name;
        },

        findPackage: function (name) {
            var pkg = null;
            forEach(this.packages, function (item) {
                if (item.name === name) {
                    pkg = item;
                    return true;
                }
            });
            return pkg;
        },

        put: function (module) {
            /// <summary>
            /// put module definition
            /// </summary>
            /// <param name="module" type="Module"></param>

            var name = module.getName();

            if (this.isDefined(name)) {
                throw new Error("duplicate define module: " + name);
            }

            this.modules[name] = {
                definition: module
            };
        },

        get: function (name) {
            /// <summary>
            /// get module definition
            /// </summary>
            /// <param name="name" type="String">module name</param>
            /// <returns type="Module"></returns>

            if (!name) {
                return null;
            }
            else if (this.isDefined(name)) {
                return this.modules[name].definition;
            }
            else {
                var module = new Module(name, null, null);
                this.put(module);
                var script = this.getScript(this.getUrl(name));
                script.setModuleName(name);
                script.addRefererModuleName(name);
                script.load();
                return module;
            }
        },

        isDefined: function (name) {
            /// <summary>
            /// determining module is defined
            /// </summary>
            /// <param name="name" type="String">module name</param>
            /// <returns type="Boolean"></returns>
            return this.modules.hasOwnProperty(name);
        },

        putNonameModule: function (module) {
            if (this.nonameModule) {
                throw new Error("there was a module without name existed!");
            }
            else {
                this.nonameModule = module;
            }
        },

        takeNonameModule: function () {
            var module = this.nonameModule;
            this.nonameModule = null;
            return module;
        },

        require: function (deps, callback) {
            if (!isArray(deps) && isString(deps)) {
                deps = [deps];
            }

            var require = new Require(deps, callback);

            if (require.check()) {
                // 当所有资源都已经是就绪状态时，异步开始execCallback，尽可能保持和等待资源一样的逻辑
                window.setTimeout(function () {
                    require.execCallback();
                }, 0);
            }
        },

        define: function (name, deps, factory) {
            if (!isString(name)) {
                // throw new Error("Can"t define withour a name!");
                factory = deps;
                deps = name;
                name = null;
            }

            if (typeof factory === "undefined") { // factory maybe an array
                factory = deps;
                deps = [];
            }

            this.checkDuplicateDependency(deps);

            if (name != null) {
                var module = null;
                if (moduleManager.isDefined(name)) {
                    module = this.get(name);
                }
                else {
                    module = new Module(name, null, null);
                    this.put(module);
                }
                module.save(deps, factory);
            }
            else {
                this.putNonameModule({
                    deps: deps,
                    factory: factory
                });

                if (isIE) {
                    var script = moduleManager.getInteractiveScript();
                    if (script) {
                        script.onLoad();
                    }
                }
            }
        },

        checkDuplicateDependency: function (deps) {
            var duplicateDependency = null;
            var length = deps.length;
            for (var i = 0; i < length; i++) {
                for (var j = 0; j < i; j++) {
                    if (deps[i] === deps[j]) {
                        throw new Error("duplication dependency: " + deps[i]);
                    }
                }
            }
        }
    };

    var Require = function (dependencies, callback) {
        this.dependencies = dependencies;
        this.callback = callback;
        this.waitingDependencies = [];
        this.module = null;
    };

    Require.prototype = {
        check: function () {
            var deps = this.dependencies;
            var ready = true;
            var waitingLoadModules = [];

            for (var i = 0, length = deps.length; i < length; i++) {
                var dependency = deps[i];
                if (dependency == 'exports') {
                    continue;
                }

                var module = moduleManager.get(dependency);
                if (this.module && module.getDepends() && contains(module.getDepends(), this.module.getName())) {
                    console.warn(module.name + " circular dependency!");
                    continue;
                }
                else if (module.isCompiled()) {
                    continue;
                }
                else if (module.isSaved()) {
                    ready = false;
                    this.waitingDependencies.push(dependency);
                    module.addListener(Module.LISTENERS.COMPILED, this);
                    this.compileModule(module);
                }
                else {
                    ready = false;
                    this.waitingDependencies.push(dependency);
                    module.addListener(Module.LISTENERS.COMPILED, this);

                    module.addListener(Module.LISTENERS.SAVED, this);
                }
            }

            return ready;
        },

        compileModule: function (module) {
            //this.waitingDependencies.push(module.name);
            //module.addListener(Module.LISTENERS.COMPILED, this);

            if (module.state < Module.STATUS.COMPILING) {
                module.compile();
            }
        },

        execCallback: function (module) {
            var depExports = [];
            var deps = this.dependencies;
            for (var i = 0, length = deps.length; i < length; i++) {
                if (deps[i] == 'exports' && module) {
                    depExports.push(module.exports);
                }
                else {
                    depExports.push(moduleManager.get(deps[i]).exports);
                }
            }
            this.callback.apply(this.callback, depExports);
        },

        onModuleCompiled: function (module) {
            var deps = this.waitingDependencies;
            for (var i = 0, length = deps.length; i < length; i++) {
                if (deps[i] === module.getName()) {
                    this.waitingDependencies.splice(i, 1);
                    module.removeListener(Module.LISTENERS.COMPILED, this);
                    break;
                }
            }
            if (this.waitingDependencies.length === 0) {
                this.execCallback(this.module);
            }
        },

        onModuleSaved: function (module) {
            module.removeListener(Module.LISTENERS.SAVED, this);
            this.compileModule(module);
        }
    };

    moduleManager = new ModuleManager();

    window.define = bind(moduleManager, moduleManager.define);

    //@debugger
    window.define.modules = moduleManager.modules;

    window.define.amd = {
        jQuery: true
    };

    window.require = bind(moduleManager, moduleManager.require);

    window.require.config = bind(moduleManager, moduleManager.config);

    window.require.adapte = function (adaptee) {
        adaptee.setModuleManager(moduleManager);
    };
} ());