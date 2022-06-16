import { Account } from '../types/Account.js';
import { ValidUser } from '../types/User.js';
import * as sleep from '../util/sleep.js';
import * as Logger from '../util/logger.js';
import * as auth from '../util/auth.js';
import proto from '../util/proto.js';

const logger = Logger.getLogger('TokenLoginRequest');

const id = 1;
const type = 'TokenLoginRequest';

export default {
    id: id,
    type: type,
    receive: async (socket, buffer) => {
        await sleep(3000);
        if (!proto.server.verify(type, buffer)) {
            logger.warn(`Invalid packet from ${socket.remoteAddress}`);
            return;
        }
        const tokenLoginRequest = proto.server.decode(type, buffer);
        const verifyed = auth.verifyToken(tokenLoginRequest.Token);
        if (verifyed) {
            if (auth.login(verifyed.username, verifyed.password)) {
                socket.user = new ValidUser(
                    socket,
                    socket.sessionId,
                    new Account(socket.sessionId, verifyed.username)
                );
                socket.sendPacket(proto.client.encode(proto.client.LoginResponse, {
                    Success: true,
                    UserUUID: socket.user.account.userId,
                    Username: socket.user.account.username,
                    Token: tokenLoginRequest.Token,
                }));
                return;
            }
        }
        socket.sendPacket(proto.client.encode(proto.client.LoginResponse, {
            Success: false,
        }));
    }
}
