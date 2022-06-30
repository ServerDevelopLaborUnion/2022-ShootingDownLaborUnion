import { v4 } from 'uuid';

export class Account {
    userId;
    username;
    constructor(userId: string, username: string) {
        this.userId = userId ?? v4();
        this.username = username;
    }
}