import { connection } from "websocket";
import { User, UserType, ValidUser } from '../types/User';

declare module 'websocket' {
    export interface connection {
        user: User | ValidUser;
    }
}

connection.prototype.user = {
    type: UserType.User,
    socket: null,
    sessionId: ""
};
