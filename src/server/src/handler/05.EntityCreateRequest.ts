import { storage } from "../storage";
import { Client } from "../types/Client";

const Logger = require('../util/logger').getLogger('EntityCreateRequest');
const proto = require('../util/proto');

const id = 5;
const type = 'EntityCreateRequest';

module.exports = {
    id: id,
    type: type,
    receive: async (client: Client, buffer: Buffer) => {
        if (!proto.server.verify(type, buffer)) {
            Logger.warn(`Invalid packet from ${client.sessionId}`);
            return;
        }

        const EntityCreateRequest = proto.server.decode(type, buffer);
        const entity = storage.server.rooms.get("testRoom")?.entities.get(EntityCreateRequest.EntityUUID);
        if (entity !== undefined) {
            if (client.sessionId == entity.OwnerUUID) {
                entity.Position = EntityCreateRequest.Position;
                storage.server.broadcastPacket(proto.client.encode(proto.client.EntityMove, {
                    EntityUUID: EntityCreateRequest.EntityUUID,
                    Position: EntityCreateRequest.Position,
                    Rotation: EntityCreateRequest.Rotation,
                }), client);
            }
        }
    }
}

