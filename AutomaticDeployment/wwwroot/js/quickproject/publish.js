$(function () {
    layui.use(['upload', 'element'], function () {
        var $ = layui.jquery
            , upload = layui.upload
            , element = layui.element;

        var project_uid = getQuery('project_uid');
        if (!project_uid)
            return;

        let host = '../publishlog';
        let hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(host)
            .build();
        recv_publish_log(hubConnection);
        recv_publish_result(hubConnection, project_uid);

        hubConnection.start().then(() => {
            hubConnection.send("publish", project_uid);
        });
        hubConnection.onclose(() => {
            hub_reconnection(hubConnection);
        });
        get_project(project_uid);
        upload.render({
            elem: '#project_file' //绑定元素
            , url: '../api/upload/upload' //上传接口
            , accept: 'file'
            , exts: 'zip'
            , acceptMime: 'application/zip'
            , data: {
                project_uid: project_uid
            }
            , choose: function (obj) {
                $('#step_project input[name=file_id]').val('');
                element.progress('progress-file-upload', '0%');
            }
            , before: o => {
                element.progress('progress-file-upload', '0%');
                $('#publish_log').children().remove();
            }
            , progress: function (n, elem) {
                element.progress('progress-file-upload', n + '%');
                if (n == 100) {
                    btn_publish_upload();
                }
            }
            , done: function (res) {
                $('#step_project input[name=file_id]').val(res.id);

                publish(project_uid, [
                    { file_id: res.id }
                ]);
            }
            , error: function () {
                layer.msg('上传失败');
                btn_publish_failed();
            }
        });
    })
});

function recv_publish_log(conn) {
    conn.on("log", function (data) {
        $('#publish_log').append($(`<li>`).text(data['publish_info']));
    });
}

function recv_publish_result(conn, id) {
    conn.on("result", function (data) {
        layer.msg(data['publish_info']);
        get_project(id);
    });
}

function publish(project_uid, files) {
    post({
        url: '../api/quickproject/publish',
        data: {
            project_uid: project_uid,
            files: files
        },
        done: o => {
            layer.msg(o.msg);
        },
        err: o => {
            layer.msg(o.msg);
        }
    })
}

function btn_publish_upload() {
    $('#project_file').html(`<i class="layui-icon">&#xe67c;</i>发布(压缩文件)`);
}

function btn_publish_progress() {
    $('#project_file').html(`<i class="layui-icon layui-anim-loop">&#xe63e;</i>上传中`);
}

function btn_publish_failed() {
    $('#project_file').html(`<i class="layui-icon">&#xe67c;</i>上传失败`);
}

function get_project(id) {
    get({
        url: '../api/quickproject/getproject?project_uid=' + id,
        done: o => {
            render_project(o.data);
        },
        err: o => {
            layer.msg(o.msg);
        }
    })
}

function render_project(data) {
    $('#project_name').text(data['project']['project_name']);
    $('#server_ip').text(data['server']['server_ip']);
    $('#server_account').text(data['server']['server_account']);
    $('#publish_time').text(data['publish']['publish_time']);
    $('#publish_path').text(data['publish']['publish_path']);
    $('#publish_before_command').text(data['publish']['publish_before_command']);
    $('#publish_after_command').text(data['publish']['publish_after_command']);
    $('#publish_status').text(data['publish']['publish_status']);
    $('#project_remark').text(data['project']['project_remark']);
}