$(function () {
    $('#btn_open_addproject').click(open_addproject);
    var w = get_top_window();
    layui.use(['carousel', 'upload'], function () {
        var carousel = layui.carousel;
        var upload = layui.upload;

        get({
            url: '../api/quickproject/getprojectlist',
            done: o => {
                render_project_table(o['data']);
                bind_publish(upload, o['data']);
            },
            err: o => {
                w.layer.msg(o.msg);
            }
        })
    });
});

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
                        </td>
                        <td>
                            <p>${item['project']['project_remark']}</p>
                        </td>
                        <td>
                            <button type="button" class="layui-btn btn-publish" id='${item['project']['proj_guid']}'>
                                <i class="layui-icon">&#xe67c;</i>发布
                            </button>
                        </td>
                    </tr>`;
        dom += temp;
    }

    $('#project-body tbody').html($(dom));
}

function bind_publish(upload, o) {
    for (var k in o) {
        var item = o[k];
        bind_publish(item['project']['proj_guid']);
        upload.render({
            elem: '#' + item['project']['proj_guid']
            , url: 'api/quickproject/publish'
            , accept: 'file'
            , choose: function (obj) {

            }
            , done: function (res) {
                //上传完毕回调
                layer.msg(res.msg);
            }
            , error: function () {
                layer.msg('发布失败');
            }
        });
    }
}