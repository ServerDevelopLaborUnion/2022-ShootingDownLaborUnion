import * as Logger from '../util/logger';
import { Client } from '../types/Client';
import { IHandler } from '../types/IHandler';
import proto from '../util/proto';

const logger = Logger.getLogger('SetRoleRequest');

class SetRoleRequest implements IHandler {
    id = 11;
    type = 'SetRoleRequest';
    async receive(client: Client, buffer: Buffer) {
        const SetRoleRequest: any = proto.server.decode(this.type, buffer);

        if (client.user.type == "user") {
            if (client.user.isReady) return;
            client.room?.broadcast(proto.client.encode(proto.client.SetRole, {
                UUID: client.sessionId,
                Role: SetRoleRequest.Role,
                IsReady: SetRoleRequest.IsReady
            }));
        }
    }
}

export default new SetRoleRequest();
