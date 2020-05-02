$(function () {
    layui.use(['carousel', 'upload'], function () {
        var carousel = layui.carousel;
        var upload = layui.upload;

        var developer_config = {
            server: {
                server_ip: '',
                server_port: '',
                server_account: '',
                server_password: ''
            },
            project: {
                file_id: ''
            },
            publish: {
                publish_path: '',
                publish_before_command: '',
                publish_after_command: ''
            }
        };

        var step_el = init_dev_steps();
        var step_total = 4;

        //建造实例
        carousel.render({
            elem: '#dev_step'
            , width: '75%' //设置容器宽度
            , height: '80%'
            , arrow: 'always' //始终显示箭头
            , indicator: 'none'
            , autoplay: false
            //,anim: 'updown' //切换动画方式
        });

        upload.render({
            elem: '#project_file' //绑定元素
            , url: 'api/upload/uploaddemo' //上传接口
            , accept: 'file'
            , choose: function (obj) {
                $('#step_project input[name=file_id]').val('');
            }
            , done: function (res) {
                $('#step_project input[name=file_id]').val(res.id);
                //上传完毕回调
                layer.msg(res.msg);
            }
            , error: function () {
                layer.msg('上传失败');
            }
        });

        var sub_step_el = $('#dev_step > button[lay-type="sub"]');
        var add_step_el = $('#dev_step > button[lay-type="add"]');
        sub_step_el.hide();
        add_step_el.hide();

        var btn_step_sub = $('#btn_step_sub');
        var btn_step_add = $('#btn_step_add');
        btn_step_sub.hide();
        btn_step_add.show();

        btn_step_sub.click(() => { sub_step_el.click(); });
        btn_step_add.click(() => {
            var index = step_el.getActive(); if (index + 1 == step_total) return publish(); if (check_step(index)) add_step_el.click();
        });

        carousel.on('change(dev_step)', function (obj) { //test1来源于对应HTML容器的 lay-filter="test1" 属性值
            step_el.setActive(obj.index);
            if (obj.index == 0) {
                btn_step_sub.hide();
            }
            else if (obj.index + 1 == step_total) {
                btn_step_add.text('发布');
            }
            else {
                btn_step_sub.show();
                btn_step_add.text('下一步');
            }
        });

    });
});


function init_dev_steps() {
    return steps({
        el: "#dev_steps",
        data: [
            { title: "说明", description: "" },
            { title: "服务器配置", description: "" },
            { title: "项目配置", description: "" },
            { title: "发布", description: "" }
        ],
        active: 0,
        center: true,
        sides: "start-single",
        iconType: "bullets"
    });
}

function get_step_server() {
    var data = {
        server_ip: $('#step_server input[name=server_ip]').val(),
        server_port: $('#step_server input[name=server_port]').val(),
        server_account: $('#step_server input[name=server_account]').val(),
        server_password: $('#step_server input[name=server_password]').val()
    };
    return data;
}

function verify_step_server(data) {
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

function get_step_project() {
    var data = {
        file_id: $('#step_project input[name=file_id]').val()
    };
    return data;
}

function verify_step_project(data) {

    if (!data['file_id']) {
        layer.msg('请上传发布文件');
        return false;
    }

    return true;
}

function get_step_publish() {
    var data = {
        publish_path: $('#step_publish input[name=publish_path]').val(),
        publish_before_command: $('#step_publish textarea[name=publish_before_command]').val(),
        publish_after_command: $('#step_publish textarea[name=publish_after_command]').val()
    };
    return data;
}

function verify_step_publish(data) {

    if (!data['publish_path']) {
        layer.msg('请填写项目的发布路径');
        return false;
    }

    return true;
}

function check_step(index) {
    switch (index) {
        case 1:
            return verify_step_server(get_step_server());
        case 2:
            return verify_step_project(get_step_project());
        case 3:
            return verify_step_publish(get_step_publish());
        default:
            return true;
    }
}

function publish() {
    var data = {
        server: get_step_server(),
        project: get_step_project(),
        publish: get_step_publish()
    }
    if (!verify_step_server(data['server'])) {
        return false;
    }
    if (!verify_step_project(data['project'])) {
        return false;
    }
    if (!verify_step_publish(data['publish'])) {
        return false;
    }

    layer.confirm('确认发布？', {
        btn: ['发布', '取消'] //可以无限个按钮
    }, function (index, layero) {
        layer.close(index);
        layer.load();
        post({
            url: 'api/demo/publishdemo',
            data: data,
            done: o => {
                layer.closeAll();
                layer.msg(o.msg);
            },
            err: o => {
                layer.closeAll();
                layer.msg(o.msg);
            }
        })
    }, function (index) {
        layer.close(index);
    });
}