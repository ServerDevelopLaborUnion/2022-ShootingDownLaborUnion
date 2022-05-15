const Logger = require('../util/logger').getLogger('Router');
const fs = require('fs');

const handlers = {};

const files = fs.readdirSync('./handler');

Logger.debug('Loading handlers: ' + files.join(', '));

for (const file of files) {
    if (file.endsWith('.js')) {
        const handler = require(`../handler/${file}`);
        handlers[handler.id] = handler;
    }
}

/**
 * 
 * @param {*} socket 
 * @param {Buffer} buffer 
 */
exports.receive = (socket, buffer) => {
    const type = buffer.readUInt16BE(0);
    const data = buffer.slice(2, 2 + buffer.length);

    if (handlers[type] !== undefined) {
        handlers[type].receive(socket, data);
        Logger.debug(`Received: ${handlers[type].type} (${data.length}bytes) from ${socket.remoteAddress}`);
    } else {
        Logger.warn(`Unknown packet type: ${type} (${data.length}bytes) from ${socket.remoteAddress}`);
    }
}
