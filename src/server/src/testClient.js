/* eslint-disable no-case-declarations */
const websocket = require('websocket');
const protobuf = require('protobufjs');
const proto = require('./util/proto');
const { Buffer } = require('buffer');
const Logger = require('./util/logger').getLogger('Client');

const client = new websocket.client();

Object.defineProperty(websocket.connection.prototype, 'sendPacket', {
    value: function (type, buffer) {
        const packet = Buffer.alloc(2);
        packet.writeUInt16BE(type, 0);
        const message = Buffer.concat([packet, buffer]);
        this.sendBytes(message);
    },
    enumerable: false,
    configurable: true,
    writable: true
});

client.connect('ws://localhost:3000');

function bufferToString(buffer) { 
    return buffer.toString('hex').toUpperCase(); 
}

let count = 0;

client.on('connect', (server) => {
    console.log('connected');

    server.on('message', (message) => {
        if (message.type === 'utf8') {
            console.log('Received: ' + message.utf8Data);
        } else if (message.type === 'binary') {
            console.log('Received: ' + message.binaryData.length + ' bytes');

            const buffer = Buffer.from(message.binaryData);
            const type = buffer.readUInt16BE(0);
            const body = buffer.slice(2);

            switch (type) {
                case proto.client.LoginResponse.id:
                    const parsed = proto.client.decode(proto.client.LoginResponse.name, body);
                    console.log(parsed);

                    server.sendPacket(2, proto.server.encode('MoveRequest', {
                        position: {
                            x: 0,
                            y: 0,
                        }
                    }));
                    break;
            }
        }
    });

    const data = {
        username: 'test',
        password: 'test'
    };
     
    protobuf.load('./proto/Server.proto', (err, root) => {
        if (err) {
            console.log(err);
            return;
        }

        const LoginRequest = root.lookupType('LoginRequest');
        const loginRequest = LoginRequest.create(data);

        const type = 0;
        const buffer = LoginRequest.encode(loginRequest).finish();
        const packet = Buffer.alloc(2);
        packet.writeUInt16BE(type, 0);
        const message = Buffer.concat([packet, buffer]);
        Logger.debug(bufferToString(message) + "-" + ++count);
        server.sendBytes(message);
    });
});