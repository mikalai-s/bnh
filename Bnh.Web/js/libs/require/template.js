define({
    load: function (name, req, load, config) {
        req(['text!' + name], function (value) {
            var re = /([\w\/]+)\./im;
            var id = name.match(re)[1].replace('/', '_');
            var head = document.getElementsByTagName('head')[0];
            var script = document.createElement('script');
            script.type = 'text/html';
            script.id = id;
            script.text = value;
            head.appendChild(script);
            load(value);
        });
    }
});