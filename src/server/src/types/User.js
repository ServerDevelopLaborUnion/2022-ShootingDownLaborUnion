exports.UserType = {
    User: 0,
    ValidUser: 1
}

exports.User = class User {
    type;
    socket;
    sessionId;
    constructor(socket, sessionId, account) {
        this.type = exports.UserType.User;
        this.socket = socket;
        this.sessionId = sessionId;
        this.account = account;
    }
}

exports.ValidUser = class ValidUser {
    type;
    socket;
    sessionId;
    account;
    constructor(socket, sessionId, account) {
        this.type = exports.UserType.ValidUser;
        this.socket = socket;
        this.sessionId = sessionId;
        this.account = account;
    }
}