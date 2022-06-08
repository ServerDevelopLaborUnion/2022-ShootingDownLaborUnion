import websocket from 'websocket';
const { server, connection } = websocket;

import { v4 } from 'uuid';
import http from 'node:http';

import { Storage } from '../storage.js';
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

export const WebsocketServer = new class WebsocketServer {
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
        this.server.broadcastPacket = function (buffer, socket = null) {
            logger.debug(`Broadcasting: ${buffer.length} bytes`);
            if (socket) {
                Storage.server.clients.forEach(client => {
                    if (client != socket) {
                        client.sendPacket(buffer);
                    }
                });
            } else {
                Storage.server.clients.forEach(client => {
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
            
            Storage.server.clients.set(socket.sessionId, socket);

            // 클라이언트에게 SessionId를 전송한다.
            socket.sendPacket(proto.client.encode(proto.client.Connection, {
                SessionId: socket.sessionId,
            }));

            logger.info(`${socket.sessionId} connected`);

            const newEntity = {
                Entity: {
                    UUID: v4(),
                    OwnerUUID: socket.sessionId,
                    Position: { X: 0, Y: 0 },
                    TargetPosition: { X: 0, Y: 0 },
                    Rotation: { X: 0, Y: 0, Z: 0, W: 0 },
                    Data: `{"type":"0"}`
                }
            };

            socket.server.broadcastPacket(proto.client.encode(proto.client.EntityCreate, newEntity));

            this.entityes.forEach(entity => {
                socket.sendPacket(proto.client.encode(proto.client.EntityCreate, {
                    Entity: entity
                }));
            });

            this.entityes.set(newEntity.Entity.UUID, newEntity.Entity);

            // 클라이언트에게 메시지를 받았을 때 처리한다.
            socket.on("message", (message) => {
                if (message.type === 'binary') {
                    router.receive(socket, message.binaryData);
                }
            });

            // 클라이언트에게 연결이 끊겼을 때 처리한다.
            socket.on("close", (reasonCode, description) => {
                this.connections.delete(socket.sessionId);
                logger.info(`${socket.sessionId} disconnected: ${reasonCode} ${description}`);
            });

            // 클라이언트에게 에러가 발생했을 때 처리한다.
            socket.on("error", (error) => {
                logger.error(`${error}`);
            });
        });
    }
}
