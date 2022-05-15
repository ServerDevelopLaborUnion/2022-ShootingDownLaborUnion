const jwt = require('jsonwebtoken');

const secret = 'secret';

module.exports.encode = (username, password) => {
    // TODO: DB 로그인 구현
    return jwt.sign({ username: username, password: password }, secret, { expiresIn: '90d' })
};

module.exports.verify = (token) => {
    return jwt.verify(token, secret);
};

module.exports.decode = (token) => {
    return jwt.decode(token);
};