const { v4 } = require('uuid');

exports.Account = class Account {
    userId;
    username;
    constructor(userId, username) {
        this.userId = userId ?? v4();
        this.username = username;
    }
}