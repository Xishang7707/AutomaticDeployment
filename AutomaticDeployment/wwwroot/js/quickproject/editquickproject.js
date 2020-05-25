var form;
$(function () {
    var project_uid = getQuery('project_uid');
    if (!project_uid)
        return;

    layui.use(['carousel', 'form'], function () {
        var carousel = layui.carousel;
        form = layui.form;
        get_project(project_uid);

        var step_el = init_dev_steps();

        //建造实例
        carousel.render({
            elem: '#dev_step'
            , width: '75%' //设置容器宽度
            , height: '80%'
            , arrow: 'always' //始终显示箭头
            , indicator: 'none'
            , autoplay: false
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
            var index = step_el.getActive(); if (index + 1 == step_config.length) return publish(); if (check_step(index)) add_step_el.click();
        });

        carousel.on('change(dev_step)', function (obj) {
            step_el.setActive(obj.index);
            if (obj.index == 0) {
                btn_step_sub.hide();
            }
            else if (obj.index + 1 == step_config.length) {
                btn_step_add.text('提交');
            }
            else {
                btn_step_sub.show();
                btn_step_add.text('下一步');
            }
        });

    });
});

var step_config = [
    { title: "说明", description: "", },
    { title: "项目", description: "", verify: verify_step_project, verify_data: get_step_project },
    { title: "服务器", description: "", verify: verify_step_server, verify_data: get_step_server },
    { title: "发布信息", description: "", verify: verify_step_publish, verify_data: get_step_publish }
];

function init_dev_steps() {
    return steps({
        el: "#dev_steps",
        data: step_config,
        active: 0,
        center: true,
        sides: "start-single",
        iconType: "bullets"
    });
}

function get_step_server() {
    var data = {
        server_platform: get_selected('#step_server select[name=server_platform]'),
        server_ip: $('#step_server input[name=server_ip]').val(),
        server_port: $('#step_server input[name=server_port]').val(),
        server_account: $('#step_server input[name=server_account]').val(),
        server_password: $('#step_server input[name=server_password]').val()
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

function get_step_project() {
    var data = {
        project_guid: $('#step_project input[name=project_uid]').val(),
        project_name: $('#step_project input[name=project_name]').val(),
        project_classify: get_select_input('#step_project select[name=project_classify]'),
        project_remark: $('#step_project textarea[name=project_remark]').val()
    };
    return data;
}

function verify_step_project(data) {

    if (!data['project_name']) {
        layer.msg('请填写项目名称');
        return false;
    }

    if (data['project_classify'].length > 20) {
        layer.msg('项目归类最多20个字符');
        return false;
    }

    if (data['project_remark'].length > 200) {
        layer.msg('项目描述最多200个字符');
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
    if (!(step_config[index].verify && step_config[index].verify_data))
        return true;

    return step_config[index].verify && step_config[index].verify_data && step_config[index].verify(step_config[index].verify_data());

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

    layer.load();
    post({
        url: '../api/quickproject/editproject',
        data: data,
        done: o => {
            layer.closeAll();
            tw.layer.msg(o.msg);
            close_page();
        },
        err: o => {
            layer.closeAll();
            layer.msg(o.msg);
        }
    });
}

function get_project(id) {
    get({
        url: '../api/quickproject/getproject?project_uid=' + id,
        done: o => {
            render_project(o.data);
            get_classify(o.data['project_classify']);
        },
        err: o => {
            layer.msg(o.msg);
        }
    })
}

function render_project(data) {
    //项目
    $('#step_project input[name=project_uid]').val(data['project']['project_uid']);
    $('#step_project input[name=project_name]').val(data['project']['project_name']);
    $('#step_project textarea[name=project_remark]').val(data['project']['project_remark']);

    //服务器
    $('#step_server input[name=server_ip]').val(data['server']['server_ip']);
    $('#step_server input[name=server_port]').val(data['server']['server_port']);
    $('#step_server input[name=server_account]').val(data['server']['server_account']);
    $('#step_server input[name=server_password]').val(data['server']['server_password']);

    //发布
    $('#step_publish input[name=publish_path]').val(data['publish']['publish_path']);
    $('#step_publish textarea[name=publish_before_command]').val(data['publish']['publish_before_command']);
    $('#step_publish textarea[name=publish_after_command]').val(data['publish']['publish_after_command']);
}

function get_classify(sed) {
    render_select({
        sor: '#step_project select[name=project_classify]',
        el: form,
        sed: sed,
        url: '../api/quickproject/getclassify',
        done: o => {
            $('#step_project select[name=project_classify]').next().find('div input.layui-input').unbind('blur');
        }
    });
}