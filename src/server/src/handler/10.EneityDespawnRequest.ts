import * as Logger from '../util/logger';
import { Client } from '../types/Client';
import { IHandler } from '../types/IHandler';
import proto from '../util/proto';

const logger = Logger.getLogger('EneityDespawnRequest');

class EneityDespawnRequest implements IHandler {
    id = 10;
    type = 'EneityDespawnRequest';
    async receive(client: Client, buffer: Buffer) {
        const entityDespawnRequest: any = proto.server.decode(this.type, buffer);
        const entity = client.room?.entities.get(entityDespawnRequest.entityId);

        if (!entity) {
            logger.warn(`Entity ${entityDespawnRequest.entityId} not found`);
            return;
        }
        
        client.room?.broadcast(entity.getDespawnPacket());
    }
}
