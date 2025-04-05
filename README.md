# Security JWT para APIs .NET 8

Este projeto é uma prova de conceito (POC) que demonstra a implementação de JWT (JSON Web Tokens) em um sistema financeiro composto por duas APIs em .NET 8.

## Visão Geral do Projeto

O projeto consiste em duas APIs:

1. **TransactionAPI** - API responsável por iniciar transações financeiras
2. **PaymentAPI** - API responsável por processar pagamentos

A comunicação entre as APIs é protegida usando JWT, onde a TransactionAPI gera um token JWT para autenticar-se na PaymentAPI.

### Diagrama de Arquitetura

O diagrama abaixo ilustra o fluxo de autenticação JWT entre as duas APIs:

![Diagrama de Fluxo JWT](images/jwt-flow-diagram.png)

## Tecnologias Utilizadas

- .NET 8
- ASP.NET Core Web API
- JWT (JSON Web Tokens)
- Autenticação e Autorização baseada em roles
- HttpClient para comunicação entre APIs

## Estrutura do Projeto

- **TransactionAPI**
  - Endpoint para criação de transações
  - Geração de tokens JWT para acessar a PaymentAPI
  - Autorização baseada em role "transaction_initiator"

- **PaymentAPI**
  - Endpoint para processamento de pagamentos
  - Validação de tokens JWT
  - Autorização baseada em role "payment_processor"

## Pré-requisitos

- .NET 8 SDK
- Visual Studio 2022 ou VS Code
- Postman (para testes)

## Como Executar o Projeto

### Configurando o ambiente

1. Clone o repositório:
   ```
   git clone https://github.com/SpinxStudy/security-jwt-dotnet.git
   cd security-jwt-dotnet
   ```

2. Restaure as dependências e compile as APIs:

   **Para a PaymentAPI:**
   ```
   cd PaymentAPI
   dotnet restore
   dotnet build
   ```

   **Para a TransactionAPI:**
   ```
   cd TransactionAPI
   dotnet restore
   dotnet build
   ```

### Executando as APIs

É importante executar ambas as APIs simultaneamente, pois a TransactionAPI depende da PaymentAPI.

**PaymentAPI** - Execute em um terminal:
```
cd PaymentAPI/PaymentAPI
dotnet run
```
A API estará disponível em: http://localhost:5000

**TransactionAPI** - Execute em outro terminal:
```
cd TransactionAPI/TransactionAPI
dotnet run
```
A API estará disponível em: http://localhost:5001

## Testando com Postman

### 1. Configurando o Postman

1. Abra o Postman e crie uma nova coleção chamada "JWT Financial System"
2. Adicione uma nova requisição POST para criar uma transação conforme instruções abaixo

### 2. Testando a API de Transações

Para testar o fluxo completo, você precisa criar uma transação na TransactionAPI atraves de uma requisição tambem autenticada com JWT, que por sua vez irá se comunicar com a PaymentAPI:

**Requisição POST para criar uma transação:**
- Método: `POST`
- URL: `http://localhost:5001/api/transaction/create`
- Headers:
  - Content-Type: `application/json`
- Body (raw JSON):
  ```json
  {
    "amount": 100.50,
    "sourceAccountId": "ACC123456",
    "destinationAccountId": "ACC789012"
  }
  ```
- Authorization:
    - Auth Type: JWT Bearer
    - Algorithm: HS256
    - Secret: 3ss4-s3r4-4-m1nh4-s3cr3t-k3y-t0-us3-w1th-th1s-4pp
    - Payload (JSON):
        ```
        {
            "sub": "usertest",
            "iss": "financial-system",
            "aud": "transaction-api",
            "role": "transaction_initiator",
            "exp": 1744060800 
        }
        ```
    - Request header prefix: Bearer

![Visao Completa do Postman](images/complete-test-postman.png)


A TransactionAPI gerará automaticamente um token JWT para autenticar-se na PaymentAPI.

### 3. Exemplos de Resposta

Se tudo ocorrer deve ser gerada uma saida similar:

```json
{
  "message": "Transação criada com sucesso",
  "paymentDetails": {
    "transactionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "status": "Processed",
    "amount": 100.50,
    "sourceAccountId": "ACC123456",
    "destinationAccountId": "ACC789012",
    "processedAt": "2023-09-20T15:32:05.123Z"
  }
}
```

## Fluxo de Autenticação JWT

1. O Postman gera uma token para se autenticar com a TransactionAPI 
2. A TransactionAPI valida a token e request enviada pelo Postman para prosseguir
3. A TransactionAPI gera um token JWT com claims específicas
4. O token JWT é enviado no cabeçalho de autorização para a PaymentAPI
5. A PaymentAPI valida o token JWT e verifica as claims
6. Se válido, a PaymentAPI processa a requisição e retorna o resultado

## Segurança e Boas Práticas

- A chave de assinatura do JWT está codificada diretamente apenas para fins de demonstração
- Em ambiente de produção seria ideal avaliar:
  - AWS Secrets Manager ou similar para armazenar chaves
  - Períodos de expiração mais curtos para os tokens
  - HTTPS para todas as comunicações
 
## Melhorias Futuras
- Criar um servico de gerenciamento de tokens / autenticacoes
