var project_uid;
var _service_info;
$(function () {
    project_uid = getQuery('project_uid');
    if (!project_uid)
        return;

    layui.use(['carousel', 'upload', 'form'], function () {
        var carousel = layui.carousel;
        var form = layui.form;

        get_project(project_uid, form);

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

function get_step_project() {
    var data = {
        service_id: get_selected('#step_project select[name=project_service_id]'),
        project_name: $('#step_project input[name=project_name]').val(),
        code_souce_tool: get_selected('#step_project select[name=code_souce_tool]'),
        code_get_cmd: $('#step_project input[name=code_get_cmd]').val(),
        project_path: $('#step_project input[name=project_path]').val(),
        project_remark: $('#step_project textarea[name=project_remark]').val(),
    };
    return data;
}

function verify_step_project(data) {
    if (!data['service_id']) {
        layer.msg('请选择服务器');
        return false;
    }
    if (!data['project_name']) {
        layer.msg('请填写项目名称');
        return false;
    }
    if (!data['code_souce_tool']) {
        layer.msg('请选择代码工具');
        return false;
    }
    if (!data['code_get_cmd']) {
        layer.msg('请填写获取代码的命令');
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
        build_before_command: $('#step_publish input[name=build_before_command]').val(),
        build_command: $('#step_publish input[name=build_command]').val(),
        build_after_command: $('#step_publish input[name=build_after_command]').val(),
        publish_file_path: $('#step_publish input[name=publish_file_path]').val(),
        publish_before_command: $('#step_publish input[name=publish_before_command]').val(),
        publish_after_command: $('#step_publish input[name=publish_after_command]').val()
    };
    return data;
}

function verify_step_publish(data) {
    if (!data['build_command']) {
        layer.msg('请填写构建命令');
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
        project: get_step_project(),
        publish: get_step_publish(),
        project_uid: project_uid
    }

    if (!verify_step_project(data['project'])) {
        return false;
    }
    if (!verify_step_publish(data['publish'])) {
        return false;
    }

    layer.load();
    post({
        url: '../api/flowproject/editproject',
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

function get_service(form, id) {
    get({
        url: '../api/service/getdropservice',
        done: o => {
            var data = o['data'];
            _service_info = data;
            var dom = ``;
            for (var i = 0; i < data.length; i++) {
                var tmp = `<option value="${data[i]['value']}" ${data[i]['value'] == id ? 'selected' : ''}>${data[i]['name']}</option>`;
                dom += tmp;
            }

            $('#step_project select[name=project_service_id]').html(dom);
            form.render('select');
        }
    })
}

function get_project(id, form) {
    get({
        url: '../api/flowproject/getprojectinfo?project_uid=' + id,
        done: o => {
            render_project(o.data);
            get_service(form, o.data['project']['service_id']);
        },
        err: o => {
            layer.msg(o.msg);
        }
    })
}

function render_project(data) {
    $('#step_project input[name=project_name]').val(data['project']['project_name']);
    $('#step_project input[name=code_get_cmd]').val(data['project']['code_get_cmd']);
    $('#step_project input[name=project_path]').val(data['project']['project_path']);
    $('#step_project textarea[name=project_remark]').val(data['project']['project_remark']);

    $('#step_publish input[name=build_before_command]').val(data['publish']['build_before_command']);
    $('#step_publish input[name=build_command]').val(data['publish']['build_command']);
    $('#step_publish input[name=build_after_command]').val(data['publish']['build_after_command']);
    $('#step_publish input[name=publish_file_path]').val(data['publish']['publish_file_path']);
    $('#step_publish input[name=publish_before_command]').val(data['publish']['publish_before_command']);
    $('#step_publish input[name=publish_after_command]').val(data['publish']['publish_after_command']);
}