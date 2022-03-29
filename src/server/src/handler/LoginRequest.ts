import { connection } from 'websocket';
import { LoginRequest } from '../protobuf/LoginRequest/LoginRequest';
import { Account } from '../types/Account';
import { UserType } from '../types/User';
import { Handler } from '../types/Handler';
import { handlers } from '../websocket/router';

export class LoginRequestHandler implements Handler{
    receive(socket: connection, buffer: Buffer) {
        const loginRequest = LoginRequest.decode(buffer);
        const account = new Account(socket.user.sessionId, loginRequest.username);

        socket.user = {
            type: UserType.ValidUser,
            sessionId: socket.user.sessionId,
            socket: socket,
            account: account
        }

        console.log(loginRequest);
    }
}

handlers[0] = new LoginRequestHandler();