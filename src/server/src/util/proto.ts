import { Buffer } from 'buffer';
import protobuf, { Message } from 'protobufjs';

const protoPath = './proto/';

const server = protobuf.loadSync(`${protoPath}Server.proto`);
const client = protobuf.loadSync(`${protoPath}Client.proto`);

interface IProto {
    id: number;
    type: string;
}

export default {
    server: {
        encode: (type: string, message: any) => {
            const buffer = server.lookupType(`Server.${type}`).encode(message).finish();
            return buffer;
        },
        decode: (type: string, buffer: Buffer) => {
            const message = server.lookupType(`Server.${type}`).decode(buffer);
            return message;
        },
        verify: (type: string, buffer: Buffer) => {
            const message = server.lookupType(`Server.${type}`).verify(buffer);
            return message == null;
        },
        LoginRequest: {
            id: 0,
            type: 'LoginRequest',
        },
        TokenLoginRequest: {
            id: 1,
            type: 'TokenLoginRequest',
        },
    },
    client: {
        encode: (type: IProto, message: any) => {
            const packet = Buffer.alloc(2);
            packet.writeUInt16BE(type.id, 0);
            const buffer = client.lookupType(`Client.${type.type}`).encode(message).finish();
            return Buffer.concat([packet, buffer]);
        },
        decode: (type: IProto, buffer: Buffer) => {
            const message = client.lookupType(`Client.${type.type}`).decode(buffer);
            return message;
        },
        verify: (type: IProto, buffer: Buffer) => {
            const message = client.lookupType(`Client.${type.type}`).verify(buffer);
            return message != null;
        },
        Connection: {
            id: 0,
            type: 'Connection',
        },
        LoginResponse: {
            id: 1,
            type: 'LoginResponse',
        },
        EntityCreate: {
            id: 2,
            type: 'EntityCreate',
        },
        EntityMove: {
            id: 3,
            type: 'EntityMove',
        },
        EntityRemove: {
            id: 4,
            type: 'EntityRemove',
        },
        EntityUpdate: {
            id: 5,
            type: 'EntityUpdate',
        },
        EntityEvent: {
            id: 6,
            type: 'EntityEvent',
        }
    }
}
