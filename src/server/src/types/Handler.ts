import { connection } from "websocket";

export interface Handler {
    receive(socket: connection, buffer: Buffer): void;
}