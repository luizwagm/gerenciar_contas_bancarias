export interface Transaction {
    Id?: number;
    TransactionDate?: Date;
    Description?: string;
    Amount?: number;
    TransactionType?: string;
    AccountId?: number;
}