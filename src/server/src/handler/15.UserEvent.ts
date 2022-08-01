import { storage } from '../storage';
import { Client } from '../types/Client';
import * as Logger from '../util/logger';
import proto from '../util/proto';

const logger = Logger.getLogger('UserEvent');

const id = 15;
const type = 'UserEvent';

export default {
    id: id,
    type: type,
    receive: async (client: Client, buffer: Buffer) => {
        if (!proto.server.verify(type, buffer)) {
            logger.warn(`Invalid packet from ${client.sessionId}`);
            return;
        }

        const UserEvent: any = proto.server.decode(type, buffer);
        client.room?.broadcast(proto.client.encode(proto.client.UserEvent, {
            RoomUUID: client.room?.uuid,
            EventName: UserEvent.EventName,
            EventData: UserEvent.EventData
        }));
    }
}
