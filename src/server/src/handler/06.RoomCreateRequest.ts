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
        console.log(client.room);
        
        if (client.room === null) {
            const room = new Room(RoomCreateRequest.Name, RoomCreateRequest.Password);
            storage.server.rooms.set(room.uuid, room);
            logger.info(`Room ${room.uuid} created`);

            if (client.user.type === "valid") {
                client.user = {
                    type: "user",
                    client: client,
                    account: client.user.account,
                    role: 0,
                    isReady: false,
                    isMaster: true
                }
            }

            room.addUser(client);

            client.sendPacket(proto.client.encode(proto.client.RoomJoinResponse, {
                Success: true,
                Room: room.toProto(),
            }));
        }
    }
}

export default new RoomCreateRequest();