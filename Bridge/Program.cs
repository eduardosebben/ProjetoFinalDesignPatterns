using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge_Builder_Prototype_Composite
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

    //prototype-----------------------
    public class Person
    {
        public int Idade;
        public DateTime Aniversario;
        public string Nome;
        public IdInfo IdInfo;

        public Person ShallowCopy()
        {
            return (Person)this.MemberwiseClone();
        }

        public Person DeepCopy()
        {
            Person clone = (Person)this.MemberwiseClone();
            clone.IdInfo = new IdInfo(IdInfo.IdNumber);
            clone.Nome = String.Copy(Nome);
            return clone;
        }
    }

    public class IdInfo
    {
        public int IdNumber;

        public IdInfo(int idNumber)
        {
            this.IdNumber = idNumber;
        }
    }

    //Compiste --------------------------------------------

    // A classe base Component declara operações comuns para ambos simples e
    // objetos complexos de uma composição.
    abstract class Component
    {
        public Component() { }

        // O componente base pode implementar algum comportamento padrão ou deixá-lo para
        // classes concretas (declarando o método que contém o comportamento como
        // "resumo").
        public abstract string Operation();

        // Em alguns casos, seria benéfico definir o controle de filhos
        // operações diretamente na classe Component base. Desta forma, você não vai
        // precisa expor quaisquer classes de componentes concretos ao código do cliente,
        // mesmo durante a montagem da árvore de objetos. A desvantagem é que esses
        // os métodos estarão vazios para os componentes de nível folha.
        public virtual void Add(Component component)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(Component component)
        {
            throw new NotImplementedException();
        }

        // Você pode fornecer um método que permite que o código do cliente descubra se
        // um componente pode ter filhos.
        public virtual bool IsComposite()
        {
            return true;
        }
    }

    // A classe Leaf representa os objetos finais de uma composição. Uma folha não pode
    // tem filhos.
    //
    // Normalmente, são os objetos Leaf que fazem o trabalho real, enquanto Composite
    // os objetos apenas delegam para seus subcomponentes.
    class Leaf : Component
    {
        public override string Operation()
        {
            return "Leaf";
        }

        public override bool IsComposite()
        {
            return false;
        }
    }

    // A classe Composite representa os componentes complexos que podem ter
    // crianças. Normalmente, os objetos Composite delegam o trabalho real para
    // seus filhos e depois "soma" o resultado.
    class Composite : Component
    {
        protected List<Component> _children = new List<Component>();

        public override void Add(Component component)
        {
            this._children.Add(component);
        }

        public override void Remove(Component component)
        {
            this._children.Remove(component);
        }

        // O Composite executa sua lógica primária de uma maneira particular. Isto
        // percorre recursivamente todos os seus filhos, coletando e
        // somando seus resultados. Como os filhos do composto passam por esses
        // chama seus filhos e assim por diante, toda a árvore de objetos é
        // percorrido como resultado.
        public override string Operation()
        {
            int i = 0;
            string result = "Branch(";

            foreach (Component component in this._children)
            {
                result += component.Operation();
                if (i != this._children.Count - 1)
                {
                    result += "+";
                }
                i++;
            }

            return result + ")";
        }
    }

    class Client2
    {
        // O código do cliente funciona com todos os componentes via base
        //interface.
        public void ClientCode(Component leaf)
        {
            Console.WriteLine($"RESULT: {leaf.Operation()}\n");
        }

        // Graças ao fato de que as operações de gerenciamento de filhos são declaradas
        // na classe Component base, o código do cliente pode funcionar com qualquer
        // componente, simples ou complexo, sem depender do seu concreto
        // Aulas.
        public void ClientCode2(Component component1, Component component2)
        {
            if (component1.IsComposite())
            {
                component1.Add(component2);
            }

            Console.WriteLine($"RESULT: {component1.Operation()}");
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

            //prototype
            Person p1 = new Person();
            p1.Idade = 42;
            p1.Aniversario = Convert.ToDateTime("1977-01-01");
            p1.Nome = "Jack Daniels";
            p1.IdInfo = new IdInfo(666);

            // Execute uma cópia rasa de p1 e atribua-a a p2.
            Person p2 = p1.ShallowCopy();
            // Faça uma cópia profunda de p1 e atribua-a a p3.
            Person p3 = p1.DeepCopy();

            // Display values of p1, p2 and p3.
            Console.WriteLine("Original values of p1, p2, p3:");
            Console.WriteLine("   p1 valores de instância : ");
            DisplayValues(p1);
            Console.WriteLine("   p2 valores de instância:");
            DisplayValues(p2);
            Console.WriteLine("   p3 valores de instância:");
            DisplayValues(p3);

            // Altere o valor das propriedades p1 e exiba os valores de p1,
            // p2 and p3.
            p1.Idade = 32;
            p1.Aniversario = Convert.ToDateTime("1900-01-01");
            p1.Nome = "Frank";
            p1.IdInfo.IdNumber = 7878;
            Console.WriteLine("\nValores de p1, p2 e p3 após mudanças para p1:");
            Console.WriteLine("   p1 valores de instância: ");
            DisplayValues(p1);
            Console.WriteLine("   p2 valores de instância (os valores de referência mudaram):");
            DisplayValues(p2);
            Console.WriteLine("   p3 valores de instância (tudo foi mantido o mesmo):");
            DisplayValues(p3);

            //Composite
            Client2 client2 = new Client2();

            // Desta forma, o código do cliente pode suportar a folha simples
            // components...
            Leaf leaf = new Leaf();
            Console.WriteLine("Client: Eu recebo um componente simples:");
            client2.ClientCode(leaf);

            // ...bem como os compostos complexos.
            Composite tree = new Composite();
            Composite branch1 = new Composite();
            branch1.Add(new Leaf());
            branch1.Add(new Leaf());
            Composite branch2 = new Composite();
            branch2.Add(new Leaf());
            tree.Add(branch1);
            tree.Add(branch2);
            Console.WriteLine("Client: Agora eu tenho uma árvore composta:");
            client2.ClientCode(tree);

            Console.Write("Client: Não preciso verificar as classes de componentes mesmo ao gerenciar a árvore:\n");
            client2.ClientCode2(tree, leaf);
        }
        //prototype
        public static void DisplayValues(Person p)
        {
            Console.WriteLine("      Nome: {0:s}, Idade: {1:d}, Aniversário: {2:MM/dd/yy}",
                p.Nome, p.Idade, p.Aniversario);
            Console.WriteLine("      ID#: {0:d}", p.IdInfo.IdNumber);
        }
    }
}
