$(function () {
    layui.use(['upload','element'], function () {
        var $ = layui.jquery
            , upload = layui.upload
            , element = layui.element;

        var project_uid = getQuery('project_uid');

        upload.render({
            elem: '#project_file' //绑定元素
            , url: '../api/upload/upload' //上传接口
            , accept: 'file'
            , data: {
                project_uid: project_uid
            }
            , choose: function (obj) {
                $('#step_project input[name=file_id]').val('');
                element.progress('progress-file-upload', '0%');
            }
            , before: o => {
                element.progress('progress-file-upload', '0%');
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