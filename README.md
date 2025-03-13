# TechOrder 📦🚀  
A scalable **order management system** built with **.NET Core** that allows for seamless reseller order processing.

## 🛠️ Tech Stack  
- **.NET 8**, **ASP.NET Core**  
- **Entity Framework Core** (PostgreSQL)  
- **FluentValidation** for request validation  
- **MediatR** for CQRS pattern  
- **IHttpClientFactory** for external API communication  
- **Docker** for containerized deployment  

## 🌟 Features  
✅ **CNPJ validation** for reseller registration  
✅ **Efficient order creation and management**  
✅ **Integration with external supplier APIs**  
✅ **Domain-driven design (DDD) principles** for maintainable architecture  

## 🏗️ Project Structure  
This solution consists of **three microservices**, with synchronous communication between them:

### **1. Customer Service**
This API is responsible for handling customer orders for resellers. The main functionalities include:
- Identifying the customer placing the order
- Accepting a list of products with specified quantities
- No minimum order requirement for resellers
- Returning an order ID and the list of requested items

### **2. Reseller Service**
This API is responsible for registering resellers with the following data:
- **CNPJ** (mandatory, must be valid)
- **Corporate Name** (mandatory, must be valid)
- **Trade Name** (mandatory, must be valid)
- **Email** (mandatory, must be valid)
- **Phone Numbers** (optional, multiple allowed, must be valid)
- **Contact Names** (mandatory, at least one primary contact required)
- **Delivery Addresses** (mandatory, multiple addresses allowed)

### **3. Supplier Service**
This service is responsible for placing reseller orders with the **SUPPLIER** API. Key considerations:
- The **SUPPLIER** only accepts orders from registered **RESELLERS**
- Minimum order quantity is **1,000 units** (sum of all product quantities)
- The service handles API instability (retry logic, fallback mechanisms, etc.)
- The contract and response handling are customizable
- The actual **SUPPLIER API** is mocked, no need for real implementation

## 🚀 Getting Started  
### Prerequisites  
Ensure you have the following installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/)
- Entitity framework in memory (or use Docker for local development)

### Running the Project  
```sh
# Clone the repository
git clone https://github.com/matheuandrade/TechOrder.git
cd TechOrder

# Run the services using Docker Compose
docker-compose up -d
```

## 🤝 Contributing  
Contributions are welcome! Please follow these steps:
1. Fork the repository
2. Create a new branch (`feature/your-feature-name`)
3. Commit your changes
4. Push to your fork and submit a PR

## 📜 License  
This project is licensed under the **MIT License**.