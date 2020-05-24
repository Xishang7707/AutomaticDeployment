var form;
$(function () {
    $('#btn_open_addproject').click(() => { open_page('添加项目', 'flowproject/addproject', 'addflowproject') });
    $('#btn-search').click(() => { get_projects(); });
    layui.use(['element', 'form'], function () {
        form = layui.form;
        form.on('select(project_classify)', function (data) {
            get_projects();
        });
        get_classify();
        get_projects();
    });
});

function get_projects() {
    get({
        url: `../api/flowproject/getprojectlist?project_classify=${get_select_input('form select[name=project_classify]')}`,
        done: o => {
            render_project_table(o['data']);
            bind_act(o['data']);
        },
        err: o => {
            tw.layer.msg(o.msg);
        }
    })
}

/**
 * 编辑项目
 * */
function open_edit(id, name) {
    if (tw.open_tab) {
        tw.open_tab(`修改[${name}]`, 'flowproject/editflowproject?project_uid=' + id, 'editflowproject#' + id);
    } else {
        tw.open('editflowproject?project_uid=' + id, '_blank');
    }
}

function render_project_table(o) {
    var dom = ``;
    for (var k in o) {
        var item = o[k];
        var proj_item = ``;
        if (item['project']['project_classify']) {
            proj_item = `<p>名称：${item['project']['project_name']}</p>
                         <p>归类：${item['project']['project_classify']}</p>
                         <p>服务器：${item['server']['name']}</p>`;
        } else {
            proj_item = `<p>项目名称：${item['project']['project_name']}</p>
                         <p>服务器：${item['server']['name']}</p>`;
        }
        var temp = `
                    <tr>
                        <td>
                            ${proj_item}
                        </td>
                        <td>
                        </td>
                        <td>
                            <p>发布路径：${item['project']['project_path']}</p>
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
            $(`#btn-publish-${id}`).click(() => { open_page(`发布[${name}]`, 'flowproject/publish?project_uid=' + id, 'flowpublish#' + id); });
            $(`#btn-edit-${id}`).click(() => { open_page(`修改[${name}]`, 'flowproject/editproject?project_uid=' + id, 'editflowproject#' + id); });
            $(`#btn-delete-${id}`).click(() => { delete_project(id); });
        })(item['project']['project_uid'], item['project']['project_name']);
    }
}

function notice_add(data) {
    get_classify(get_selected('form select[name=project_classify]'));
    get_projects();
}

function notice_update(data) {
    get_classify(get_selected('form select[name=project_classify]'));
    get_projects();
}

function notice_delete(data) {
    get_classify(get_selected('form select[name=project_classify]'));
    get_projects();
}

function delete_project(id) {
    layer.confirm('确定要删除此项目吗？', { icon: 3, title: '询问' }, function (index) {
        post({
            url: '../api/flowproject/deleteproject',
            data: { project_uid: id },
            done: o => {
                tw.layer.msg(o.msg);
            },
            err: o => {
                tw.layer.msg(o.msg);
            }
        });
        layer.close(index);
    });
}

function get_classify(sed) {
    render_select({
        sor: 'form select[name=project_classify]',
        el: form,
        def: '项目归属类别',
        sed: sed,
        url: '../api/flowproject/getclassify',
        done: o => {
            $('form select[name=project_classify]').next().find('div input.layui-input').unbind('blur');
        }
    });
}