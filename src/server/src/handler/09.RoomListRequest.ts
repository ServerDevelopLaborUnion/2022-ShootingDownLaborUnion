import * as Logger from '../util/logger';
import { Client } from '../types/Client';
import { IHandler } from '../types/IHandler';
import proto from '../util/proto';
import { storage } from '../storage';

const logger = Logger.getLogger('RoomListRequest');

class RoomListRequest implements IHandler {
    id = 9;
    type = 'RoomListRequest';
    async receive(client: Client, buffer: Buffer) {
        const roomInfoList = storage.server.getRoomInfoList();
        client.sendPacket(proto.client.encode(proto.client.RoomListResponse, {
            
        }));
    }
}

export default new RoomListRequest();