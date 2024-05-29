import { AccountService } from '../../services/account.service';
import { Component, OnInit } from "@angular/core";
import { ClientService } from "../../services/client.service";
import { Client } from "../../models/client";

@Component({
    selector: "app-account-management",
    templateUrl: "./account-management.component.html",
    styleUrls: ["./account-management.component.css"],
})
export class AccountManagementComponent implements OnInit {
    clients: Client[] = [];
    selectedClient: Client | null = null;
    transactions: number = 0;
    deleteButton: boolean = false;
    deactivateButton: boolean = false;
    userTypeClient: boolean = false;
    userTypeGerente: boolean = false;

    constructor(
        private clientService: ClientService,
        private accountService: AccountService
    ) {}

    ngOnInit() {
        this.loadClients();
        this.clientService.clientCreated$.subscribe(() => {
            this.loadClients();
        });

        var userType = localStorage.getItem("userType");

        if (userType == "client") {
            this.userTypeClient = true;
        } else {
            this.userTypeGerente = true;
        }
    }

    loadClients() {
        this.clientService.getClients("client").subscribe(
            (clients) => {
                this.clients = clients;
                this.calculateTransactions();
            },
            (error) => console.error("Failed to load clients", error)
        );
    }

    calculateTransactions() {
        this.transactions = 0;
        let totalTransactions = 0;
        this.clients.forEach((client) => {
            if (client.Transactions.length) {
                totalTransactions += client.Transactions.length;
            }
        });
        this.transactions = totalTransactions;
        console.log(this.transactions);
    }

    createAccount() {
        if (this.selectedClient) {
            const accountNumber = this.generateAccountNumber();
            const payload = {
                AccountNumber: accountNumber,
                Balance: 0.0,
                ClientId: this.selectedClient.Id,
            };

            this.accountService.createAccount(payload).subscribe(
                (response) => {
                    console.log("Account created successfully:", response);
                    this.clientService.notifyClientCreated();
                    this.deleteButton = false;
                    this.deactivateButton = false;
                },
                (error) => console.error("Failed to create account", error)
            );
        }
    }

    closeAccount() {
        if (this.selectedClient) {
            const payload = {
                ClientId: this.selectedClient.Id,
            };

            this.accountService.deleteAccount(payload).subscribe(
                (response) => {
                    console.log("Account removed successfully:", response);
                    this.clientService.notifyClientCreated();
                    this.deleteButton = false;
                    this.deactivateButton = false;
                },
                (error) => console.error(error.message, error)
            );
        }
    }

    deactivateAccount() {
        if (this.selectedClient) {
            const payload = {
                ClientId: this.selectedClient.Id,
            };

            this.accountService.deactivateAccount(payload).subscribe(
                (response) => {
                    console.log("Account deactivate successfully:", response);
                    this.clientService.notifyClientCreated();
                    this.deleteButton = false;
                    this.deactivateButton = false;
                },
                (error) => console.error(error.message, error)
            );
        }
    }

    changeOptionsUser() {
        if (this.selectedClient) {
            this.calculateTransactions();
            if (this.transactions == 0) {
                this.deactivateButton = false;
                this.deleteButton = true;
            } else {
                this.deleteButton = false;
                this.deactivateButton = true;
            }
        }
    }

    generateAccountNumber(): string {
        const chars = "0123456789";
        let accountNumber = "";
        for (let i = 0; i < 10; i++) {
            accountNumber += chars.charAt(
                Math.floor(Math.random() * chars.length)
            );
        }
        return accountNumber;
    }
}
