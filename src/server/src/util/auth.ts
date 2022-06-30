import * as jwt from 'jsonwebtoken';
const secret = 'secret';

function encode(data: string | object | Buffer, exriresIn: string | number): string {
    return jwt.sign(data, secret, { expiresIn: exriresIn })
}

export function generateToken(userId: string, username: string): string {
    return encode({
        userId: userId,
        username: username,
    }, '14d');
}

export function login(username: string, password: string): boolean {
    // TODO: DB 로그인 구현
    console.log(username, password);
    return true;
}

export function verify(token: string, callback: (err: any, decoded: any) => void) {
    return jwt.verify(token, secret, callback);
}
