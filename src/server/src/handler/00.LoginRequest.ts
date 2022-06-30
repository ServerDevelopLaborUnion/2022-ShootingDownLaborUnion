import { Account } from '../types/Account';
import { UserType, User } from '../types/User';
import { sleep } from '../util/sleep';
import * as Logger from '../util/logger';
import * as auth from '../util/auth';
import proto from '../util/proto';
import { IHandler } from '../types/IHandler';
import { connection } from 'websocket';
import { Client } from '../types/Client';

const logger = Logger.getLogger('LoginRequest');

class LoginRequest implements IHandler {
    id = 0;
    type = 'LoginRequest';
    async receive(client: Client, buffer: Buffer) {
        await sleep(3000);
        if (!proto.server.verify(this.type, buffer)) {
            logger.warn(`Invalid packet from ${client.socket.remoteAddress}`);
            return;
        }
        const loginRequest: any = proto.server.decode(this.type, buffer);
        const account = new Account(client.sessionId, loginRequest.Username);

        if (auth.login(loginRequest.Username, loginRequest.Password)) {
            if (client.user.type == "notValid") {
                client.user = {
                    type: "valid",
                    client: client,
                    account: new Account(client.sessionId, "")
                }

                const token = auth.generateToken(client.user.account.userId, client.user.account.username);

                client.sendPacket(proto.client.encode(proto.client.LoginResponse, {
                    Success: true,
                    UserUUID: client.user.account.userId,
                    Username: client.user.account.username,
                    Token: token,
                }));
            } else {
                
            }
        } else {
            client.sendPacket(proto.client.encode(proto.client.LoginResponse, {
                Success: false,
            }));
        }
    }
}
