import { Account } from './Account';
import { Client } from './Client';
import { IValidUser } from './User';

export class RoomUser implements IValidUser {
    type: "valid";
    client: Client;
    account: Account;
    constructor(user: IValidUser) {
        this.type = "valid";
        this.client = user.client;
        this.account = user.account;
    }
}