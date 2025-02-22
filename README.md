
# CustomerManagerWeb

Este projeto Web foi desenvolvido para gerenciar clientes e seus respectivos endereços, com funcionalidades de criação, edição, visualização e exclusão de registros. Ele foi construído utilizando ASP.NET Core MVC e oferece uma interface de usuário para a gestão de clientes e endereços. Além disso, ele oferece suporte para upload de imagens, como o logotipo da empresa associada a um cliente.

## Funcionalidades

- **Gestão de Clientes**:
  - Visualizar lista de clientes.
  - Visualizar detalhes de um cliente.
  - Criar um novo cliente.
  - Editar informações de um cliente existente.
  - Excluir um cliente.
  - Filtro de clientes por nome ou email.

- **Gestão de Endereços**:
  - Criar um endereço para um cliente.
  - Editar um endereço existente.
  - Excluir um endereço.

- **Upload de Imagem**:
  - Os clientes podem associar um logotipo da empresa ao seu cadastro através de upload de imagem.

## Estrutura do Projeto

O projeto é composto por controllers, serviços, modelos e views, de acordo com o padrão MVC. Abaixo está a estrutura básica do controlador `CustomerController`, que gerencia as funcionalidades de clientes e endereços:

### CustomerController

- **Métodos para Clientes**:
  - `Index`: Exibe a lista de clientes com a opção de filtro.
  - `Details`: Exibe os detalhes de um cliente específico.
  - `Create`: Cria um novo cliente.
  - `Edit`: Edita um cliente existente.
  - `Delete`: Exclui um cliente.

- **Métodos para Endereços**:
  - `CreateAddress`: Cria um novo endereço para um cliente.
  - `DetailsAddress`: Exibe os detalhes de um endereço.
  - `EditAddress`: Edita um endereço existente.
  - `DeleteAddress`: Exclui um endereço associado a um cliente.

## Instalação

1. **Clone o repositório**:
   ```bash
   git clone https://github.com/elitonsantana18/CustomerManagerWeb.git
   ```

2. **Restaurar dependências**:
   Abra o projeto no Visual Studio ou em sua IDE de preferência e execute o comando para restaurar as dependências do projeto:
   ```bash
   dotnet restore
   ```

3. **Rodar o projeto**:
   Execute o projeto com o comando:
   ```bash
   dotnet run
   ```
   
## Contribuições

Se desejar contribuir para este projeto, siga as etapas abaixo:

1. Fork o repositório.
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`).
3. Faça o commit das suas alterações (`git commit -am 'Add nova feature'`).
4. Envie a branch para o repositório remoto (`git push origin feature/nova-feature`).
5. Abra um Pull Request.
