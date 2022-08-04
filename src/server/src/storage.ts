/* eslint no-unused-vars: "off" */
import WebsocketServer from "./websocket/server";
import DatabaseManager from "./util/database";

// 전역적으로 사용한 변수들을 담는 곳
class Storage {
    public server: WebsocketServer;
    public database: DatabaseManager; // TODO: Database
    constructor() {
        this.server = new WebsocketServer();
        this.database = new DatabaseManager();
    }
}

export const storage = new Storage();
