const Logger = require('../util/logger').getLogger('MoveRequest');
const proto = require('../util/proto');

const id = 3;
const type = 'MoveRequest';

module.exports = {
    id: id,
    type: type,
    receive: async (socket, buffer) => {
        if (!proto.server.verify(type, buffer)) {
            Logger.warn(`Invalid packet from ${socket.sessionId}`);
            return;
        }

        const moveRequest = proto.server.decode(type, buffer);
        socket.server.broadcastPacket(proto.client.encode(proto.client.MoveEntity, {
            EntityId: moveRequest.EntityId,
            Position: moveRequest.Position,
            Rotation: moveRequest.Rotation,
        }), socket);

        socket.server.connections;
    }
}