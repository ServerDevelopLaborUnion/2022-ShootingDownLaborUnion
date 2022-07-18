import { Client } from '../types/Client';
import * as Logger from '../util/logger';
import proto from '../util/proto';
import { storage } from '../storage';

const logger = Logger.getLogger('EntityUpdateRequest');

const id = 3;
const type = 'EntityUpdateRequest';

export default {
    id: id,
    type: type,
    receive: async (client: Client, buffer: Buffer) => {
        if (!proto.server.verify(type, buffer)) {
            logger.warn(`Invalid packet from ${client.sessionId}`);
            return;
        }

        const EntityUpdateRequest: any = proto.server.decode(type, buffer);
        const entity = storage.server.rooms.get("testRoom")?.entities.get(EntityUpdateRequest.EntityUUID);
        if (entity !== undefined) {
            if (client.sessionId == entity.OwnerUUID) {
                storage.server.broadcastPacket(proto.client.encode(proto.client.EntityUpdate, {
                    EntityUUID: EntityUpdateRequest.EntityUUID,
                    EventName: EntityUpdateRequest.EventName,
                }), client);
            }
        }
    }
}
