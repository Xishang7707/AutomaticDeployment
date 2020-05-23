$(function () {
    layui.use('element', () => {
        element = layui.element;
        tw.element = element;
        element.on('nav(top-nav)', function (elem) {
            var el = $(elem);
            if (!el.attr('nav-page')) {
                return;
            }
            var layid = el.attr('lay-id');
            open_tab(el.html(), el.attr('page-link'), layid);
        });

    });
});

/**
 * 添加tab
 * @param title 标题
 * @param url 链接地址
 * @param id 唯一标识
 */
function open_tab(title, url, id) {
    var layid = id;
    var lis = $('ul.layui-tab-title li');
    for (var i = 0; i < lis.length; i++) {
        var t = lis.eq(i);
        if (t.attr('lay-id') == layid) {
            element.tabChange('main-tab', layid);
            return;
        }
    }

    element.tabAdd('main-tab', {
        title: title
        , content: `<iframe name='${layid}' lay-id='${layid}' src='${url}' style="width:100%;height:100%;border:none;"></iframe>`
        , id: layid
    });
    element.tabChange('main-tab', layid);
}

function init_ws() {
    let host = '../notice';
    let hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(host)
        .build();
    recv_publish_log(hubConnection);
    recv_publish_result(hubConnection, project_uid);

    hubConnection.start();
}

function recv_update(hub) {
    hub.on("update", function (data) {
        var iframe = $(`div.layui-tab-content iframe[lay-id=${data['pid']}]`)[0];
        iframe && iframe.contentWindow && iframe.contentWindow.global_update && iframe.contentWindow.global_update(data);
    });
}