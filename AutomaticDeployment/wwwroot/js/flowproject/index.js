$(function () {
    $('#btn_open_addproject').click(() => { open_page('添加项目', 'flowproject/addproject', 'addflowproject') });
    var w = get_top_window();
    layui.use([], function () {
        get({
            url: '../api/flowproject/getprojectlist',
            done: o => {
                render_project_table(o['data']);
                bind_publish(o['data']);
                bind_edit_project(o['data']);
            },
            err: o => {
                w.layer.msg(o.msg);
            }
        })
    });
});

/**
 * 编辑项目
 * */
function open_edit(id, name) {
    var w = get_top_window();
    if (w.open_tab) {
        w.open_tab(`修改[${name}]`, 'flowproject/editflowproject?project_uid=' + id, 'editflowproject#' + id);
    } else {
        w.open('editflowproject?project_uid=' + id, '_blank');
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
                        <td>
                            <button type="button" class="layui-btn btn-publish" id='btn-publish-${item['project']['project_uid']}'>
                                <i class="layui-icon">&#xe67c;</i>发布
                            </button>
                            <button type="button" class="layui-btn btn-edit-project" id='btn-edit-${item['project']['project_uid']}'>
                                编辑
                            </button>
                        </td>
                    </tr>`;
        dom += temp;
    }
    $('#project-body tbody').html($(dom));
}

function bind_publish(o) {
    for (var k in o) {
        var item = o[k];
        ((id, name) => {
            $(`#btn-publish-${id}`).click(() => {
                open_page(`发布[${name}]`, 'flowproject/publish?project_uid=' + id, 'flowpublish#' + id);
            });
        })(item['project']['project_uid'], item['project']['project_name']);
    }
}

function bind_edit_project(o) {
    for (var k in o) {
        var item = o[k];
        ((id, name) => {
            $(`#btn-edit-${id}`).click(() => {
                open_edit(id, name);
            });
        })(item['project']['project_uid'], item['project']['project_name']);
    }
}