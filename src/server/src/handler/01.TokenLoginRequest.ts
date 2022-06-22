import { Account } from '../types/Account';
import { User } from '../types/User';
import { sleep } from '../util/sleep';
import * as Logger from '../util/logger';
import * as auth from '../util/auth';
import proto from '../util/proto';
import { Client } from '../types/Client';

const logger = Logger.getLogger('TokenLoginRequest');

const id = 1;
const type = 'TokenLoginRequest';

export default {
    id: id,
    type: type,
    receive: async (client: Client, buffer: Buffer) => {
        await sleep(3000);
        if (!proto.server.verify(type, buffer)) {
            logger.warn(`Invalid packet from ${client.socket.remoteAddress}`);
            return;
        }
        const tokenLoginRequest: any = proto.server.decode(type, buffer);
        const verifyed = auth.verify(tokenLoginRequest.Token);
        if (verifyed) {
            if (auth.login(verifyed.username, verifyed.password)) {
                client.user = {
                    type: "valid",
                    client: client,
                    account: new Account(client.sessionId, verifyed.username)
                }
                client.socket.sendPacket(proto.client.encode(proto.client.LoginResponse, {
                    Success: true,
                    UserUUID: client.socket.user.account.userId,
                    Username: client.socket.user.account.username,
                    Token: tokenLoginRequest.Token,
                }));
                return;
            }
        }
        client.socket.sendPacket(proto.client.encode(proto.client.LoginResponse, {
            Success: false,
        }));
    }
}
