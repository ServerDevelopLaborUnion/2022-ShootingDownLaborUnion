import * as Logger from '../util/logger';
import { Client } from '../types/Client';
import { IHandler } from '../types/IHandler';
import proto from '../util/proto';
import { storage } from '../storage';

const logger = Logger.getLogger('RoomJoinRequest');

class RoomJoinRequest implements IHandler {
    id = 7;
    type = 'RoomJoinRequest';
    async receive(client: Client, buffer: Buffer) {
        const roomJoinRequest: any = proto.server.decode(this.type, buffer);
        const roomUUID = roomJoinRequest.RoomUUID;
        const password = roomJoinRequest.Password;

        const room = storage.server.getRoom(roomUUID);
        if (room) {
            if (room.password === password) {
                client.sendPacket(proto.client.encode(proto.client.RoomJoinResponse, {
                    Success: true,
                    Room: room.toProto(),
                }));
                room.addUser(client);
                logger.info(`${client.sessionId} joined room ${roomUUID}`);
            }
        }
    }
}

export default new RoomJoinRequest();