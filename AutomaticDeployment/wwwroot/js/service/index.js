$(function () {
    $('#btn_open_addservice').click(() => { open_page('添加服务器', 'service/addservice', 'addservice'); });
    get_service();
});

/**
 * 打开添加服务器
 * */
function open_addservice() {
    var w = get_top_window();
    if (w.open_tab) {
        w.open_tab('添加服务器', 'service/addservice', 'addservice');
    } else {
        w.open('addservice', '_blank');
    }
}

function get_service() {
    get({
        url: '../api/service/getservicelist',
        done: o => {
            render_table(o['data']);
        },
        err: o => {
            layer.msg(o.msg);
        }
    })
}

function render_table(data) {
    var dom = ``;
    for (var i = 0; i < data.length; i++) {
        var it = data[i];
        var act_dom = `<button type="button" class="layui-btn btn-edit-service" onclick="open_edit('${it.server_id}')">编辑</button>`;
        if (it['act_delete']) {
            act_dom += `<button type="button" class="layui-btn layui-btn-danger btn-delete-service" onclick="delete_service('${it.server_id}')"> 删除</button>`;
        }

        var item = `<tr>
                        <td>
                            ${it.server_name}
                        </td>
                        <td>
                            <p>平台：${it.server_platform}</p>
                            <p>ip：${it.server_ip}</p>
                            <p>端口：${it.server_port}</p>
                            <p>账号：${it.server_account}</p>
                        </td>
                        <td>${it.workspace}</td>
                        <td style="text-align:center;">
                            <div class="layui-btn-group">${act_dom}</div>
                        </td>
                    </tr>`;
        dom += item;
    }

    $('#service-body tbody').html($(dom));
}

function open_edit(id) {
    open_page('修改服务器', 'service/editservice?id=' + id, 'editservice#' + id);
}

function delete_service(id) {
    layer.confirm('确定要删除此服务器吗？', { icon: 3, title: '询问' }, function (index) {
        var w = get_top_window();
        post({
            url: '../api/service/deleteservice',
            data: { service_id: id },
            done: o => {
                w.layer.msg(o.msg);
                get_service();
            },
            err: o => {
                w.layer.msg(o.msg);
            }
        });
        layer.close(index);
    });
}