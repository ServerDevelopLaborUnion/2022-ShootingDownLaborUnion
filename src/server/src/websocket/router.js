const Logger = require('../util/logger').getLogger('Router');
const fs = require('fs');

const handlers = {};

const files = fs.readdirSync('src/handler');

Logger.debug('Loading handlers: ' + files.join(', '));

for (const file of files) {
    if (file.endsWith('.js')) {
        const handler = require(`../handler/${file}`);
        console.log(handler);
        handlers[handler.id] = handler;
    }
}

exports.receive = (socket, buffer) => {
    const type = buffer.readUInt32BE(0);
    const length = buffer.readUInt32BE(1);
    const data = buffer.slice(5, 5 + length);
    Logger.debug(`Received: ${data.toString()} from ${socket.remoteAddress}`);

    if (handlers[type]) {
        handlers[type].receive(socket, data);
    }
}
