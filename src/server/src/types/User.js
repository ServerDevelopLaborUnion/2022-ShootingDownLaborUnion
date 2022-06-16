export const UserType = {
    User: 0,
    ValidUser: 1
}

export class User {
    type;
    socket;
    constructor(socket) {
        this.type = exports.UserType.User;
        this.socket = socket;
    }
}

export class ValidUser {
    type;
    socket;
    account;
    constructor(socket, account) {
        this.type = exports.UserType.ValidUser;
        this.socket = socket;
        this.account = account;
    }
}