import * as Logger from '../util/logger';
import { Client } from '../types/Client';
import { IHandler } from '../types/IHandler';

const logger = Logger.getLogger('RoomListRequest');

class RoomListRequest implements IHandler {
    id = 9;
    type = 'RoomListRequest';
    async receive(client: Client, buffer: Buffer) {
        // TODO: Implement
    }
}