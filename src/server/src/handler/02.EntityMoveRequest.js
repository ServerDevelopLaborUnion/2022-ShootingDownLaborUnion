const Logger = require('../util/logger').getLogger('EntityMoveRequest');
const proto = require('../util/proto');

const id = 2;
const type = 'EntityMoveRequest';

module.exports = {
    id: id,
    type: type,
    receive: async (socket, buffer) => {
        if (!proto.server.verify(type, buffer)) {
            Logger.warn(`Invalid packet from ${socket.sessionId}`);
            return;
        }

        const EntityMoveRequest = proto.server.decode(type, buffer);
        const entity = socket.server.entityes.get(EntityMoveRequest.EntityUUID);
        if (entity !== undefined) {
            if (socket.sessionId == entity.OwnerUUID) {
                entity.Position = EntityMoveRequest.Position;
                console.log(EntityMoveRequest);
                socket.server.broadcastPacket(proto.client.encode(proto.client.EntityMove, EntityMoveRequest), socket);
            }
        }
    }
}
