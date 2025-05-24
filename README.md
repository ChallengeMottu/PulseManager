# 🚀 Solução Pulse - Onde a Eficiência encontra a Velocidade
 A Solução Pulse foi desenvolvida para atender às necessidades operacionais da empresa Mottu, focando na organização e automação dos processos nos pátios onde suas motos estão alocadas. 
Como as motos são o principal ativo da empresa, garantir seu controle, localização e rastreabilidade é essencial para a eficiência do negócio.

### 🔗 Como a Solução Pulse resolve os desafios operacionais da Mottu? 
Cada moto é equipada com um Beacon BLE (Bluetooth Low Energy), modelo nRF52810 da Nordic Semiconductor, que transmite um código único via sinal Bluetooth. Esse código é captado por uma interface web operada por colaboradores no pátio, permitindo:

- 🏍️ Identificação automática das motos;
- 🗺️ Confirmação de presença no pátio;
- 📊 Alocação inteligente de cada moto com base em seu status e nas zonas disponíveis;
- 📡Visualização em tempo real do pátio, com dashboard dinâmico, mapa visual e integração com câmeras de segurança.

Essa combinação de sensores físicos, visualização inteligente e backend robusto permite eficiência, rastreabilidade e controle operacional em tempo real.


---

## 🧩 Objetivo do Pulse Registration Manager API
Esta API RESTful desenvolvida em ASP.NET Core é responsável por gerenciar os dados de cadastro e autenticação dos colaboradores que utilizam o sistema Pulse.

### Funcionalidades principais:
- Cadastro de colaboradores com informações pessoais e endereço da Filial Mottu;
- Autenticação segura com senha criptografada utilizando BCrypt;
- Integração com banco de dados Oracle via Entity Framework Core;
- Arquitetura organizada em camadas (Controller, Service, Repository, DTO, Domain);
- Documentação via Swagger, facilitando testes e integração com o front-end.
  
Essa API é um componente essencial do ecossistema Pulse, garantindo que apenas usuários autorizados tenham acesso à operação do sistema e à gestão das motos.


### 🛠️ Tecnologias Utilizadas: 
- ASP.NET Core 8
- Oracle Entity Framework Core
- BCrypt.Net-Next
- AutoMapper
- EF Core Migrations
- Swagger (OpenAPI)
- Clean Architecture (Controller, Service, Repository, DTO, Domain)
- Data Annotations


### 📚 Endpoints 

### CadastroController

| Método | Endpoint             | Descrição                          |
|--------|----------------------|----------------------------------|
| GET    | `/api/cadastro`      | Lista todos os usuários cadastrados |
| GET    | `/api/cadastro/{id}` | Busca usuário cadastrado por ID    |
| POST   | `/api/cadastro`      | Cria cadastro do usuário            |
| PUT    | `/api/cadastro/{id}` | Atualiza dados de usuário cadastrado |
| DELETE | `/api/cadastro/{id}` | Deleta cadastro pelo ID             |


### LoginController

| Método | Endpoint                | Descrição                                  |
|--------|-------------------------|--------------------------------------------|
| POST   | `/api/login/autenticar` | Autentica o acesso do usuário               |
| GET    | `/api/login`            | Retorna todos os logins                      |
| GET    | `/api/login/{id}`       | Busca login do usuário pelo ID               |
| PUT    | `/api/login/{id}/senha` | Atualiza senha do usuário                     |
| DELETE | `/api/login/{id}`       | Deleta login pelo ID                          |



### ▶️ Instruções de execução:

1. Clone o Repositório
```bash
  git clone https://github.com/ChallengeMottu/PulseManagerMVP.git
  cd PulseManagerMVP
```

2. Configure a connection string do Oracle:
No arquivo appsettings.json, atualize a string de conexão:
```bash
  "ConnectionStrings": {
     "Oracle": "Data Source=[SEU_SERVIDOR];User ID=[SEU_ID_USER];Password=[PASSWORD];"
  }
```

3. Execute as migrações para criar as tabelas no banco:
```bash
  dotnet ef database update
```

4. Execute a aplicação:
```bash
  dotnet run
```

---

## 👥 Grupo Desenvolvedor 

- Gabriela de Sousa Reis - RM558830
- Laura Amadeu Soares - RM556690
- Raphael Lamaison Kim - RM557914 







