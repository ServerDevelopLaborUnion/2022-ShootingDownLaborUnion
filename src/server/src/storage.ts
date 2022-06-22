/* eslint no-unused-vars: "off" */
import websocket from "websocket";
const { connection } = websocket;

import WebsocketServer from "./websocket/server";

import { ValidUser } from "./types/User";
import { Room } from "./types/Room";

class Storage {
    public server: WebsocketServer;
    constructor() {
        this.server = new WebsocketServer();
    }
}

export const storage = new Storage();
