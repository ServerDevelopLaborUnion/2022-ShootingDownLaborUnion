/* eslint no-unused-vars: "off" */
import websocket from "websocket";
const { connection } = websocket;

import WebsocketServer from "./Websocket/server.js";

import { ValidUser } from "./types/User.js";
import { Room } from "./types/Room.js";

export const Storage = {
    /** @type {WebsocketServer} */
    server: null
};
