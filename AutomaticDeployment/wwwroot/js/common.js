﻿function post({ url, async = true, data, done, err }) {
    $.ajax({
        url: url,
        type: 'post',
        headers: {
            'content-type': 'application/json'
        },
        data: JSON.stringify(data),
        processData: false,
        async: async,
        success: o => {
            done && done(o);
        },
        error: o => {
            if (o.responseJSON)
                err && err(o.responseJSON);
            else err && err({ status: false, msg: '服务器请求异常' });
        }
    })
}

function get({ url, async = true, data, done, err }) {
    $.ajax({
        url: url,
        type: 'get',
        data: JSON.stringify(data),
        async: async,
        success: o => {
            done && done(o);
        },
        error: o => {
            if (o.responseJSON)
                err && err(o.responseJSON);
            else err && err({ status: false, msg: '服务器请求异常' });
        }
    })
}

function get_selected(sor) {
    return $(sor).siblings('div.layui-form-select').find('dl dd.layui-this').attr('lay-value');
}

/**
 * 获取顶层window
 */
function get_top_window() {
    var p = window.parent;
    while (p != p.window.parent) {
        p = p.window.parent;
    } return p;
}

function getQuery(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) { return pair[1]; }
    }
    return '';
}

//打开页面
function open_page(title, path, id) {
    var w = get_top_window();
    if (w.open_tab) {
        w.open_tab(title, path, id);
    } else {
        w.open(path.substring(path.indexOf('/')), '_blank');
    }
}