const Logger = require('../util/logger').getLogger('EntityCreateRequest');
const proto = require('../util/proto');

const id = 5;
const type = 'EntityCreateRequest';

module.exports = {
    id: id,
    type: type,
    receive: async (socket, buffer) => {
        if (!proto.server.verify(type, buffer)) {
            Logger.warn(`Invalid packet from ${socket.sessionId}`);
            return;
        }

        const EntityCreateRequest = proto.server.decode(type, buffer);
        const entity = socket.server.entityes.get(EntityCreateRequest.EntityUUID);
        if (entity !== undefined) {
            if (socket.sessionId == entity.OwnerUUID) {
                entity.Position = EntityCreateRequest.Position;
                socket.server.broadcastPacket(proto.client.encode(proto.client.EntityMove, {
                    EntityUUID: EntityCreateRequest.EntityUUID,
                    Position: EntityCreateRequest.Position,
                    Rotation: EntityCreateRequest.Rotation,
                }), socket);
            }
        }
    }
}

