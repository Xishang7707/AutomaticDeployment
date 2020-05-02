function post({ url, async = true, data, done, err }) {
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