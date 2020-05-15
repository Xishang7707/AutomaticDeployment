$(function () {
    $('#btn_open_addservice').click(() => { open_page('添加服务器', 'service/addservice', 'addservice'); });
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