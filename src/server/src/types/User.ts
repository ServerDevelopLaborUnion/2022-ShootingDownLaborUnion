import { connection } from "websocket";
import { Account } from "./Account";
import { Client } from './Client';

export interface INotValidUser {
    type: "notValid";
    client: Client;
}

export interface IValidUser {
    type: "valid";
    client: Client;
    account: Account;
}

export interface IUser {
    type: "user";
    client: Client;
    account: Account;
    isReady: boolean;
    isMaster: boolean;
}

export type User = INotValidUser | IValidUser | IUser;