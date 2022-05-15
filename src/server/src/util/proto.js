const protobuf = require('protobufjs');

const protoPath = '../proto/';

const server = protobuf.loadSync(`${protoPath}Server.proto`);
const client = protobuf.loadSync(`${protoPath}Client.proto`);

module.exports = {
    server: {
        encode: (type, message) => {
            const buffer = server.lookupType(`Server.${type}`).encode(message).finish();
            return buffer;
        },
        decode: (type, buffer) => {
            const message = server.lookupType(`Server.${type}`).decode(buffer);
            return message;
        },
        verify: (type, buffer) => {
            const message = server.lookupType(`Server.${type}`).verify(buffer);
            return message == null;
        },
        LoginRequest: 0,
        TokenLoginRequest: 1,
    },
    client: {
        encode: (type, message) => {
            const buffer = client.lookupType(`Client.${type}`).encode(message).finish();
            return buffer;
        },
        decode: (type, buffer) => {
            const message = client.lookupType(`Client.${type}`).decode(buffer);
            return message;
        },
        verify: (type, buffer) => {
            const message = client.lookupType(`Client.${type}`).verify(buffer);
            return message != null;
        },
        LoginResponse: {
            id: 0,
            name: 'LoginResponse',
        },
    }
}