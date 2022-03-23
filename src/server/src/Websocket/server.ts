import * as http from 'node:http';
import * as router from './router';
import getLogger from '../utils/logger';
import { Message, request, server, connection } from 'websocket';
import { IncomingMessage, ServerResponse } from 'node:http';

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
            const connection: connection = request.accept(null, request.origin);
            Logger.Info(`Connection accepted from ${request.origin}`);
            connection.on("message", (message: Message) => {
                if (message.type === 'utf8') {
                    Logger.Debug(`Received: ${message.utf8Data} from ${request.origin}`);
                    connection.sendUTF(`You said: ${message.utf8Data}`);
                }
                else {
                    Logger.Debug(`Received: ${message.binaryData} from ${request.origin}`);
                    message.binaryData
                }
            });
            connection.on("close", (reasonCode: number, description: string) => {
                Logger.Debug(`Closed: ${reasonCode} ${description}`);
            });
            connection.on("error", (error: Error) => {
                Logger.Error(`${error}`);
            });
        });
    }
}
