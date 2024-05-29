import { Router } from '@angular/router';
import { Component, OnInit } from "@angular/core";
import { ClientService } from "../../services/client.service";
import { Client } from "../../models/client";

@Component({
    selector: "app-clients",
    templateUrl: "./clients.component.html",
    styleUrls: ["./clients.component.css"],
})
export class ClientsComponent implements OnInit {
    clients: Client[] = [];
    display: boolean = false;
    selectedClientOnlyOne: Client | null = null;
    userTypeClient: boolean = false;
    userTypeGerente: boolean = false;

    constructor(private clientService: ClientService, private router: Router) {}

    ngOnInit() {
        this.loadClients();
        this.clientService.clientCreated$.subscribe(() => {
            this.loadClients();
        });
    }

    loadClients() {
        this.clientService.getClients("client").subscribe(
            (clients) => {
                var meuId: number = +localStorage.getItem("meuId");
                var userType = localStorage.getItem("userType");
                this.clients = clients;

                if (userType == "client") {
                    this.userTypeClient = true;
                } else {
                    this.userTypeGerente = true;
                }
                this.selectedClientOnlyOne =
                    this.clients.find((client) => client.Id === meuId) || null;
            },
            (error) => {
                console.error("Failed to load clients", error);
                if (error.status === 401) {
                    this.router.navigate(["/login"]);
                }
            }
        );
    }

    showCreateClientDialog() {
        this.display = true;
    }

    onClientCreated() {
        this.display = false;
        this.clientService.notifyClientCreated();
    }
}
