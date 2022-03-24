import { LoginRequest } from '../protobuf/LoginRequest/LoginRequest';
import { connection } from 'websocket';
import { Account } from '../types/Account';
import { UserType } from '../types/User';

export function receive(socket: connection, buffer: Buffer) {
    const type: number = buffer.readUInt32BE(0);
    const length: number = buffer.readUInt32BE(1);
    const data: Buffer = buffer.slice(5, 5 + length);

    switch (type) {
        case 0:
            LoginRequestHandler(socket, data);
            break;
        default:
            break;
    }
}


function LoginRequestHandler(socket: connection, data: Buffer) {
    const loginRequest = LoginRequest.decode(data);
    const account = new Account(socket.user.sessionId, loginRequest.username);
    
    socket.user = {
        type: UserType.ValidUser,
        sessionId: socket.user.sessionId,
        socket: socket,
        account: account
    }

    console.log(loginRequest);
}