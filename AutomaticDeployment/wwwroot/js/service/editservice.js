var _service_id;
$(function () {
    _service_id = getQuery('id');
    if (!_service_id)
        return;
    $('#btn_service_sub').click(editservice);
    layui.use(['form'], () => {
        form = layui.form;

        get_service(_service_id);
    });
});

function get_service(id) {
    get({
        url: '../api/service/getservice?id=' + id,
        done: o => {
            render_service(o['data']);
        },
        err: o => {
            get_top_window().layer.msg(o.msg);
        }
    })
}

function render_service(data) {
    $('#service_form input[name=server_ip]').val(data['server_ip']);
    $('#service_form input[name=server_port]').val(data['server_port']);
    $('#service_form input[name=server_account]').val(data['server_account']);
    $('#service_form input[name=server_password]').val(data['server_password']);
    $('#service_form input[name=server_space]').val(data['workspace']);

    form.render('select');
}

function get_server() {
    var data = {
        server_id: _service_id,
        server_platform: get_selected('#service_form select[name=server_platform]'),
        server_ip: $('#service_form input[name=server_ip]').val(),
        server_port: $('#service_form input[name=server_port]').val(),
        server_account: $('#service_form input[name=server_account]').val(),
        server_password: $('#service_form input[name=server_password]').val(),
        server_space: $('#service_form input[name=server_space]').val()
    };
    return data;
}

function verify_step_server(data) {
    if (!data['server_platform']) {
        tw.layer.msg('请选择平台');
        return false;
    }
    if (!data['server_ip']) {
        tw.layer.msg('请填写服务器ip');
        return false;
    }
    if (!verifyIp(data['server_ip'])) {
        tw.layer.msg('请填写正确的ip地址，如127.0.0.1');
        return false;
    }
    if (!data['server_port']) {
        tw.layer.msg('请填写服务器端口');
        return false;
    }
    if (!verifyPort(data['server_port'])) {
        tw.layer.msg('请填写正确的端口号');
        return false;
    }

    if (!data['server_account']) {
        tw.layer.msg('请填写服务器登录账号');
        return false;
    }

    if (!data['server_password']) {
        tw.layer.msg('请填写服务器登录密码');
        return false;
    }
    return true;
}

function editservice() {
    var data = get_server();
    if (!verify_step_server(data)) {
        return false;
    }

    layer.load();
    post({
        url: '../api/service/editservice',
        data: data,
        done: o => {
            layer.closeAll();
            tw.layer.msg(o.msg);
            close_page();
        },
        err: o => {
            layer.closeAll();
            tw.layer.msg(o.msg);
        }
    });
}