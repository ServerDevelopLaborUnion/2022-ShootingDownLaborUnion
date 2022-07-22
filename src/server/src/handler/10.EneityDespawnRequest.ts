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
        const entity = client.room?.entities.get(entityDespawnRequest.EntityUUID);

        if (!entity) {
            logger.warn(`Entity ${entityDespawnRequest.EntityUUID} not found`);
            return;
        }
        
        client.room?.removeEntity(entity);
        client.room?.broadcast(entity.getDespawnPacket());

        console.log(`Entity ${entityDespawnRequest.EntityUUID} despawned`);
    }
}

export default new EneityDespawnRequest();