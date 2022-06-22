import * as Logger from '../util/logger';
import proto from '../util/proto';

const logger = Logger.getLogger('EntityEvent');

const id = 4;
const type = 'EntityEventRequest';

export default {
    id: id,
    type: type,
    receive: async (socket, buffer) => {
        if (!proto.server.verify(type, buffer)) {
            logger.warn(`Invalid packet from ${socket.sessionId}`);
            return;
        }

        const EntityEventRequest = proto.server.decode(type, buffer);
        const entity = socket.server.entityes.get(EntityEventRequest.EntityUUID);
        if (entity !== undefined) {
            if (socket.sessionId == entity.OwnerUUID) {
                socket.server.broadcastPacket(proto.client.encode(proto.client.EntityEvent, {
                    EntityUUID: EntityEventRequest.EntityUUID,
                    EventName: EntityEventRequest.EventName,
                }), socket);
            }
        }
    }
}
