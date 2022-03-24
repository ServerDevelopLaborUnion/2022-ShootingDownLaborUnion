import { connection } from "websocket";
import { Account } from './Account';

export enum UserType {
    User,
    ValidUser
}

export interface User {
    type: UserType.User;
    socket: connection | null;
    sessionId: string;
}

export interface ValidUser {
    type: UserType.ValidUser;
    socket: connection;
    sessionId: string;
    account: Account;
}