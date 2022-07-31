import * as Logger from '../util/logger';
import { Client } from '../types/Client';
import { IHandler } from '../types/IHandler';
import proto from '../util/proto';

const logger = Logger.getLogger('StartGameRequest');

class StartGameRequest implements IHandler {
    id = 12;
    type = 'StartGameRequest';
    async receive(client: Client) {
        if (client.user.type == "user") {
            if (client.user.isMaster) {
                logger.info(`${client.user.account.username} is starting game`);
                client.room?.broadcast(proto.client.encode(proto.client.StartGame, {}));
            }
        }
    }
}

export default new StartGameRequest();
