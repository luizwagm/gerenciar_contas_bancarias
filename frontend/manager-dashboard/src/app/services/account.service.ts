import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Account, AccountClientId } from "../models/account";

@Injectable({
    providedIn: "root",
})
export class AccountService {
    private baseUrl = "http://localhost:5004/api/v1/account";

    constructor(private http: HttpClient) {}

    createAccount(account: Account): Observable<Account> {
        const token = localStorage.getItem("authToken");
        const headers = { Authorization: `Bearer ${token}` };

        return this.http.post<Account>(this.baseUrl, account, { headers });
    }

    deleteAccount(account: AccountClientId): Observable<Account> {
        const token = localStorage.getItem("authToken");
        const headers = { Authorization: `Bearer ${token}` };

        return this.http.delete<Account>(
            `${this.baseUrl}/${account.ClientId}`,
            {
                headers,
            }
        );
    }

    deactivateAccount(account: AccountClientId): Observable<Account> {
        const token = localStorage.getItem("authToken");
        const headers = { Authorization: `Bearer ${token}` };

        return this.http.post<Account>(
            `${this.baseUrl}/deactivate/${account.ClientId}`,
            {},
            {
                headers,
            }
        );
    }
}
