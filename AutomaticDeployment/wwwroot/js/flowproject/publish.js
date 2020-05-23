$(function () {
    layui.use(['element'], function () {
        var $ = layui.jquery
            , element = layui.element;

        var project_uid = getQuery('project_uid');
        if (!project_uid)
            return;
        $('#btn-project-publish').click(() => { publish(project_uid); });
        let host = '../publishlog';
        let hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(host)
            .build();
        recv_publish_log(hubConnection);
        recv_publish_result(hubConnection, project_uid);

        hubConnection.start().then(() => {
            hubConnection.send("publish", project_uid);
        });

        get_project(project_uid);
    })
});

function recv_publish_log(conn) {
    conn.on("log", function (data) {
        $('#publish_log').append($(`<li>`).text(data['publish_info']));
    });
}

function recv_publish_result(conn, id) {
    conn.on("result", function (data) {
        tw.layer.msg(data['publish_info']);
        get_project(id);
    });
}

function publish(project_uid) {
    var w = get_top_window();
    post({
        url: '../api/flowproject/publish',
        data: { project_uid: project_uid },
        done: o => {
            w.layer.msg(o.msg);
        },
        err: o => {
            w.layer.msg(o.msg);
        }
    })
}

function get_project(id) {
    get({
        url: '../api/flowproject/getproject?project_uid=' + id,
        done: o => {
            render_project(o.data);
        },
        err: o => {
            w.layer.msg(o.msg);
        }
    })
}

function render_project(data) {
    $('#project_name').text(data['project']['project_name']);
    $('#server_name').text(data['server']['name']);
    $('#publish_time').text(data['publish']['publish_time']);
    $('#publish_before_command').text(data['publish']['publish_before_command']);
    $('#publish_after_command').text(data['publish']['publish_after_command']);
    $('#publish_status').text(data['publish']['publish_status']);
    $('#project_remark').text(data['project']['project_remark']);
}