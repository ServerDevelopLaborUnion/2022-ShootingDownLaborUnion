import * as Logger from '../util/logger';
import { Client } from '../types/Client';
import { IHandler } from '../types/IHandler';
import proto from '../util/proto';

const logger = Logger.getLogger('RoomLeaveRequest');

class RoomLeaveRequest implements IHandler {
    id = 8;
    type = 'RoomLeaveRequest';
    async receive (client: Client, buffer: Buffer) {
        const roomLeaveRequest: any = proto.client.decode(this, buffer);
        logger.info(`${client.sessionId} requested to leave room ${client.room?.uuid}`);
        const room = client.room;
        if (room) {
            room.removeClient(client);
        }
    }
}

export default new RoomLeaveRequest();
