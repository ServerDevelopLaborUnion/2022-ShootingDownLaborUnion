import * as jwt from 'jsonwebtoken';
const secret = 'secret';

function encode(data, exriresIn) {
    return jwt.sign(data, secret, { expiresIn: exriresIn })
}

export function generateToken (userId, username) {
    return encode({
        userId: userId,
        username: username,
    }, '14d');
}

export function login (username, password) {
    // TODO: DB 로그인 구현
    console.log(username, password);
    return true;
}

export function verify (token) {
    return jwt.verify(token, secret, (err, decoded) => {
        if (err) {
            return undefined;
        } else {
            return decoded;
        }
    });
}
