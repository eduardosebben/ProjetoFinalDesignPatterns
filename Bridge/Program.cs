using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge_Builder
{
    //Bridge------------------------------
    // A Abstração define a interface para a parte "controle" dos dois
    // hierarquias de classe. Ele mantém uma referência a um objeto do
    // Hierarquia de implementação e delega todo o trabalho real para isso
    // objeto.
    class Abstraction
    {
        protected IImplementation _implementation;

        public Abstraction(IImplementation implementation)
        {
            this._implementation = implementation;
        }

        public virtual string Operation()
        {
            return "Abstract: Base operation with:\n" +
                _implementation.OperationImplementation();
        }
    }

    // A Implementação define a interface para todas as classes de implementação.
    // Não precisa combinar com a interface do Abstraction. Na verdade, os dois
    // as interfaces podem ser totalmente diferentes. Normalmente a Implementação
    // a interface fornece apenas operações primitivas, enquanto a abstração
    // define operações de nível superior com base nessas primitivas.
    public interface IImplementation
    {
        string OperationImplementation();
    }

    // Each Concrete Implementation corresponds to a specific platform and
    // implements the Implementation interface using that platform's API.
    class ConcreteImplementationA : IImplementation
    {
        public string OperationImplementation()
        {
            return "ConcreteImplementationA: O resultado na plataforma A.\n";
        }
    }
    class Client
    {
        // Exceto na fase de inicialização, onde um objeto Abstraction recebe
        // vinculado a um objeto de Implementação específico, o código do cliente deve
        // depende apenas da classe Abstraction. Desta forma, o código do cliente pode
        // suporta qualquer combinação de implementação de abstração.
        public void ClientCode(Abstraction abstraction)
        {
            Console.Write(abstraction.Operation());
        }
    }

    //Builder------------------------------
    // A interface Builder especifica métodos para criar as diferentes partes
    // dos objetos Produto.
    public interface IBuilder
    {
        void Build1();

        void Build2();
    }

    // As classes do Concrete Builder seguem a interface do Builder e fornecem
    // implementações específicas das etapas de construção. Seu programa pode ter
    // diversas variações de Builders, implementadas de forma diferente.
    public class ConcreteBuilder : IBuilder
    {
        private Produto _product = new Produto();

        // Uma nova instância do construtor deve conter um objeto de produto em branco, que
        // é usado em outra montagem.
        public ConcreteBuilder()
        {
            this.Reset();
        }

        public void Reset()
        {
            this._product = new Produto();
        }

        // Todas as etapas de produção funcionam com a mesma instância do produto.
        public void Build1()
        {
            this._product.Add("Build1");
        }

        public void Build2()
        {
            this._product.Add("Build2");
        }

        // Concrete Builders devem fornecer seus próprios métodos para
        // recuperando resultados. Isso porque vários tipos de construtores podem
        // criar produtos totalmente diferentes que não seguem o mesmo
        //interface. Portanto, tais métodos não podem ser declarados na base
        // Interface do construtor (pelo menos em uma programação estaticamente tipada
        // língua).
        //
        // Normalmente, após retornar o resultado final para o cliente, um construtor
        // espera-se que a instância esteja pronta para começar a produzir outro produto.
        // É por isso que é uma prática comum chamar o método reset no final
        // do corpo do método `GetProduct`. No entanto, esse comportamento não é
        // obrigatório, e você pode fazer seus construtores esperarem por um reset explícito
        // chamada do código do cliente antes de descartar o resultado anterior.
        public Produto GetProduto()
        {
            Produto result = this._product;

            this.Reset();

            return result;
        }
    }

    // Faz sentido usar o padrão Builder somente quando seus produtos são
    // bastante complexo e requer configuração extensa.
    //
    // Ao contrário de outros padrões de criação, diferentes construtores concretos podem
    // produzir produtos não relacionados. Em outras palavras, os resultados de vários construtores
    // pode não seguir sempre a mesma interface.
    public class Produto
    {
        private List<object> _parts = new List<object>();

        public void Add(string part)
        {
            this._parts.Add(part);
        }

        public string ListarPartes()
        {
            string str = string.Empty;

            for (int i = 0; i < this._parts.Count; i++)
            {
                str += this._parts[i] + ", ";
            }

            str = str.Remove(str.Length - 2); // removing last ",c"

            return "Partes do Produto: " + str + "\n";
        }
    }

    // O Diretor é responsável apenas por executar as etapas de construção em um
    // sequência particular. É útil ao produzir produtos de acordo com um
    // ordem ou configuração específica. Estritamente falando, a classe Diretor é
    // opcional, pois o cliente pode controlar os construtores diretamente.
    public class Director
    {
        private IBuilder _builder;

        public IBuilder Builder
        {
            set { _builder = value; }
        }

        // The Director can construct several product variations using the same
        // building steps.
        public void BuildMinimalViableProduct()
        {
            this._builder.Build1();
        }

        public void BuildFullFeaturedProduct()
        {
            this._builder.Build1();
            this._builder.Build2();
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            //Bridge
            Client client = new Client();

            Abstraction abstraction;
            // O código do cliente deve ser capaz de funcionar com qualquer pré-configurado
            // combinação abstração-implementação.
            abstraction = new Abstraction(new ConcreteImplementationA());
            client.ClientCode(abstraction);

            Console.WriteLine();

            //Builder
            // O código do cliente cria um objeto construtor, passa-o para o
            // diretor e então inicia o processo de construção. O fim
            // o resultado é recuperado do objeto construtor.
            var director = new Director();
            var builder = new ConcreteBuilder();
            director.Builder = builder;

            Console.WriteLine("Produto básico padrão:");
            director.BuildMinimalViableProduct();
            Console.WriteLine(builder.GetProduto().ListarPartes());

            Console.WriteLine("Produto padrão completo:");
            director.BuildFullFeaturedProduct();
            Console.WriteLine(builder.GetProduto().ListarPartes());

            // Lembre-se, o padrão Builder pode ser usado sem um Director
            // aula.
            Console.WriteLine("Customizar produto:");
            builder.Build1();
            Console.Write(builder.GetProduto().ListarPartes());
        }
    }
}
