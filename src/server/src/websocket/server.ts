import { server, connection } from 'websocket';

import { v4 } from 'uuid';
import http from 'node:http';

import { Room } from '../types/Room';
import * as Logger from'../util/logger';
import * as router from'./router';
import proto from'../util/proto';
import { Client } from '../types/Client';
import { Entity } from '../types/Entity';
import { Vector2 } from '../types/Vector2';
import { Quaternion } from '../types/Quaternion';

const logger = Logger.getLogger('Websocket');

type UserType = {
    UUID: string,
    Name: string,
    Weapon: number,
    Role: number,
    IsReady: boolean,
    IsMaster: boolean,
}

type RoomInfoType = {
    UUID: string,
    Name: string,
    IsPrivate: boolean,
    PlayerCount: number
}

type RoomType = {
    Info: RoomInfoType[],
    Users: UserType[]
}

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
        this.rooms.set('test', new Room('test', null));
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
        });

        this.wsServer.on("request", async (request) => {
            const socket = request.accept(null, request.origin);
            const client = new Client(socket);

            this.clients.set(client.sessionId, client);

            logger.info(`${client.sessionId} connected`);

            // ????????????????????? ???????????? ????????? ??? ????????????.
            socket.on("message", (message) => {
                if (message.type === 'binary') {
                    router.receive(client, message.binaryData);
                }
            });

            // ????????????????????? ????????? ????????? ??? ????????????.
            socket.on("close", (reasonCode, description) => {
                this.clients.delete(client.sessionId);
                if (client.room !== null) {
                    client.room.removeClient(client);
                }
                logger.info(`${client.sessionId} disconnected: ${reasonCode} ${description}`);
            });

            // ????????????????????? ????????? ???????????? ??? ????????????.
            socket.on("error", (error) => {
                logger.error(`${error}`);
            });

            // ????????????????????? SessionId??? ????????????.
            client.sendPacket(proto.client.encode(proto.client.Connection, {
                SessionId: client.sessionId,
            }));

            this.rooms.get('test')?.addUser(client);

            const playerEntity = new Entity(v4(), client.sessionId, "????????????????????????", new Vector2(0, 0), new Quaternion(0, 0, 0, 0), '{"type": 0}');
            this.rooms.get('test')?.addEntity(playerEntity);
            
            const enemyEntity = new Entity(v4(), client.sessionId, "????????????????????????", new Vector2(8, 0), new Quaternion(0, 0, 0, 0), '{"type": 1}');
            this.rooms.get('test')?.addEntity(enemyEntity);
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

    getRoomInfoList() {
        const roomInfoList: RoomInfoType[] = [];
        this.rooms.forEach((room, key) => {
            roomInfoList.push({
                UUID: key,
                Name: room.name,
                IsPrivate: room.password !== "",
                PlayerCount: room.clients.length,
            });
        }
        );
        return roomInfoList;
    }

    getRoom(roomId: string) {
        return this.rooms.get(roomId);
    }
}
