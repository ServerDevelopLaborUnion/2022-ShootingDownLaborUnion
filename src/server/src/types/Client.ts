import { connection } from "websocket";
import { v4 } from 'uuid';
import { User } from "./User";
import * as Logger from '../util/logger';
import { Room } from "./Room";

const logger = Logger.getLogger('Client');

export class Client {
    socket: connection;
    sessionId: string;
    user: User;
    room: Room | null;
    constructor(socket: connection) {
        this.socket = socket;
        this.sessionId = v4();
        this.user = {
            type: "valid",
            client: this,
            account: {
                userId: this.sessionId,
                username: "Not Defined",
            }
        };
        this.room = null;
    }

    sendPacket(buffer: Buffer) {
        // logger.debug(`Sending: ${buffer.length} bytes to ${this.sessionId}`);
        this.socket.sendBytes(buffer);
    }

    toProto() {
        if (this.user.type === "user") {
            return {
                UUID: this.sessionId,
                Role: this.user.role,
                IsReady: this.user.isReady,
                IsMaster: this.user.isMaster,
            };
        }
        else if (this.user.type === "valid") {
            return {
                UUID: this.sessionId,
                Name: this.user.account.username,
            }
        }
        else {
            return {
                UUID: this.sessionId,
                Name: "",
            }
        }
    }
}