# gerenciadorpedidos

Aplicação de gerenciamento de pedidos com suporte a cadastro, listagem, status e cálculo de impostos. Desenvolvida em ASP.NET Core 8.0 e integração com banco SQL Server.
A aplicação foi desenvolvida utilizando padrões CQRS , SOLID e CLEAN CODE 
Também foi adotado uma estrutura de 3 camadas API, DOMAIN, DATA com conexão com SQL SERVER utilizando Entity Framework e com mapeamento através do Mapper. 
Foi adotado um sistema de Tenance Id (OID, SID, ID)  para melhor localização de problemas através de troubleshooting, mais foi preenchido automaticamente dentro da controller para simplicar o processo de testes, uma vez que essas informaões vem na origem da informação como API externa ou FRONT END. 

O banco de dados SQLSERVER e criado automaticamente através do EF, para preencher a conectionstring corretamente no arquivo de configuracao (appsettings), como segue exemplo ja configurado no projeto 

O sistema funciona inserindo as informações iniciais no banco de dados, foi criado um SERVICEBACGROUND para ficar lendo os pedidos que forem chegando na base que serão processados utilizando paralelismo para atender a demanda de pedidos conforme especificação de requisitos. O sistema utilizará uma metodologia de maquina de estado para ir controlando os pedidos que estão criados, pendentes e processados, logo depois da leitura serão enviado para uma URL de teste simulando uma api de terceiro. 
O sistema retorna a informação para o front que foi criado, aonde o msm poderá está checando em uma url para ver se o pedido que ele enviou ja foi processado. 
Essa metodologia agiliza pedidos grandes e evita o timeout de conexão uma vez que pedidos podem haver muitos outros processamentos a serem feitos internamente. 


TECNOLOGIAS UTILIZADAS
- .NET 8.0
- ASP.NET Core Web API
- Docker
- SQL Server
- Entity Framework Core
- Swagger / Swashbuckle
- xUnit, NSubstitute, FluentAssertions, Bogus
- AutoMapper
- Serilog

PRE-REQUISITOS
- .NET SDK 8.0 ou superior
- Visual Studio 2022+ ou VS Code
- SQLSERVER EXPRESSION

COMO EXECUTAR O PROJETO 
Clone o projeto na maquina através do repositório https://github.com/ronaldorobledo1902/gerenciadorpedidos/tree/develop

ESTRUTURA DO PROJETO 
├── GerenciadorPedidos.Api
├── GerenciadorPedidos.Domain
├── GerenciadorPedidos.Infra
├── GerenciadorPedidos.Tests
