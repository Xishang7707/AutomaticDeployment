/**
 * 验证ip
 * @param {any} ip
 */
function verifyIp(ip) {
    var patten = /^((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))$/;
    return ip && patten.test(ip);
}

/**
 * 验证端口
 * @param {any} port
 */
function verifyPort(port) {
    return port && !(Number.isNaN(port) || port <= 0 || port > 65535);
}