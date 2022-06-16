import { v4 } from 'uuid';

export class Account {
    userId;
    username;
    constructor(userId, username) {
        this.userId = userId ?? v4();
        this.username = username;
    }
}