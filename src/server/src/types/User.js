exports.UserType = {
    User: 0,
    ValidUser: 1
}

exports.User = class User {
    type;
    socket;
    sessionId;
}

exports.ValidUser = class ValidUser {
    type;
    socket;
    sessionId;
    account;
}