function post({ url, async = true, data, done, err }) {
    $.ajax({
        url: url,
        type: 'post',
        headers: {
            'content-type': 'application/json'
        },
        data: JSON.stringify(data),
        processData: false,
        async: async,
        success: o => {
            done && done(o);
        },
        error: o => {
            err && err(o);
        }
    })
}

function get({ url, async = true, data, done, err }) {
    $.ajax({
        url: url,
        type: 'get',
        data: JSON.stringify(data),
        async: async,
        success: o => {
            done && done(o);
        },
        error: o => {
            err && err(o);
        }
    })
}