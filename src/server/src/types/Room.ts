/* eslint no-unused-vars: "off" */
import { v4 } from "uuid";
import { User } from "./User";
import { Entity } from './Entity';

export class Room {
    id: string;
    name: string;
    password: string | null;
    users: Map<string, User>;
    entitys: Map<string, Entity>;

    constructor(name: string, password: string | null) {
        this.id = v4();
        this.name = name;
        this.password = password;
        this.users = new Map();
        this.entitys = new Map();
    }

    GetPlayerCount() {
        return this.users.size;
    }
}