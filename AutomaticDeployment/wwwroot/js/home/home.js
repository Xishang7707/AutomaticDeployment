$(function () {
    get_service_check();
});

function get_service_check() {
    //local-service
    get({
        url: '../api/environment/checklocalbashes',
        done: o => {
            var data = o['data'];
            var dom = ``;
            for (var i = 0; i < data.length; i++) {
                var it = data[i];
                var state = ``;
                if (it['pass']) {
                    state = `<span style='color:#5FB878;'><i class='layui-icon'>&#x1005;</i>已安装</span>${(it['version'] ? `(${it['version']})` : ``)}`;
                } else {
                    state = `<span style='color:#FF5722;'><i class='layui-icon'>&#x1007;</i>未安装</span>`;
                }
                var item = `<li>${it['title']}(${it['bash']}) ${state}</li>`;
                dom += item;
            }

            $('#local-service .local-bashs').html(dom);
        },
        err: o => {
            tw.layer.msg(o.msg);
        }
    });
}