## Server Side with microservice

# Docker Compose Setup

## 1. Run Docker Compose
Chạy lệnh sau trong thư mục **src** (nơi chứa file `docker-compose`):

```powershell
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans
```

---

## 2. Application Config URL

### Local Environment (Docker Container)
- **Product API**: [http://localhost:6002/api/products](http://localhost:6002/api/products)  
- **Customer API**: [http://localhost:6003/api/customers](http://localhost:6003/api/customers)  
- **Basket API**: [http://localhost:6004/api/baskets](http://localhost:6004/api/baskets)
- **Order API**:   [http://localhost:6004/api/baskets](http://localhost:6005/api/Orders)

### Developer Environment (Dev)
- **Product API**: [http://localhost:5002/api/products](http://localhost:5002/api/products)  
- **Customer API**: [http://localhost:5003/api/customers](http://localhost:5003/api/customers)  
- **Basket API**: [http://localhost:5004/api/baskets](http://localhost:5004/api/baskets)  
- **Order API**:   [http://localhost:6004/api/baskets](http://localhost:5005/api/Orders)
---

## 3. Docker Application URL - Local
- **Portainer**: [http://localhost:9000](http://localhost:9000)  
  - username: `admin`  
  - password: `admin12345678`  

- **Kibana**: [http://localhost:5601](http://localhost:5601)  
  - username: `elastic`  
  - password: `admin`  

- **RabbitMQ**: [http://localhost:15672](http://localhost:15672)  
  - username: `guest`  
  - password: `guest`  

---

## 4. Useful Commands

### .NET Core
```bash
dotnet restore
dotnet build
dotnet ef database update
```

### Migration Commands
```bash
dotnet ef migrations add "Init_DB"   --project {project dir}   --startup-project {startup project}   --output-dir {destination}
```

```bash
dotnet ef migrations add "init_migration"   --project Ordering.Infastructure   --startup-project Ordering.API   --output-dir Persistance/Migrations
```

```bash
dotnet ef database update   --project Ordering.Infastructure   --startup-project Ordering.API
```
