$(function () {
    $('#btn_open_addproject').click(open_addproject);
    var w = get_top_window();
    layui.use([], function () {
        get_projects();
    });
});

function get_projects() {
    get({
        url: '../api/quickproject/getprojectlist',
        done: o => {
            render_project_table(o['data']);
            bind_act(o['data']);
        },
        err: o => {
            w.layer.msg(o.msg);
        }
    })
}

/**
 * 打开添加项目
 * */
function open_addproject() {
    var w = get_top_window();
    if (w.open_tab) {
        w.open_tab('添加项目', 'quickproject/addquickproject', 'addquickproject');
    } else {
        w.open('addquickproject', '_blank');
    }
}

/**
 * 发布项目
 * */
function open_publish(id, name) {
    var w = get_top_window();
    if (w.open_tab) {
        w.open_tab(`发布[${name}]`, 'quickproject/publish?project_uid=' + id, 'quickpublish#' + id);
    } else {
        w.open('publish?project_uid=' + id, '_blank');
    }
}

/**
 * 编辑项目
 * */
function open_edit(id, name) {
    var w = get_top_window();
    if (w.open_tab) {
        w.open_tab(`修改[${name}]`, 'quickproject/editquickproject?project_uid=' + id, 'quickpublish#' + id);
    } else {
        w.open('editquickproject?project_uid=' + id, '_blank');
    }
}

function render_project_table(o) {
    var dom = ``;
    for (var k in o) {
        var item = o[k];
        var temp = `
                    <tr>
                        <td>
                            <p>${item['project']['project_name']}</p>
                        </td>
                        <td>
                            <p>IP：${item['server']['server_ip']}</p>
                            <p>账号：${item['server']['server_account']}</p>
                            <p>登录模式：${item['server']['server_connect_mode']}</p>
                        </td>
                        <td>
                            <p>发布路径：${item['publish']['publish_path']}</p>
                            <p>发布前命令：${item['publish']['publish_before_command']}</p>
                            <p>发布后命令：${item['publish']['publish_after_command']}</p>
                            <p>发布时间：${item['publish']['publish_time']}</p>
                            <p>发布状态：${item['publish']['publish_status']}</p>
                        </td>
                        <td>
                            <p>${item['project']['project_remark']}</p>
                        </td>
                        <td style="text-align:center;">
                            <div class="layui-btn-group">
                                <button type="button" class="layui-btn btn-publish" id='btn-publish-${item['project']['project_uid']}'>
                                    <i class="layui-icon">&#xe67c;</i>发布
                                </button>
                                <button type="button" class="layui-btn btn-edit-project" id='btn-edit-${item['project']['project_uid']}'>
                                    编辑
                                </button>
                                <button type="button" class="layui-btn layui-btn-danger btn-delete-project" id='btn-delete-${item['project']['project_uid']}'>
                                    删除
                                </button>
                            </div>
                        </td>
                    </tr>`;
        dom += temp;
    }
    $('#project-body tbody').html($(dom));
}

function bind_act(o) {
    for (var k in o) {
        var item = o[k];
        ((id, name) => {
            $(`#btn-publish-${id}`).click(() => {
                open_publish(id, name);
            });
            $(`#btn-edit-${id}`).click(() => {
                open_edit(id, name);
            });
            $(`#btn-delete-${id}`).click(() => {
                delete_project(id);
            });
        })(item['project']['project_uid'], item['project']['project_name']);
    }
}

function delete_project(id) {
    layer.confirm('确定要删除此项目吗？', { icon: 3, title: '询问' }, function (index) {
        var w = get_top_window();
        post({
            url: '../api/quickproject/deleteproject',
            data: { project_uid: id },
            done: o => {
                w.layer.msg(o.msg);
                get_projects();
            },
            err: o => {
                w.layer.msg(o.msg);
            }
        });
        layer.close(index);
    });
}