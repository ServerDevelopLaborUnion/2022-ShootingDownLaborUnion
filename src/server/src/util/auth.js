const jwt = require('jsonwebtoken');
const secret = 'secret';

function encode(data, exriresIn) {
    return jwt.sign(data, secret, { expiresIn: exriresIn })
}

module.exports.generateToken = (account) => {
    return encode({
        userId: account.userId,
        username: account.username,
    }, '14d');
}

module.exports.login = (account) => {
    // TODO: DB 로그인 구현
    return true;
}

module.exports.verify = (token) => {
    return jwt.verify(token, secret, (err, decoded) => {
        if (err) {
            return undefined;
        } else {
            return decoded;
        }
    });
};
