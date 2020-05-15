$(function () {
    $('#btn_service_sub').click(addservice);
    layui.use(['form'], () => {
        form = layui.form;
    });
});

function get_server() {
    var data = {
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
        layer.msg('请选择平台');
        return false;
    }
    if (!data['server_ip']) {
        layer.msg('请填写服务器ip');
        return false;
    }
    if (!verifyIp(data['server_ip'])) {
        layer.msg('请填写正确的ip地址，如127.0.0.1');
        return false;
    }
    if (!data['server_port']) {
        layer.msg('请填写服务器端口');
        return false;
    }
    if (!verifyPort(data['server_port'])) {
        layer.msg('请填写正确的端口号');
        return false;
    }

    if (!data['server_account']) {
        layer.msg('请填写服务器登录账号');
        return false;
    }

    if (!data['server_password']) {
        layer.msg('请填写服务器登录密码');
        return false;
    }
    return true;
}

function addservice() {
    var data = get_server();
    if (!verify_step_server(data)) {
        return false;
    }

    layer.load();
    post({
        url: '../api/service/addservice',
        data: data,
        done: o => {
            layer.closeAll();
            layer.msg(o.msg);
        },
        err: o => {
            layer.closeAll();
            layer.msg(o.msg);
        }
    });
}