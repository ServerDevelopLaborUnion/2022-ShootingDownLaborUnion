import { Client } from "./Client";

export interface IHandler {
    id: number;
    type: string;
    receive(client: Client, buffer: Buffer): void;
}