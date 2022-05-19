const { Account } = require('../types/Account');
const { ValidUser } = require('../types/User');
const sleep = require('../util/sleep');
const Logger = require('../util/logger').getLogger('LoginRequest');
const auth = require('../util/auth');
const proto = require('../util/proto');

const id = 0;
const type = 'LoginRequest';

module.exports = {
    id: id,
    type: type,
    receive: async (socket, buffer) => {
        await sleep(3000);
        if (!proto.server.verify(type, buffer)) {
            Logger.warn(`Invalid packet from ${socket.remoteAddress}`);
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
