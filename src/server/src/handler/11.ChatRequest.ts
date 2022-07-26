import * as Logger from '../util/logger';
import { Client } from '../types/Client';
import { IHandler } from '../types/IHandler';
import proto from '../util/proto';

const logger = Logger.getLogger('ChatRequest');

class ChatRequest implements IHandler {
    id = 11;
    type = 'ChatRequest';
    async receive(client: Client, buffer: Buffer) {
        const ChatRequest: any = proto.server.decode(this.type, buffer);

        client.room?.broadcast(proto.client.encode(proto.client.ChatMessage, {
            UUID: client.sessionId,
            Message: ChatRequest.Message,
        }));
    }
}

export default new ChatRequest();
