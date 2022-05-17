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
        const account = new Account(socket.user.sessionId, loginRequest.username);

        // TODO: DB 로그인 구현
        if (auth.login(account)) {
            socket.user = new ValidUser(
                socket,
                account.userId,
                account
            );

            const { username, password } = loginRequest;
            const token = auth.encode({ username, password }, '1h');

            socket.sendPacket(proto.client.encode(proto.client.LoginResponse, {
                Success: true,
                Username: loginRequest.username,
                Token: token,
            }));
        } else {
            socket.sendPacket(proto.client.encode(proto.client.LoginResponse, {
                Success: false,
            }));
        }
    }
}
