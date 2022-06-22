import { Client } from "./Client";

export interface Handler {
    id: number;
    type: string;
    receive(client: Client, buffer: Buffer): void;
}