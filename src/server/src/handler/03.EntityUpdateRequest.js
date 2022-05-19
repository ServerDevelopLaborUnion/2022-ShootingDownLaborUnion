const Logger = require('../util/logger').getLogger('EntityUpdateRequest');
const proto = require('../util/proto');

const id = 3;
const type = 'EntityUpdateRequest';

module.exports = {
    id: id,
    type: type,
    receive: async (socket, buffer) => {
        if (!proto.server.verify(type, buffer)) {
            Logger.warn(`Invalid packet from ${socket.sessionId}`);
            return;
        }

        const EntityUpdateRequest = proto.server.decode(type, buffer);
        const entity = socket.server.entityes.get(EntityUpdateRequest.EntityUUID);
        if (entity !== undefined) {
            if (socket.sessionId == entity.OwnerUUID) {
                entity.Data = EntityUpdateRequest.Data;
                socket.server.broadcastPacket(proto.client.encode(proto.client.EntityUpdate, {
                    EntityUUID: EntityUpdateRequest.EntityUUID,
                    Data: EntityUpdateRequest.Data,
                }), socket);
            }
        }
    }
}
