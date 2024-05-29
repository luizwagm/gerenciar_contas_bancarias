import { Component, EventEmitter, Output, Input } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ClientService } from "../../services/client.service";
import { Client } from "../../models/client";
import { MessageService } from "primeng/api";

@Component({
    selector: "app-create-client",
    templateUrl: "./create-client.component.html",
    styleUrls: ["./create-client.component.css"],
})
export class CreateClientComponent {
    @Input() display: boolean = false;
    @Output() displayChange = new EventEmitter<boolean>();
    @Output() clientCreated = new EventEmitter<void>();

    createClientForm: FormGroup;

    constructor(
        private fb: FormBuilder,
        private clientService: ClientService,
        private messageService: MessageService
    ) {
        this.createClientForm = this.fb.group({
            FirstName: ["", Validators.required],
            LastName: ["", Validators.required],
            Email: ["", [Validators.required, Validators.email]],
            DateOfBirth: ["", Validators.required],
            Password: ["", Validators.required],
            Role: ["client"],
        });
    }

    onCreate() {
        if (this.createClientForm.valid) {
            const newClient: Client = this.createClientForm.value;
            this.clientService.createClient(newClient).subscribe(
                () => {
                    this.messageService.add({
                        severity: "success",
                        summary: "Success",
                        detail: "Client created successfully",
                    });
                    this.createClientForm.reset();
                    this.displayChange.emit(false);
                    this.clientCreated.emit();
                    this.clientService.notifyClientCreated();
                    this.createClientForm.reset();
                },
                (error) => {
                    console.error("Failed to create client", error);
                    this.messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: "Failed to create client",
                    });
                }
            );
        }
    }

    onHide() {
        this.clientCreated.emit();
    }
}
