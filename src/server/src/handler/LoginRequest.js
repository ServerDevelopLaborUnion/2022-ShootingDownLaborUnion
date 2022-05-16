const { Account } = require('../types/Account');
const { UserType } = require('../types/User');
const auth = require('../util/auth');
const proto = require('../util/proto');

const id = 0;
const type = 'LoginRequest';

module.exports = {
    id: id,
    type: type,
    receive: (socket, buffer) => {
        if (!proto.server.verify(type, buffer)) return;
        const loginRequest = proto.server.decode(type, buffer);

        const account = new Account(socket.user.sessionId, loginRequest.username);

        // TODO: DB 로그인 구현

        const token = auth.encode(loginRequest.username, loginRequest.password);

        socket.sendPacket(proto.client.encode(proto.client.LoginResponse, {
            Success: true,
            Username: loginRequest.username,
            Token: token,
        }));

        socket.user = {
            type: UserType.ValidUser,
            sessionId: socket.user.sessionId,
            socket: socket,
            account: account
        }
    }
}
