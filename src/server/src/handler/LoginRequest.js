const protobuf = require('protobufjs');
const { Account } = require('../types/Account');
const { UserType } = require('../types/User');

module.exports = {
    id: 0,
    receive: (socket, buffer) => {
        protobuf.load('./src/server/proto/login.proto', (err, root) => {
            if (err) {
                console.log(err);
                return;
            }

            const LoginRequest = root.lookupType('LoginRequest');
            const loginRequest = LoginRequest.decode(buffer);

            const account = new Account(socket.user.sessionId, loginRequest.username);

            socket.user = {
                type: UserType.ValidUser,
                sessionId: socket.user.sessionId,
                socket: socket,
                account: account
            }
        });
    }
}
