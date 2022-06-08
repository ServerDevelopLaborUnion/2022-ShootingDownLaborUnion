/* eslint no-unused-vars: "off" */
import websocket from "websocket";
const { server, connection } = websocket;

import { ValidUser } from "./types/User.js";
import { Room } from "./types/Room.js";

export const Storage = {
    server: {
        /** @type {Map<string, connection>} */
        clients: {},

        /** @type {Map<string, Room>} */
        rooms: {},

        /** @type {Map<string, ValidUser>} */
        users: {},
    }
};
