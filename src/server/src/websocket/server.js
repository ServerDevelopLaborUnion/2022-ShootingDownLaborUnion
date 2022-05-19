const { server, connection } = require('websocket');
const { UserType } = require('../types/User');
const { v4 } = require('uuid');
const http = require('node:http');
const Logger = require('../util/logger').getLogger('Websocket');
const proto = require('../util/proto');
const router = require('./router');

Object.defineProperty(connection.prototype, 'sendPacket', {
    value: function (buffer) {
        Logger.debug(`Sending: ${buffer.length} bytes to ${this.sessionId}`);
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
    /** @type {Map<string, connection>} */
    connections = new Map();
    /** @type {Map<string, Entity>} */
    entityes = new Map();

    constructor() {
        this.port = 0;
        this.wsServer = null;
        this.server = new http.createServer((request, response) => {
            response.end('');
        });

        connection.prototype.server = this.server;
        this.server.connections = this.connections;
        this.server.entityes = this.entityes;
        this.server.broadcastPacket = function (buffer, socket = null) {
            Logger.debug(`Broadcasting: ${buffer.length} bytes`);
            if (socket) {
                socket.server.connections.forEach(connection => {
                    if (connection != socket) {
                        connection.sendPacket(buffer);
                    }
                });
            } else {
                this.connections.forEach(connection => {
                    connection.sendPacket(buffer);
                });
            }
        }
    }

    listen(port) {
        this.port = port;
        this.server.listen(this.port, () => {
            Logger.info(`Server is listening on ${this.port}`);
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

            this.connections.set(socket.sessionId, socket);

            // 클라이언트에게 SessionId를 전송한다.
            socket.sendPacket(proto.client.encode(proto.client.Connection, {
                SessionId: socket.sessionId,
            }));

            Logger.info(`${socket.sessionId} connected`);

            const newEntity = {
                Entity: {
                    UUID: v4(),
                    OwnerUUID: socket.sessionId,
                    Position: { X: 0, Y: 0 },
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
                const entityCount = this.entityes.size;
                this.entityes.forEach(entity => {
                    if (entity.OwnerUUID === socket.sessionId) {
                        this.server.broadcastPacket(proto.client.encode(proto.client.EntityRemove, {
                            UUID: entity.UUID
                        }));
                        this.entityes.delete(entity.UUID);
                    }
                });
                Logger.info(`${socket.sessionId} disconnected: ${reasonCode} ${description} ${entityCount - this.entityes.size} entities removed.`);
            });

            // 클라이언트에게 에러가 발생했을 때 처리한다.
            socket.on("error", (error) => {
                Logger.error(`${error}`);
            });
        });
    }
}
