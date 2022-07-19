import { connection } from "websocket";
import { Account } from "./Account";
import { Client } from './Client';

export const UserType = {
    NotValidUser: 0,
    ValidUser: 1
}

export interface INotValidUser {
    type: "notValid";
    client: Client;
}

export interface IValidUser {
    type: "valid";
    client: Client;
    account: Account;
}

export type User = INotValidUser | IValidUser;