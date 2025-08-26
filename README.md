## ASPnetcore microservice

```
Go to the folder contain file `docker-compose` (folder src)
1. Run docker compose
	``` powershell
	docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans
	```
## Application config URL - Local environment (for docker container):
- Produt API: http://localhost:6002/api/products
- Customer API: http://localhost:6003/api/customers
- Basket API: http://localhost:6004/api/baskets

## Application config URL - Developer environment (for dev):
- Produt API: http://localhost:5002/api/products
- Customer API: http://localhost:5003/api/customers
- Basket API: http://localhost:5004/api/baskets

## Docker Application URL - Local 
- Portainer: localhost:9000 - username: admin ; password admin12345678
- kibana : localhost: 5601 - username: elastic ; pass: admin
- rabbitMQ: localhost: 15672 - username: guest; pass: guest


##Useful Commands:
- dotnet ef database update
- dotnet restore
- dotnet build
- Migration commands:
	- dotnet ef migrations add "Init_DB" --project {project dir} --startup-project --output-dir {destination}
	- dotnet ef migrations add "Order_DocumentNo" --project Ordering.Infastructure --startup-project Ordering.API --output-dir Persistance/Migrations
- Update Database: dotnet ef database update --project Ordering.Infastructure --startup-project Ordering.API
``` 