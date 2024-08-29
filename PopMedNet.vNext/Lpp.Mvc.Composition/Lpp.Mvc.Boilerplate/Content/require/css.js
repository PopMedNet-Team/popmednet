define('css', [], function () {
    return {
        load: function (name, require, load, config) {
            var head = document.getElementsByTagName('head')[0];
            var link = document.createElement('link');
            link.href = require.toUrl(name);
            link.rel = 'stylesheet';
            link.type = 'text/css';
            head.appendChild(link);
            load(true);
        }
    };
});