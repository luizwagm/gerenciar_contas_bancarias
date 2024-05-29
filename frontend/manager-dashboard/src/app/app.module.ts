import { CreateTransactionComponent } from './components/create-transaction/create-transaction.component';
import { AccountService } from './services/account.service';
import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { LoginComponent } from "./components/login/login.component";
import { DashboardComponent } from "./components/dashboard/dashboard.component";
import { ClientsComponent } from "./components/clients/clients.component";
import { TransactionsComponent } from "./components/transactions/transactions.component";
import { AccountManagementComponent } from "./components/account-management/account-management.component";
import { AuthService } from "./services/auth.service";
import { ClientService } from "./services/client.service";
import { TransactionService } from "./services/transaction.service";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { ButtonModule } from "primeng/button";
import { TableModule } from "primeng/table";
import { InputTextModule } from "primeng/inputtext";
import { PanelModule } from "primeng/panel";
import { DropdownModule } from "primeng/dropdown";
import { CalendarModule } from "primeng/calendar";
import { ToastModule } from "primeng/toast";
import { MessageService } from "primeng/api";
import { RouterModule } from "@angular/router";
import { AuthGuard } from "./guards/auth.guard";

import { CreateClientComponent } from "./components/create-client/create-client.component";
import { DialogModule } from "primeng/dialog";

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        DashboardComponent,
        ClientsComponent,
        TransactionsComponent,
        AccountManagementComponent,
        CreateClientComponent,
        CreateTransactionComponent,
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        RouterModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        BrowserAnimationsModule,
        ButtonModule,
        TableModule,
        InputTextModule,
        PanelModule,
        DropdownModule,
        CalendarModule,
        ToastModule,
        DialogModule,
        RouterModule.forRoot([]),
    ],
    providers: [
        AuthService,
        ClientService,
        TransactionService,
        MessageService,
        AuthGuard,
        AccountService
    ],
    bootstrap: [AppComponent],
})
export class AppModule {}
