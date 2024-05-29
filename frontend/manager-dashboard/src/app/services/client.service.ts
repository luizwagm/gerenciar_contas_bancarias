import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, Subject } from "rxjs";
import { Client } from "../models/client";

@Injectable({
    providedIn: "root",
})
export class ClientService {
    private apiUrl = "http://localhost:5004/api/v1/client";
    private clientCreatedSource = new Subject<void>();

    clientCreated$ = this.clientCreatedSource.asObservable();

    constructor(private http: HttpClient) {}

    getClients(role: string): Observable<Client[]> {
        const token = localStorage.getItem("authToken");
        const headers = new HttpHeaders({
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
        });

        const body = { Role: role };

        return this.http.post<Client[]>(`${this.apiUrl}/get`, body, {
            headers,
        });
    }

    createClient(client: Client): Observable<Client> {
        const token = localStorage.getItem("authToken");
        const headers = new HttpHeaders({
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
        });
        return this.http.post<Client>(`${this.apiUrl}/create`, client, {
            headers,
        });
    }

    notifyClientCreated() {
        this.clientCreatedSource.next();
    }
}
