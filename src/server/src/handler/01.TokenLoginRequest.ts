import { Account } from '../types/Account';
import { User } from '../types/User';
import { sleep } from '../util/sleep';
import * as Logger from '../util/logger';
import * as auth from '../util/auth';
import proto from '../util/proto';
import { Client } from '../types/Client';
import { storage } from '../storage';

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
        const verifyed = auth.verify(tokenLoginRequest.Token, (err, decoded) => {
            if (err) {
                logger.warn(`Invalid token from ${client.socket.remoteAddress}`);
                return;
            }
            const account = storage.database.accounts.get(decoded.id);
            if (account === undefined) {
                logger.warn(`Invalid token from ${client.socket.remoteAddress}`);
                return;
            }

            if (account) {
                if (auth.login(account.username, account.password)) {
                    client.user = {
                        type: "valid",
                        client: client,
                        account: new Account(client.sessionId, account.username)
                    }
                    client.socket.sendBytes(proto.client.encode(proto.client.LoginResponse, {
                        Success: true,
                        UserUUID: client.user.account.userId,
                        Username: client.user.account.username,
                        Token: tokenLoginRequest.Token,
                    }));
                    return;
                }
            }
        });

        client.socket.sendBytes(proto.client.encode(proto.client.LoginResponse, {
            Success: false,
        }));
    }
}
