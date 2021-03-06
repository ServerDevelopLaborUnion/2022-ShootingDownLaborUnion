/* eslint no-unused-vars: "off" */
import { v4 } from "uuid";
import { User } from "./User";
import { Entity } from './Entity';
import { Client } from "./Client";

export class Room {
    uuid: string;
    name: string;
    password: string | null;
    clients: Client[];
    entities: Map<string, Entity>;

    constructor(name: string, password: string | null) {
        this.uuid = v4();
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

    addUser(client: Client) {
        if (this.clients.indexOf(client) !== -1) return;
        client.room = this;
        this.clients.push(client);
        this.entities.forEach(entity => {
            client.sendPacket(entity.getSpawnPacket());
        });
    }

    removeClient(client: Client) {
        client.room = null;
        this.clients.splice(this.clients.indexOf(client), 1);
        this.entities.forEach(entity => {
            if (entity.OwnerUUID === client.sessionId) {
                this.removeEntity(entity);
            }
        });
    }

    addEntity(entity: Entity) {
        this.entities.set(entity.UUID, entity);
        this.broadcast(entity.getSpawnPacket());
    }

    removeEntity(entity: Entity) {
        this.entities.delete(entity.UUID);
        this.broadcast(entity.getDespawnPacket());
    }

    toProto() {
        return {
            Info: {
                UUID: this.uuid,
                Name: this.name,
                IsPrivate: this.password !== null,
                PlayerCount: this.getPlayerCount(),
            },
            Users: this.clients.map(client => client.toProto()),
        };
    }
}