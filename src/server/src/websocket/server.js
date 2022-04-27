const http = require('node:http');
const router = require('./router');
const Logger = require('../util/logger').getLogger('Websocket');
const { v4 } = require('uuid');
const { server } = require('websocket');
const { UserType } = require('../types/User');

exports.WebsocketServer = new class WebsocketServer {
    port;
    server;
    wsServer;

    constructor() {
        this.port = 0;
        this.wsServer = null;
        // @ts-expect-error ts(7009)
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
