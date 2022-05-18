const { server, connection } = require('websocket');
const { UserType } = require('../types/User');
const { v4 } = require('uuid');
const http = require('node:http');
const Logger = require('../util/logger').getLogger('Websocket');
const proto = require('../util/proto');
const router = require('./router');

Object.defineProperty(connection.prototype, 'sendPacket', {
    value: function (buffer) {
        Logger.debug(`Sending: ${buffer.length} bytes to ${this.user.sessionId}`);
        this.sendBytes(buffer);
    },
    enumerable: false,
    configurable: true,
    writable: true
});

exports.WebsocketServer = new class WebsocketServer {
    port;
    /** @type {http.Server} */
    server;
    /** @type {server} */
    wsServer;

    constructor() {
        this.port = 0;
        this.wsServer = null;
        this.server = new http.createServer((request, response) => {
            response.end('');
        });

        connection.prototype.server = this.server;
    }

    listen(port) {
        this.port = port;
        this.server.listen(this.port, () => {
            Logger.info(`Server is listening on ${this.port}`);
        });

        this.wsServer = new server({ httpServer: this.server });

        this.wsServer.on("request", (request) => {
            const socket = request.accept(null, request.origin);
            console.log(this.server);
            // 서버에 접속한 사용자를 추가한다.
            socket.sessionId = v4();
            socket.user = {
                type: UserType.User,
                socket: socket,
            }

            // 클라이언트에게 SessionId를 전송한다.
            socket.sendPacket(proto.client.encode(proto.client.Connection, {
                SessionId: socket.sessionId,
            }));
            Logger.debug(`${socket.sessionId} connected`);

            socket.sendPacket(proto.client.encode(proto.client.CreateEntity, {
                Entity: {
                    UUID: v4(),
                    OwnerUUID: socket.sessionId,
                    Name: 'Player',
                    Position: { X: 0, Y: 0, Z: 0 },
                    Rotation: { X: 0, Y: 0, Z: 0, W: 0 },
                    Data: '{"type":"0"}',
                }
            }));

            // 클라이언트에게 메시지를 받았을 때 처리한다.
            socket.on("message", (message) => {
                if (message.type === 'binary') {
                    router.receive(socket, message.binaryData);
                }
            });

            // 클라이언트에게 연결이 끊겼을 때 처리한다.
            socket.on("close", (reasonCode, description) => {
                Logger.debug(`${socket.user?.sessionId} disconnected: ${reasonCode} ${description}`);
            });

            // 클라이언트에게 에러가 발생했을 때 처리한다.
            socket.on("error", (error) => {
                Logger.error(`${error}`);
            });
        });
    }
}
