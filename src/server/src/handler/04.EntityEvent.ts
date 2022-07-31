import { storage } from '../storage';
import { Client } from '../types/Client';
import * as Logger from '../util/logger';
import proto from '../util/proto';

const logger = Logger.getLogger('EntityEvent');

const id = 4;
const type = 'EntityEventRequest';

export default {
    id: id,
    type: type,
    receive: async (client: Client, buffer: Buffer) => {
        if (!proto.server.verify(type, buffer)) {
            logger.warn(`Invalid packet from ${client.sessionId}`);
            return;
        }

        const EntityEventRequest: any = proto.server.decode(type, buffer);
        
        const entity = storage.server.rooms.get("test")?.entities.get(EntityEventRequest.EntityUUID);
        if (entity !== undefined) {
            switch (EntityEventRequest.EventName) {
                case "DoDie":
                {

                    break;
                }
            }
            if (client.sessionId == entity.OwnerUUID) {
                client.room?.broadcast(proto.client.encode(proto.client.EntityEvent, {
                    EntityUUID: EntityEventRequest.EntityUUID,
                    EventName: EntityEventRequest.EventName,
                }), client);
            }
        }
    }
}
