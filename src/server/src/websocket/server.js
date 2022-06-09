import websocket from 'websocket';
const { server, connection } = websocket;

import { v4 } from 'uuid';
import http from 'node:http';

import { UserType } from'../types/User.js';
import * as Logger from'../util/logger.js';
import * as router from'./router.js';
import proto from'../util/proto.js';

const logger = Logger.getLogger('Websocket');

Object.defineProperty(connection.prototype, 'sendPacket', {
    value: function (buffer) {
        logger.debug(`Sending: ${buffer.length} bytes to ${this.sessionId}`);
        this.sendBytes(buffer);
    },
    enumerable: false,
    configurable: true,
    writable: true
});

export default class WebsocketServer {
    /** @type {number} */
    port;

    /** @type {http.Server} */
    server;

    /** @type {server} */
    wsServer;

    /** @type {Map<string, connection>} */
    clients = new Map();

    /** @type {Map<string, Room>} */
    rooms = new Map();

    /** @type {Map<string, ValidUser>} */
    users = new Map();

    /** @type {WebsocketServer} */
    websocket = null;

    constructor() {
        this.port = 0;
        this.wsServer = null;
        this.server = new http.createServer((request, response) => {
            response.end('');
        });

        connection.prototype.server = this.server;
        this.server.broadcastPacket = function (buffer, socket = null) {
            logger.debug(`Broadcasting: ${buffer.length} bytes`);
            if (socket) {
                this.clients.forEach(client => {
                    if (client != socket) {
                        client.sendPacket(buffer);
                    }
                });
            } else {
                this.clients.forEach(client => {
                    client.sendPacket(buffer);
                });
            }
        }
    }

    listen(port) {
        this.port = port;
        this.server.listen(this.port, () => {
            logger.info(`Server is listening on ${this.port}`);
        });

        this.wsServer = new server({ httpServer: this.server });

        this.wsServer.on("request", async (request) => {
            const socket = request.accept(null, request.origin);
            // 서버에 접속한 사용자를 추가한다.

            socket.sessionId = v4();
            socket.user = {
                type: UserType.User,
                socket: socket,
            }
            
            this.clients.set(socket.sessionId, socket);

            logger.info(`${socket.sessionId} connected`);

            // 클라이언트에게 메시지를 받았을 때 처리한다.
            socket.on("message", (message) => {
                if (message.type === 'binary') {
                    router.receive(socket, message.binaryData);
                }
            });

            // 클라이언트에게 연결이 끊겼을 때 처리한다.
            socket.on("close", (reasonCode, description) => {
                this.clients.delete(socket.sessionId);
                logger.info(`${socket.sessionId} disconnected: ${reasonCode} ${description}`);
            });

            // 클라이언트에게 에러가 발생했을 때 처리한다.
            socket.on("error", (error) => {
                logger.error(`${error}`);
            });

            // 클라이언트에게 SessionId를 전송한다.
            socket.sendPacket(proto.client.encode(proto.client.Connection, {
                SessionId: socket.sessionId,
            }));
        });
    }
}
