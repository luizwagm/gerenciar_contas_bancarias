import { Account } from "./account";
import { Transaction } from "./transaction";

export interface Client {
    Id: number;
    FirstName: string;
    LastName: string;
    Email: string;
    DateOfBirth: string;
    Password: string;
    Role: string;
    Accounts: Account[];
    Transactions?: Transaction[];
}
