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

// 클라한테 보낼 패킷 형식 정의
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
        this.port = 0;
        this.server = null;
        this.wsServer = null;
        // 디버그용 테스트 룸 기본 생성
        this.rooms.set('test', new Room('test', null));
    }

    listen(port: number) {
        this.port = port;

        // http 서버를 만든다.
        this.server = http.createServer(function (request, response) {
            // http 요청이 들어오면 그냥 404 보내고 종료한다.
            console.log((new Date()) + ' Received request for ' + request.url);
            response.writeHead(404);
            response.end();
        });

        // http 서버를 가지고 웹소켓 서버를 만든다.
        this.wsServer = new server({
            httpServer: this.server,
            closeTimeout: 5000,
            // 자동으로 연결 허용 해제
            autoAcceptConnections: false,
        });

        this.wsServer.on("connect", (connection: connection) => {
            logger.info(`Client connected: ${connection.remoteAddress}`);
        });

        this.wsServer.on("request", async (request) => {
            // connection을 수락하고 기타 정보를 담은 Client 객체를 생성한다.
            const client = new Client(request.accept(null, request.origin));

            // 클라이언트 리스트에 추가한다.
            this.clients.set(client.sessionId, client);

            logger.info(`${client.sessionId} connected`);

            // 클라이언트에게 메시지를 받았을 때 처리한다.
            client.socket.on("message", (message) => {
                // 메시지가 바이너리 형식이라면 처리한다.
                if (message.type === 'binary') {
                    // 라우터에 메시지를 전달한다.
                    router.receive(client, message.binaryData);
                }
            });

            // 클라이언트에게 연결이 끊겼을 때 처리한다.
            client.socket.on("close", (reasonCode, description) => {
                // 클라이언트 리스트에서 제거한다.
                this.clients.delete(client.sessionId);

                // 클라이언트가 방에 있었다면 방에서 제거한다.
                if (client.room !== null) {
                    client.room.removeClient(client);
                }
                logger.info(`${client.sessionId} disconnected: ${reasonCode} ${description}`);
            });

            // 클라이언트에게 에러가 발생했을 때 처리한다.
            client.socket.on("error", (error) => {
                logger.error(`${error}`);
            });

            // 클라이언트에게 SessionId를 전송한다.
            client.sendPacket(proto.client.encode(proto.client.Connection, {
                SessionId: client.sessionId,
            }));
        });

        // websocket 서버를 시작한다.
        this.server.listen(this.port, () => {
            logger.info(`Server is listening on ${this.port}`);
        });
    }

    // 모든 클라이언트에게 메시지를 보낸다.
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

    // RoomnInfo List 를 만들어 반환한다.
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

    // Room을 ID로 찾아 반환한다.
    getRoomByRoomID(roomId: string) {
        return this.rooms.get(roomId);
    }
}
