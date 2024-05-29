import {
    Component,
    EventEmitter,
    Output,
    Input,
    OnInit,
    OnChanges,
    SimpleChanges,
} from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { TransactionService } from "../../services/transaction.service";
import { MessageService } from "primeng/api";
import { Client } from "src/app/models/client";
import { ClientService } from "src/app/services/client.service";

@Component({
    selector: "app-create-transaction",
    templateUrl: "./create-transaction.component.html",
    styleUrls: ["./create-transaction.component.css"],
})
export class CreateTransactionComponent implements OnInit, OnChanges {
    @Input() display: boolean = false;
    selectedClient: Client | null = null;
    @Output() displayChange = new EventEmitter<boolean>();
    @Output() transactionCreated = new EventEmitter<void>();

    createTransactionForm: FormGroup;
    clients: Client[] = [];

    constructor(
        private fb: FormBuilder,
        private transactionService: TransactionService,
        private messageService: MessageService,
        private clientService: ClientService
    ) {
        this.createTransactionForm = this.fb.group({
            AccountId: ["", Validators.required],
            Amount: ["", Validators.required],
            TransactionDate: ["", Validators.required],
            TransactionType: ["", Validators.required],
        });
    }

    ngOnInit() {
        this.loadAccounts();
    }

    ngOnChanges(changes: SimpleChanges) {
        if (changes.selectedClient) {
            this.loadAccounts();
        }
    }

    loadAccounts() {
        this.clientService.getClients("client").subscribe(
            (clients) => {
                this.clients = clients;
            },
            (error) => console.error("Failed to load clients", error)
        );
    }

    onCreate() {
        if (this.createTransactionForm.valid) {
            const newTransaction = this.createTransactionForm.value;
            newTransaction.AccountId = this.selectedClient.Id;

            this.transactionService.createTransaction(newTransaction).subscribe(
                () => {
                    this.messageService.add({
                        severity: "success",
                        summary: "Success",
                        detail: "Transaction created successfully",
                    });
                    this.createTransactionForm.reset();
                    this.displayChange.emit(false);
                    this.transactionCreated.emit();
                    this.clientService.notifyClientCreated();
                },
                (error) => {
                    console.error("Failed to create transaction", error);
                    this.messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: "Failed to create transaction",
                    });
                }
            );
        }
    }

    onHide() {
        this.displayChange.emit(false);
        this.transactionCreated.emit();
    }
}
