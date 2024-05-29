import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { tap } from "rxjs/operators";

@Injectable({
    providedIn: "root",
})
export class AuthService {
    private baseUrl = "http://localhost:5004/api/v1/auth";

    constructor(private http: HttpClient) {}

    login(credentials: any): Observable<any> {
        return this.http.post<any>(`${this.baseUrl}/login`, credentials).pipe(
            tap((response) => {
                localStorage.setItem("authToken", response.token);
                localStorage.setItem("userType", response.type);
                localStorage.setItem("meuId", response.id);
            })
        );
    }

    logout(): void {
        localStorage.removeItem("authToken");
        localStorage.removeItem("userType");
        localStorage.removeItem("meuId");
    }
}
