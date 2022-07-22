import * as Logger from '../util/logger';
import { Client } from '../types/Client';
import { IHandler } from '../types/IHandler';

const logger = Logger.getLogger('RoomLeaveRequest');

class RoomLeaveRequest implements IHandler {
    id = 8;
    type = 'RoomLeaveRequest';
    async receive (client: Client, buffer: Buffer) {
        // TODO: Implement
    }
}

export default new RoomLeaveRequest();