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
    }

    listen(port) {
        this.port = port;
        this.server.listen(this.port, () => {
            Logger.info(`Server is listening on ${this.port}`);
        });

        this.wsServer = new server({ httpServer: this.server });

        this.wsServer.on("request", (request) => {
            const socket = request.accept(null, request.origin);
            socket.user = {
                type: UserType.User,
                socket: socket,
                sessionId: v4()
            }

            socket.sendPacket(proto.client.encode(proto.client.Connection, {
                SessionId: socket.user.sessionId,
            }));

            Logger.debug(`${socket.user.sessionId} connected`);
            socket.on("message", (message) => {
                if (message.type === 'binary') {
                    router.receive(socket, message.binaryData);
                }
            });
            socket.on("close", (reasonCode, description) => {
                Logger.debug(`${socket.user?.sessionId} disconnected: ${reasonCode} ${description}`);
            });
            socket.on("error", (error) => {
                Logger.error(`${error}`);
            });
        });
    }
}
