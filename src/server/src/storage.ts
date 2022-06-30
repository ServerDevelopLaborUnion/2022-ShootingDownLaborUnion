/* eslint no-unused-vars: "off" */
import WebsocketServer from "./websocket/server";

class Storage {
    public server: WebsocketServer;
    public database: any; // TODO: Database
    constructor() {
        this.server = new WebsocketServer();
    }
}

export const storage = new Storage();
