import * as Logger from '../util/logger';
import * as fs from 'fs';
import { IHandler } from '../types/IHandler';
import { Client } from '../types/Client';

const logger = Logger.getLogger('Router');

const handlers: Map<number, IHandler> = new Map();

const files = fs.readdirSync('./dist/handler');

logger.debug('Loading handlers: ' + files.join(', '));

for (const file of files) {
    if (file.endsWith('')) {
        import(`../handler/${file}`).then(handler => {
            if (handler.default) {
                handlers.set(handler.default.id, handler.default);
            }
        });
    }
}

export function receive(client: Client, buffer: Buffer) {
    const type = buffer.readUInt16BE(0);
    const data = buffer.slice(2, 2 + buffer.length);

    const handler = handlers.get(type);
    if (handler !== undefined) {
        logger.debug(`Received: ${handler.type} (${data.length} bytes) from ${client.socket.remoteAddress}`);
        handler.receive(client, data);
    } else {
        logger.warn(`Unknown packet type: ${type} (${data.length} bytes) from ${client.socket.remoteAddress}`);
    }
}
