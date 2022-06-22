import { connection } from "websocket";
import { v4 } from 'uuid';
import { User } from "./User";
import * as Logger from '../util/logger';

const logger = Logger.getLogger('Client');

export class Client {
    socket: connection;
    sessionId: string;
    user: User;
    constructor(socket: connection) {
        this.socket = socket;
        this.sessionId = v4();
        this.user = { 
            type: "notValid",
            client: this 
        };
    }

    sendPacket(buffer: Buffer) {
        logger.debug(`Sending: ${buffer.length} bytes to ${this.sessionId}`);
        this.socket.sendBytes(buffer);
    }
}