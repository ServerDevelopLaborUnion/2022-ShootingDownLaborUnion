import * as Logger from '../util/logger.js';
import proto from '../util/proto.js';

const logger = Logger.getLogger('EntityMoveRequest');

const id = 2;
const type = 'EntityMoveRequest';

export default {
    id: id,
    type: type,
    receive: async (socket, buffer) => {
        if (!proto.server.verify(type, buffer)) {
            logger.warn(`Invalid packet from ${socket.sessionId}`);
            return;
        }

        const EntityMoveRequest = proto.server.decode(type, buffer);
        const entity = socket.server.entityes.get(EntityMoveRequest.EntityUUID);
        if (entity !== undefined) {
            if (socket.sessionId == entity.OwnerUUID) {
                entity.Position = EntityMoveRequest.Position;
                socket.server.broadcastPacket(proto.client.encode(proto.client.EntityMove, {
                    EntityUUID: EntityMoveRequest.EntityUUID,
                    Position: EntityMoveRequest.Position,
                    Rotation: EntityMoveRequest.Rotation,
                }), socket);
            }
        }
    }
}
