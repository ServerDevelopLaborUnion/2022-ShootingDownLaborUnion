/* eslint no-unused-vars: "off" */
import WebsocketServer from "./websocket/server";
import DatabaseManager from "./util/database";

class Storage {
    public server: WebsocketServer;
    public database: DatabaseManager; // TODO: Database
    constructor() {
        this.server = new WebsocketServer();
        this.database = new DatabaseManager();
    }
}

export const storage = new Storage();
