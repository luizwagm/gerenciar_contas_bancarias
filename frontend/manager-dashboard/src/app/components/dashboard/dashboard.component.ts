import { Router } from '@angular/router';
import { Component } from "@angular/core";
import { AuthService } from 'src/app/services/auth.service';

@Component({
    selector: "app-dashboard",
    templateUrl: "./dashboard.component.html",
    styleUrls: ["./dashboard.component.css"],
})
export class DashboardComponent {
    constructor(
        private authService: AuthService,
        private router: Router
    ) {}

    deslogar() {
        console.log('aqui');
        this.authService.logout();
        this.router.navigate(["/login"]);
    }
}
