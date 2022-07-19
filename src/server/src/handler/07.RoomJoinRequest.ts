import * as Logger from '../util/logger';
import { Client } from '../types/Client';
import { IHandler } from '../types/IHandler';

const logger = Logger.getLogger('RoomJoinRequest');

class RoomJoinRequest implements IHandler {
    id = 7;
    type = 'RoomJoinRequest';
    async receive (client: Client, buffer: Buffer) {
        // TODO: Implement
    }
}