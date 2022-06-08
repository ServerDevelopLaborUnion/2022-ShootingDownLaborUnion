/* eslint no-unused-vars: "off" */
import { v4 } from "uuid";
import { ValidUser } from "./User.js";

export class Room {
    constructor(name, password) {
        /** @type {string} */
        this.id = v4();
        /** @type {string} */
        this.name = name;
        /** @type {string | null} */
        this.password = null;
        /** @type {Map<string, ValidUser>} */
        this.users = new Map();
    }

    GetPlayerCount() {
        return this.users.size;
    }
}