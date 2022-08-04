import { v4 } from 'uuid';
import { storage } from '../storage';
import { Client } from '../types/Client';
import { Entity } from '../types/Entity';
import { Quaternion } from '../types/Quaternion';
import { Vector2 } from '../types/Vector2';
import * as Logger from '../util/logger';
import proto from '../util/proto';

const logger = Logger.getLogger('UserEvent');

const id = 15;
const type = 'UserEvent';

export default {
    id: id,
    type: type,
    receive: async (client: Client, buffer: Buffer) => {
        if (!proto.server.verify(type, buffer)) {
            logger.warn(`Invalid packet from ${client.sessionId}`);
            return;
        }

        const UserEvent: any = proto.server.decode(type, buffer);

        switch (UserEvent.EventName) {
            case 'Debug': {
                storage.server.rooms.get('test')?.addUser(client);

                const playerEntity = new Entity(v4(), client.sessionId, "머ㅜ이망할승현아", new Vector2(0, 0), new Quaternion(0, 0, 0, 0), '{"type": 0}');
                storage.server.rooms.get('test')?.addEntity(playerEntity);
                
                const enemyEntity = new Entity(v4(), client.sessionId, "머ㅜ이망할원석아", new Vector2(8, 0), new Quaternion(0, 0, 0, 0), '{"type": 1}');
                storage.server.rooms.get('test')?.addEntity(enemyEntity);

                break;
            }
            case 'ChangeNickname': {
                if (client.user.type === 'user' || client.user.type === 'valid') {
                    client.user.account.username = UserEvent.EventData;
                }
                console.log(client.user);
                break;
            }
        }

        client.room?.broadcast(proto.client.encode(proto.client.UserEvent, {
            RoomUUID: client.room?.uuid,
            EventName: UserEvent.EventName,
            EventData: UserEvent.EventData
        }));
    }
}
