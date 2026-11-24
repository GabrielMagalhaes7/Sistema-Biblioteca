# The Midnight Library

Sistema acadêmico de gerenciamento de biblioteca em C# e .NET 10, utilizando uma estrutura de dados linear (`List<Livro>`).

## Integrantes
- Gabriel Magalhães
- Gabriel Nunes

## Funcionalidades
Cadastro, listagem, remoção, busca por ID ou título, ordenação, seleção por ano ou categoria, empréstimo e devolução. Os dados permanecem em memória durante a execução.

## Algoritmos
| Tipo | Método | Complexidade |
|---|---|---|
| Busca | Linear por ID | O(n) |
| Busca | Binária por ID | O(log n) após ordenação; O(n²) total nesta implementação |
| Ordenação | Bubble Sort por título | O(n²) pior caso |
| Ordenação | Selection Sort por ID | O(n²) |
| Seleção | Quickselect por ano | O(n) médio, O(n²) pior caso |
| Seleção | Filtragem por categoria (LINQ) | O(n) |

## Como executar
Instale o SDK .NET 10. Na pasta que contém o arquivo `.csproj`, execute:

```bash
dotnet run
```

Também é possível abrir a solução no Visual Studio com suporte ao .NET 10.

## Testes
Com o acervo inicial, use as opções 3 (ID 2, ambos os métodos), 5 (ambas as ordenações), 6 (posição 1), 7 (Romance), 8 e 9 (ID 1). Resultados esperados e detalhes constam no PDF. Os testes ainda precisam ser executados e registrados em capturas reais.

## Documentação
[Relatório acadêmico](The_Midnight_Library.pdf).
