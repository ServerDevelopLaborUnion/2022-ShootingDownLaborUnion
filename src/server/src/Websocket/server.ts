import * as http from 'node:http';
import * as router from './router';
import getLogger from '../utils/logger';
import { v4 as genUUID } from 'uuid';
import { Message, request, server, connection } from 'websocket';
import { IncomingMessage, ServerResponse } from 'node:http';
import { User } from '../types/User';

type NullableWsServer = server | null;
const Logger = getLogger('Websocket');

export default new class WebsocketServer {
    port: number;
    server: http.Server;
    wsServer: NullableWsServer;

    constructor() {
        this.port = 0;
        this.wsServer = null;
        // @ts-expect-error ts(7009)
        this.server = new http.createServer((request: IncomingMessage, response: ServerResponse) => {
            response.end('');
        });
    }

    listen(port: number) {
        this.port = port;
        this.server.listen(this.port, () => {
            Logger.Info(`Server is listening on ${this.port}`);
        });

        this.wsServer = new server({ httpServer: this.server });

        this.wsServer.on("request", (request: request) => {
            const socket: connection = request.accept(null, request.origin);
            socket.user = new User(socket, genUUID());

            Logger.Debug(`${socket.user.sessionId} connected`);
            socket.on("message", (message: Message) => {
                if (message.type === 'binary') {
                    Logger.Debug(`Received: ${message.binaryData} from ${request.origin}`);
                    router.receive(message.binaryData);
                }
            });
            socket.on("close", (reasonCode: number, description: string) => {
                Logger.Debug(`${socket.user?.sessionId} disconnected: ${reasonCode} ${description}`);
            });
            socket.on("error", (error: Error) => {
                Logger.Error(`${error}`);
            });
        });
    }
}
