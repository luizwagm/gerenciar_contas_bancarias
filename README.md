Para subir a aplicação rode

```
docker-compose up -d --build manager-dashboard
```

(o comando acima irá subir todos os serviços)

Obs.: certifique-se que as tabelas Persons, Transactions e Accounts foram criadas.

```
docker exec -it db mysql -u root -pYour_password123 -D MainDB -e "show tables;"
```

caso não, rode os comandos abaixo:

```
docker-compose run person-service dotnet ef migrations add InitialCreate // para a tabela Persons
docker-compose run transaction-service dotnet ef migrations add InitialCreate // para a tabela Transactions
docker-compose run account-service dotnet ef migrations add InitialCreate // para a tabela Accounts
```

Crie um primeiro usuário para usar o sistema usando o comando abaixo:

```
docker exec -it db mysql -u root -pYour_password123 -D MainDB -e "INSERT INTO Persons (FirstName, LastName, Email, DateOfBirth, Password, Role) VALUES ('User', 'Admin', 'admin@admin.com', '1991-10-12', '123456', 'manager');"
```

Acesse a url pelo navegador

```
http://localhost:4201/login

E-mail: admin@admin.com
Senha: 123456
```

Que eu me lembre, é isso :D.