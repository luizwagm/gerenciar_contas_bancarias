import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AuthService } from "../../services/auth.service";

@Component({
    selector: "app-login",
    templateUrl: "./login.component.html",
    styleUrls: ["./login.component.css"],
})
export class LoginComponent {
    loginForm: FormGroup;

    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private router: Router
    ) {
        this.loginForm = this.fb.group({
            email: ["", Validators.required],
            password: ["", Validators.required],
        });
    }

    onLogin() {
        if (this.loginForm.valid) {
            this.authService.login(this.loginForm.value).subscribe(
                (response) => {
                    const token = response?.token;
                    const type = response?.type;
                    const meuid = response?.id;
                    if (token) {
                        localStorage.setItem("authToken", token);
                        localStorage.setItem("userType", type);
                        localStorage.setItem("meuId", meuid);
                        this.router.navigate(["/dashboard"]);
                    }
                },
                (error) => {
                    console.error("Login failed", error);
                }
            );
        }
    }
}
