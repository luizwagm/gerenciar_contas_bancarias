import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { Transaction } from "../models/transaction";

@Injectable({
    providedIn: "root",
})
export class TransactionService {
    private apiUrl = "http://localhost:5004/api/v1/transaction";

    constructor(private http: HttpClient) {}

    getTransactions(
        accountId: number,
        startDate: string,
        endDate: string
    ): Observable<Transaction[]> {
        const token = localStorage.getItem("authToken");
        const headers = new HttpHeaders({
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
        });

        return this.http.get<Transaction[]>(
            `${this.apiUrl}/all/${accountId}/${startDate}/${endDate}`,
            {
                headers,
            }
        );
    }

    createTransaction(transaction: Transaction): Observable<Transaction> {
        const token = localStorage.getItem("authToken");
        const headers = new HttpHeaders({
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
        });

        return this.http.post<Transaction>(this.apiUrl, transaction, {
            headers,
        });
    }
}
