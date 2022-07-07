import { server, connection } from 'websocket';

import { v4 } from 'uuid';
import http from 'node:http';

import { UserType } from'../types/User';
import { Room } from '../types/Room';
import * as Logger from'../util/logger';
import * as router from'./router';
import proto from'../util/proto';
import { Client } from '../types/Client';
import { Entity } from '../types/Entity';
import { Vector2 } from '../types/Vector2';
import { Quaternion } from '../types/Quaternion';

const logger = Logger.getLogger('Websocket');

export default class WebsocketServer {
    port: number;
    server: http.Server | null;
    wsServer: server | null;
    clients: Map<string, Client> = new Map();
    rooms: Map<string, Room> = new Map();

    constructor() {
        this.port = 3000;
        this.server = null;
        this.wsServer = null;
        // this.wsServer = new server({ httpServer: this.server });
    }

    listen(port: number) {
        this.port = port;
        this.server = http.createServer(function (request, response) {
            console.log((new Date()) + ' Received request for ' + request.url);
            response.writeHead(404);
            response.end();
        });

        this.wsServer = new server({
            httpServer: this.server,
            closeTimeout: 5000,
            autoAcceptConnections: false,
        });

        this.wsServer.on("connect", (connection: connection) => {
            logger.info(`Client connected: ${connection.remoteAddress}`);
            connection.on("message", (message) => {
                logger.debug(`Received: ${message} bytes`);
            });
        });

        this.wsServer.on("request", async (request) => {
            const socket = request.accept(null, request.origin);
            const client = new Client(socket);

            this.clients.set(client.sessionId, client);

            logger.info(`${client.sessionId} connected`);

            // 클라이언트에게 메시지를 받았을 때 처리한다.
            socket.on("message", (message) => {
                if (message.type === 'binary') {
                    router.receive(client, message.binaryData);
                }
            });

            // 클라이언트에게 연결이 끊겼을 때 처리한다.
            socket.on("close", (reasonCode, description) => {
                this.clients.delete(client.sessionId);
                logger.info(`${client.sessionId} disconnected: ${reasonCode} ${description}`);
            });

            // 클라이언트에게 에러가 발생했을 때 처리한다.
            socket.on("error", (error) => {
                logger.error(`${error}`);
            });

            // 클라이언트에게 SessionId를 전송한다.
            client.sendPacket(proto.client.encode(proto.client.Connection, {
                SessionId: client.sessionId,
            }));

            this.broadcastPacket(proto.client.encode(proto.client.EntityCreate, {
                Entity: new Entity(v4(), client.sessionId, "머ㅜ이망할승현아", new Vector2(0, 0), new Quaternion(0, 0, 0, 0), '{"type": 0}')}));
            client.sendPacket(proto.client.encode(proto.client.EntityCreate, {
                Entity: new Entity(v4(), client.sessionId, "머ㅜ이망할원석아", new Vector2(3, 0), new Quaternion(0, 0, 0, 0), '{"type": 1}')
            }));
        });

        this.server.listen(this.port, () => {
            logger.info(`Server is listening on ${this.port}`);
        });
    }

    broadcastPacket (buffer: Buffer, socket: Client | null = null) {
        logger.debug(`Broadcasting: ${buffer.length} bytes`);
        if (socket) {
            this.clients.forEach(client => {
                if (client != socket) {
                    client.socket.sendBytes(buffer);
                }
            });
        } else {
            this.clients.forEach(client => {
                client.socket.sendBytes(buffer);
            });
        }
    }
}
