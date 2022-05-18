exports.UserType = {
    User: 0,
    ValidUser: 1
}

exports.User = class User {
    type;
    socket;
    constructor(socket) {
        this.type = exports.UserType.User;
        this.socket = socket;
    }
}

exports.ValidUser = class ValidUser {
    type;
    socket;
    sessionId;
    account;
    constructor(socket, account) {
        this.type = exports.UserType.ValidUser;
        this.socket = socket;
        this.account = account;
    }
}