import { connection } from "websocket";

class User {
    uuid: string;
    username: string;
    constructor() {
        this.uuid = "";
        this.username = "";
    }
}

declare module 'websocket' {
    export interface connection {
        user: User;
    }
}

connection.prototype.user = new User();