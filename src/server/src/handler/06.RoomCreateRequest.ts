import { storage } from './../storage';
import * as Logger from '../util/logger';
import { Client } from '../types/Client';
import { IHandler } from '../types/IHandler';
import { Room } from '../types/Room';
import proto from '../util/proto';

const logger = Logger.getLogger('RoomCreateRequest');

class RoomCreateRequest implements IHandler {
    id = 6;
    type = 'RoomCreateRequest';
    async receive (client: Client, buffer: Buffer) {
        const RoomCreateRequest: any = proto.server.decode(this.type, buffer);
        if (client.room === undefined) {
            const room = new Room(RoomCreateRequest.Name, RoomCreateRequest.Password);
            room.addUser(client);

            client.sendPacket(proto.client.encode(proto.client.RoomJoinResponse, {
                Success: true,
                Room: room.toProto(),
            }));
        }
    }
}