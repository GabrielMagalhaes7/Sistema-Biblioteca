using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaTED;

public class Livro
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public int Ano { get; set; }
    public string Categoria { get; set; }
    public int QuantidadeTotal { get; set; }
    public int QuantidadeDisponivel { get; set; }

    public Livro(int id, string titulo, string autor, int ano, string categoria, int quantidade)
    {
        Id = id;
        Titulo = titulo;
        Autor = autor;
        Ano = ano;
        Categoria = categoria;
        QuantidadeTotal = quantidade;
        QuantidadeDisponivel = quantidade;
    }

    public override string ToString() =>
        $"ID: {Id} | {Titulo} | {Autor} | {Ano} | {Categoria} | Disponíveis: {QuantidadeDisponivel}/{QuantidadeTotal}";
}

public class Biblioteca
{
    
    private readonly List<Livro> livros = new();

    public Biblioteca()
    {
        CarregarDadosIniciais();
    }

    private void CarregarDadosIniciais()
    {
        livros.Add(new Livro(1, "Jantar Secreto", "Raphael Montes", 2016, "Romance", 8));
        livros.Add(new Livro(2, "O Hobbit", "J. R. R. Tolkien", 1937, "romance de fantasia épica", 6));
        livros.Add(new Livro(3, "O Senhor dos Anéis", "J. R. R. Tolkien", 1954, "romance de fantasia épica", 10));
        livros.Add(new Livro(4, "Game of Thrones", "George R. R. Martin", 1996, "romance de fantasia épica", 4));
        livros.Add(new Livro(5, "Biblioteca da Meia-Noite", "Matt Haig", 2020, "Romance", 7));
        livros.Add(new Livro(6, "Mais Esperto que o Diabo", "Napoleon Hill", 1938, "Autoajuda", 5));
    }

    public IReadOnlyList<Livro> Listar() => livros.AsReadOnly();

    public bool Cadastrar(Livro livro)
    {
        if (livro.Id <= 0 || string.IsNullOrWhiteSpace(livro.Titulo) ||
            string.IsNullOrWhiteSpace(livro.Autor) || livro.Ano < 0 ||
            livro.QuantidadeTotal < 1 || BuscarPorIdLinear(livro.Id) != null)
            return false;

        livros.Add(livro);
        return true;
    }

    public bool Remover(int id)
    {
        Livro? livro = BuscarPorIdLinear(id);
        return livro != null && livros.Remove(livro);
    }

    public Livro? BuscarPorIdLinear(int id)
    {
        
        foreach (Livro livro in livros)
            if (livro.Id == id)
                return livro;
        return null;
    }

    public Livro? BuscarPorIdBinaria(int id)
    {
       
        OrdenacaoSelectionSortPorId();
        int inicio = 0, fim = livros.Count - 1;

        while (inicio <= fim)
        {
            int meio = inicio + (fim - inicio) / 2;
            if (livros[meio].Id == id) return livros[meio];
            if (livros[meio].Id < id) inicio = meio + 1;
            else fim = meio - 1;
        }
        return null;
    }

    public List<Livro> BuscarPorTitulo(string termo)
    {
        return livros.Where(l => l.Titulo.Contains(termo, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public void OrdenacaoBubbleSortPorTitulo()
    {
        
        for (int i = 0; i < livros.Count - 1; i++)
        {
            bool houveTroca = false;
            for (int j = 0; j < livros.Count - 1 - i; j++)
            {
                if (string.Compare(livros[j].Titulo, livros[j + 1].Titulo,
                    StringComparison.OrdinalIgnoreCase) > 0)
                {
                    (livros[j], livros[j + 1]) = (livros[j + 1], livros[j]);
                    houveTroca = true;
                }
            }
            if (!houveTroca) break;
        }
    }

    public void OrdenacaoSelectionSortPorId()
    {
        
        for (int i = 0; i < livros.Count - 1; i++)
        {
            int menor = i;
            for (int j = i + 1; j < livros.Count; j++)
                if (livros[j].Id < livros[menor].Id)
                    menor = j;

            if (menor != i)
                (livros[i], livros[menor]) = (livros[menor], livros[i]);
        }
    }

    public Livro? QuickselectAno(int k)
    {
        
        if (k < 1 || k > livros.Count) return null;
        Livro[] copia = livros.ToArray();
        int indice = Quickselect(copia, 0, copia.Length - 1, k - 1);
        return copia[indice];
    }

    private static int Quickselect(Livro[] dados, int esquerda, int direita, int alvo)
    {
        while (esquerda <= direita)
        {
            int pivo = Particionar(dados, esquerda, direita);
            if (pivo == alvo) return pivo;
            if (pivo < alvo) esquerda = pivo + 1;
            else direita = pivo - 1;
        }
        return alvo;
    }

    private static int Particionar(Livro[] dados, int esquerda, int direita)
    {
        int pivoAno = dados[direita].Ano;
        int i = esquerda;
        for (int j = esquerda; j < direita; j++)
        {
            if (dados[j].Ano <= pivoAno)
            {
                (dados[i], dados[j]) = (dados[j], dados[i]);
                i++;
            }
        }
        (dados[i], dados[direita]) = (dados[direita], dados[i]);
        return i;
    }

    public List<Livro> SelecionarPorCategoria(string categoria)
    {
        
        return livros.Where(l => l.Categoria.Equals(categoria,
            StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public bool RegistrarEmprestimo(int id)
    {
        Livro? livro = BuscarPorIdLinear(id);
        if (livro == null || livro.QuantidadeDisponivel == 0) return false;
        livro.QuantidadeDisponivel--;
        return true;
    }

    public bool RegistrarDevolucao(int id)
    {
        Livro? livro = BuscarPorIdLinear(id);
        if (livro == null || livro.QuantidadeDisponivel >= livro.QuantidadeTotal) return false;
        livro.QuantidadeDisponivel++;
        return true;
    }
}

public static class Program
{
    private static readonly Biblioteca biblioteca = new();

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        bool executando = true;

        while (executando)
        {

            Console.WriteLine("----- THE MIDNIGHT LIBRARY -----");
            Console.WriteLine("1 - Listar livros");
            Console.WriteLine("2 - Cadastrar livro");
            Console.WriteLine("3 - Buscar livro por ID");
            Console.WriteLine("4 - Buscar por título");
            Console.WriteLine("5 - Ordenar livros");
            Console.WriteLine("6 - Encontrar livro pela posição (ano)");
            Console.WriteLine("7 - Buscar livros por categoria");
            Console.WriteLine("8 - Registrar empréstimo");
            Console.WriteLine("9 - Registrar devolução");
            Console.WriteLine("10 - Remover livro");
            Console.WriteLine("0 - Sair");
            Console.Write("Opção: ");

            switch (Console.ReadLine())
            {
                case "1": Exibir(biblioteca.Listar()); break;
                case "2": Cadastrar(); break;
                case "3": BuscarPorId(); break;
                case "4": BuscarTitulo(); break;
                case "5": OrdenarLivros(); break;
                case "6": SelecionarK(); break;
                case "7": SelecionarCategoria(); break;
                case "8": Emprestar(); break;
                case "9": Devolver(); break;
                case "10": Remover(); break;
                case "0": executando = false; break;
                default: Console.WriteLine("Opção inválida."); break;
            }
        }
        Console.WriteLine("Sistema encerrado.");
    }

    private static void Exibir(IEnumerable<Livro> lista)
    {
        var itens = lista.ToList();
        if (itens.Count == 0) { Console.WriteLine("Nenhum livro encontrado."); return; }
        foreach (Livro livro in itens) Console.WriteLine(livro);
    }

    private static void Cadastrar()
    {
        int id = LerInteiro("ID: ");
        Console.Write("Título: "); string titulo = Console.ReadLine() ?? "";
        Console.Write("Autor: "); string autor = Console.ReadLine() ?? "";
        int ano = LerInteiro("Ano: ");
        Console.Write("Categoria: "); string categoria = Console.ReadLine() ?? "";
        int qtd = LerInteiro("Quantidade: ");
        bool ok = biblioteca.Cadastrar(new Livro(id, titulo, autor, ano, categoria, qtd));
        Console.WriteLine(ok ? "Livro cadastrado." : "Cadastro inválido ou ID já existente.");
    }

    private static void BuscarPorId()
    {
        Console.WriteLine("\nComo deseja realizar a busca?");
        Console.WriteLine("1 - Busca linear");
        Console.WriteLine("2 - Busca binária");
        Console.Write("Opção: ");

        string? opcao = Console.ReadLine();
        int id = LerInteiro("ID para buscar: ");

        Livro? livro = opcao switch
        {
            "1" => biblioteca.BuscarPorIdLinear(id),
            "2" => biblioteca.BuscarPorIdBinaria(id),
            _ => null
        };

        if (opcao != "1" && opcao != "2")
            Console.WriteLine("Opção inválida.");
        else
            Console.WriteLine(livro?.ToString() ?? "Livro não encontrado.");
    }

    private static void OrdenarLivros()
    {
        Console.WriteLine("\nComo deseja ordenar os livros?");
        Console.WriteLine("1 - Por título (Bubble Sort)");
        Console.WriteLine("2 - Por ID (Selection Sort)");
        Console.Write("Opção: ");

        switch (Console.ReadLine())
        {
            case "1":
                biblioteca.OrdenacaoBubbleSortPorTitulo();
                Console.WriteLine("Livros ordenados por título.");
                Exibir(biblioteca.Listar());
                break;

            case "2":
                biblioteca.OrdenacaoSelectionSortPorId();
                Console.WriteLine("Livros ordenados por ID.");
                Exibir(biblioteca.Listar());
                break;

            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }

    private static void BuscarTitulo()
    {
        Console.Write("Parte do título: ");
        Exibir(biblioteca.BuscarPorTitulo(Console.ReadLine() ?? ""));
    }

    private static void SelecionarK()
    {
        int k = LerInteiro("Posição (1 = ano mais antigo): ");
        Livro? l = biblioteca.QuickselectAno(k);
        Console.WriteLine(l?.ToString() ?? "Posição inválida.");
    }

    private static void SelecionarCategoria()
    {
        Console.Write("Categoria: ");
        Exibir(biblioteca.SelecionarPorCategoria(Console.ReadLine() ?? ""));
    }

    private static void Emprestar()
    {
        bool ok = biblioteca.RegistrarEmprestimo(LerInteiro("ID do livro: "));
        Console.WriteLine(ok ? "Empréstimo registrado." : "Livro inexistente ou sem exemplares disponíveis.");
    }

    private static void Devolver()
    {
        bool ok = biblioteca.RegistrarDevolucao(LerInteiro("ID do livro: "));
        Console.WriteLine(ok ? "Devolução registrada." : "Livro inexistente ou todos os exemplares já estão na biblioteca.");
    }

    private static void Remover()
    {
        bool ok = biblioteca.Remover(LerInteiro("ID para remover: "));
        Console.WriteLine(ok ? "Livro removido." : "Livro não encontrado.");
    }

    private static int LerInteiro(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            if (int.TryParse(Console.ReadLine(), out int valor)) return valor;
            Console.WriteLine("Digite um número inteiro válido.");
        }
    }
}