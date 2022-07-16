/* eslint no-unused-vars: "off" */
import { v4 } from "uuid";
import { User } from "./User";
import { Entity } from './Entity';
import { Client } from "./Client";

export class Room {
    id: string;
    name: string;
    password: string | null;
    clients: Client[];
    entities: Map<string, Entity>;

    constructor(name: string, password: string | null) {
        this.id = v4();
        this.name = name;
        this.password = password;
        this.clients = [];
        this.entities = new Map();
    }

    broadcast(buffer: Buffer, except: Client | null = null) {
        for (const client of this.clients) {
            if (client !== except) {
                client.sendPacket(buffer);
            }
        }
    }

    getPlayerCount() {
        return this.clients.length;
    }

    addClient(client: Client) {
        this.clients.push(client);
    }

    addEntity(entity: Entity) {
        this.entities.set(entity.UUID, entity);

        this.broadcast(entity.getSpawnPacket());
    }
}