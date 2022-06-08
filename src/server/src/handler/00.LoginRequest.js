import { Account } from '../types/Account.js';
import { ValidUser } from '../types/User.js';
import * as sleep from '../util/sleep.js';
import * as Logger from '../util/logger.js';
import * as auth from '../util/auth.js';
import proto from '../util/proto.js';

const logger = Logger.getLogger('LoginRequest');

const id = 0;
const type = 'LoginRequest';

export default {
    id: id,
    type: type,
    receive: async (socket, buffer) => {
        await sleep(3000);
        if (!proto.server.verify(type, buffer)) {
            logger.warn(`Invalid packet from ${socket.remoteAddress}`);
            return;
        }
        const loginRequest = proto.server.decode(type, buffer);
        const account = new Account(socket.sessionId, loginRequest.Username);

        // TODO: DB 로그인 구현
        if (auth.login(loginRequest.Username, loginRequest.Password)) {
            socket.user = new ValidUser(
                socket,
                socket.sessionId,
                account
            );

            const token = auth.generateToken(socket.user.account.userId, socket.user.account.username);

            socket.sendPacket(proto.client.encode(proto.client.LoginResponse, {
                Success: true,
                UserUUID: socket.user.account.userId,
                Username: socket.user.account.username,
                Token: token,
            }));
        } else {
            socket.sendPacket(proto.client.encode(proto.client.LoginResponse, {
                Success: false,
            }));
        }
    }
}
