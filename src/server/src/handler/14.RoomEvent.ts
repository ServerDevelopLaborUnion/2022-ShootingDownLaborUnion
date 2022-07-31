import { storage } from '../storage';
import { Client } from '../types/Client';
import * as Logger from '../util/logger';
import proto from '../util/proto';

const logger = Logger.getLogger('RoomEvent');

const id = 14;
const type = 'RoomEvent';

export default {
    id: id,
    type: type,
    receive: async (client: Client, buffer: Buffer) => {
        if (!proto.server.verify(type, buffer)) {
            logger.warn(`Invalid packet from ${client.sessionId}`);
            return;
        }

        const RoomEvent: any = proto.server.decode(type, buffer);
        client.room?.broadcast(proto.client.encode(proto.client.RoomEvent, {
            RoomUUID: client.room?.uuid,
            EventName: RoomEvent.EventName,
            EventData: RoomEvent.EventData
        }));
    }
}