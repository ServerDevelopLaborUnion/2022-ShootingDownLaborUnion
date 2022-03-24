import { connection } from "websocket";
import { User } from "../types/User";

declare module 'websocket' {
    export interface connection {
        user: User | null;
    }
}

connection.prototype.user = null;
