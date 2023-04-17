
# TechTest Angular + .NET 7

Este é um projeto para testar habilidades técnicas em Angular e .NET 7.

## Descrição

Este projeto consiste em uma aplicação web que permite o cálculo de juros bancários de boletos. O front-end foi desenvolvido em Angular e o back-end em .NET 7.

## Como usar

Para executar este projeto, é necessário ter o Docker instalado na máquina. Em seguida, siga as instruções abaixo:

1.  Clone o repositório em sua máquina;
2.  Abra um terminal e navegue até o diretório raiz do projeto;
3.  Execute o comando `docker-compose up --build` para criar e iniciar os containers do front-end e back-end;
4.  Aguarde até que todos os containers estejam em execução. Ao finalizar, você poderá acessar o front-end em [http://localhost:4200](http://localhost:4200/) e a API em [http://localhost:5000/swagger](http://localhost:5000/swagger).

## Acesso ao front-end

Para acessar o front-end, basta acessar a URL [http://localhost:4200](http://localhost:4200/) em seu navegador.

## Acesso à API (Swagger)

Para acessar a API, basta acessar a URL [http://localhost:5000/swagger](http://localhost:5000/swagger) em seu navegador. Lá você encontrará a documentação da API e poderá testar seus endpoints.

## Funcionalidade

A funcionalidade principal deste projeto é o cálculo de juros bancários de boletos. Para utilizá-la, basta informar o código de barras do boleto e a data de pagamento desejada. A API então retornará o valor do boleto com os juros calculados.
