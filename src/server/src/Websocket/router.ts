import { connection } from 'websocket';
import { Account } from '../types/Account';
import { UserType } from '../types/User';
import { Handler } from '../types/Handler';
import GetLogger from '../util/logger';
import * as fs from 'fs';

const Logger = GetLogger('Router');

export const handlers: { [key: number]: Handler } = {};

const files = fs.readdirSync('src/server/src/handler');

for (const file of files) {
    if (file.endsWith('.ts')) {
        const handler = require(`../handler/${file}`).default;
        const handlerId = parseInt(file.split('.')[0]);
        handlers[handlerId] = handler;
    }
}

export function receive(socket: connection, buffer: Buffer) {
    const type: number = buffer.readUInt32BE(0);
    const length: number = buffer.readUInt32BE(1);
    const data: Buffer = buffer.slice(5, 5 + length);
    Logger.Debug(`Received: ${data.toString()} from ${socket.remoteAddress}`);

    if (handlers[type]) {
        handlers[type].receive(socket, data);
    }
}
