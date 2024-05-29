import { Transaction } from "./transaction";

export interface Account {
    AccountNumber: string;
    Balance: number;
    ClientId: number;
    Transactions?: Transaction[];
}

export interface AccountClientId {
    ClientId: number;
}
