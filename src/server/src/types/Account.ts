import { v4 as getUUID } from 'uuid';
export class Account {
    userId: string;
    username: string;
    constructor(userId: string | null, username: string) {
        this.userId = userId ?? getUUID();
        this.username = username;
    }
}