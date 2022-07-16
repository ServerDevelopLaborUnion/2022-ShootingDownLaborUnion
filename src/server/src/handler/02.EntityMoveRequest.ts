import { Client } from '../types/Client';
import * as Logger from '../util/logger';
import proto from '../util/proto';
import { storage } from '../storage';

const logger = Logger.getLogger('EntityMoveRequest');

const id = 2;
const type = 'EntityMoveRequest';

export default {
    id: id,
    type: type,
    receive: async (client: Client, buffer: Buffer) => {
        if (!proto.server.verify(type, buffer)) {
            logger.warn(`Invalid packet from ${client.sessionId}`);
            return;
        }

        const EntityMoveRequest: any = proto.server.decode(type, buffer);
        const entity = storage.server.rooms.get("test")?.entities.get(EntityMoveRequest.EntityUUID);
        if (entity !== undefined) {
            if (client.sessionId == entity.OwnerUUID) {
                entity.Position = EntityMoveRequest.Position;
                console.log(EntityMoveRequest);
                storage.server.broadcastPacket(proto.client.encode(proto.client.EntityMove, EntityMoveRequest), client);
            }
        }
    }
}
