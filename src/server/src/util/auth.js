const jwt = require('jsonwebtoken');
const secret = 'secret';

function encode(data, exriresIn) {
    return jwt.sign(data, secret, { expiresIn: exriresIn })
}

module.exports.generateToken = (userId, username) => {
    return encode({
        userId: userId,
        username: username,
    }, '14d');
}

module.exports.login = (username, password) => {
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
