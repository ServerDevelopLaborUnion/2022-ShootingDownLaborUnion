import * as Logger from '../util/logger';
import * as fs from 'fs';

const logger = Logger.getLogger('Router');

const handlers = {};

const files = fs.readdirSync('./src/handler');

logger.debug('Loading handlers: ' + files.join(', '));

for (const file of files) {
    if (file.endsWith('')) {
        import(`../handler/${file}`).then(handler => {
            handlers[handler.id] = handler;
        });
    }
}

/**
 * 
 * @param {*} socket 
 * @param {Buffer} buffer 
 */
export function receive(socket, buffer) {
    const type = buffer.readUInt16BE(0);
    const data = buffer.slice(2, 2 + buffer.length);

    if (handlers[type] !== undefined) {
        logger.debug(`Received: ${handlers[type].type} (${data.length} bytes) from ${socket.remoteAddress}`);
        handlers[type].receive(socket, data);
    } else {
        logger.warn(`Unknown packet type: ${type} (${data.length} bytes) from ${socket.remoteAddress}`);
    }
}
