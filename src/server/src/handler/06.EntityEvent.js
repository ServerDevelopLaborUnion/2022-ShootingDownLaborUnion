const Logger = require('../util/logger').getLogger('EntityEventRequest');
const proto = require('../util/proto');

const id = 3;
const type = 'EntityEventRequest';

module.exports = {
    id: id,
    type: type,
    receive: async (socket, buffer) => {
        if (!proto.server.verify(type, buffer)) {
            Logger.warn(`Invalid packet from ${socket.sessionId}`);
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
