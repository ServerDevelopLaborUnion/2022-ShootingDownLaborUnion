import { connection } from "websocket";
import { Account } from './Account';

export class User {
    socket: connection;
    sessionId: string;
    account: Account | null;
    constructor(socket: connection, uuid: string) {
        this.socket = socket;
        this.sessionId = uuid;
        this.account = null;
    }
}