import { Component, OnInit } from "@angular/core";
import { TransactionService } from "../../services/transaction.service";
import { ClientService } from "../../services/client.service";
import { Transaction } from "../../models/transaction";
import { Client } from "../../models/client";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";

@Component({
    selector: "app-transactions",
    templateUrl: "./transactions.component.html",
    styleUrls: ["./transactions.component.css"],
})
export class TransactionsComponent implements OnInit {
    transactions: Transaction[] = [];
    clients: Client[] = [];
    selectedClient: Client | null = null;
    display: boolean = false;
    startDate: Date | null = null;
    endDate: Date | null = null;

    constructor(
        private transactionService: TransactionService,
        private clientService: ClientService
    ) {}

    ngOnInit() {
        this.loadClients();
    }

    loadClients() {
        this.clientService.getClients("client").subscribe(
            (clients) => {
                this.clients = clients;
            },
            (error) => console.error("Failed to load clients", error)
        );
    }

    loadTransactions() {
        if (!this.selectedClient || !this.startDate || !this.endDate) {
            return;
        }

        const accountId = this.selectedClient?.Id || 0;

        const startDateISOString = format(
            this.startDate,
            "yyyy-MM-dd'T'HH:mm:ssXXX",
            { locale: ptBR }
        );
        const endDateISOString = format(
            this.endDate,
            "yyyy-MM-dd'T'HH:mm:ssXXX",
            { locale: ptBR }
        );

        this.transactionService
            .getTransactions(accountId, startDateISOString, endDateISOString)
            .subscribe(
                (transactions) => {
                    this.transactions = transactions;
                },
                (error) => console.error("Failed to load transactions", error)
            );
    }

    showCreateTransactionDialog() {
        this.display = true;
    }

    onTransactionCreated() {
        this.display = false;
        this.loadTransactions();
    }
}
