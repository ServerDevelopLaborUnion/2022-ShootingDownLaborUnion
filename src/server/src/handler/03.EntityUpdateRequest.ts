import * as Logger from '../util/logger';
import proto from '../util/proto';

const logger = Logger.getLogger('EntityUpdateRequest');

const id = 3;
const type = 'EntityUpdateRequest';

export default {
    id: id,
    type: type,
    receive: async (socket, buffer) => {
        if (!proto.server.verify(type, buffer)) {
            logger.warn(`Invalid packet from ${socket.sessionId}`);
            return;
        }

        const EntityUpdateRequest = proto.server.decode(type, buffer);
        const entity = socket.server.entityes.get(EntityUpdateRequest.EntityUUID);
        if (entity !== undefined) {
            if (socket.sessionId == entity.OwnerUUID) {
                socket.server.broadcastPacket(proto.client.encode(proto.client.EntityUpdate, {
                    EntityUUID: EntityUpdateRequest.EntityUUID,
                    EventName: EntityUpdateRequest.EventName,
                }), socket);
            }
        }
    }
}
