using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy_Visitor_State_AbstractFactory_Adapter
{
    //proxy-------------------------
    // A interface Subject declara operações comuns para RealSubject e
    // o Proxy. Desde que o cliente trabalhe com RealSubject usando este
    // interface, você poderá passar um proxy em vez de um assunto real.
    public interface ISubject
    {
        void Request();
    }

    // O RealSubject contém alguma lógica de negócios principal. Normalmente, RealSubjects
    // são capazes de fazer algum trabalho útil que também pode ser muito lento ou
    // sensível - por exemplo corrigindo os dados de entrada. Um proxy pode resolver esses problemas
    // sem nenhuma alteração no código do RealSubject.
    class RealSubject : ISubject
    {
        public void Request()
        {
            Console.WriteLine("RealSubject: Handling Request.");
        }
    }

    // O Proxy possui uma interface idêntica ao RealSubject.
    class Proxy : ISubject
    {
        private RealSubject _realSubject;

        public Proxy(RealSubject realSubject)
        {
            this._realSubject = realSubject;
        }

        // As aplicações mais comuns do padrão Proxy são carregamento lento,
        // cache, controle de acesso, log, etc. Um Proxy pode executar
        // uma dessas coisas e depois, dependendo do resultado, passa o
        // execução para o mesmo método em um objeto RealSubject vinculado.
        public void Request()
        {
            if (this.CheckAccess())
            {
                this._realSubject.Request();

                this.LogAccess();
            }
        }

        public bool CheckAccess()
        {
            // Alguns cheques reais devem ir aqui.
            Console.WriteLine("Proxy: Checking access prior to firing a real request.");

            return true;
        }

        public void LogAccess()
        {
            Console.WriteLine("Proxy: Logging the time of request.");
        }
    }

    public class Client
    {

        // O código client deve funcionar com todos os objetos (ambos os assuntos
        // e proxies) através da interface Subject para suportar tanto
        // assuntos e proxies. Na vida real, no entanto, os clientes trabalham principalmente com
        // seus assuntos reais diretamente. Neste caso, para implementar o padrão
        // mais facilmente, você pode estender seu proxy da classe do sujeito real.
        public void ClientCode(ISubject subject)
        {
            // ...

            subject.Request();

            // ...
        }
        //Abstract Factory
        // O código do cliente funciona com fábricas e produtos apenas por meio abstrato
        // tipos: AbstractFactory e AbstractProduct. Isso permite que você passe qualquer
        // subclasse de fábrica ou produto para o código do cliente sem quebrá-lo.
        public void Main()
        {
            // The cliento código pode funcionar com qualquer classe de fábrica concreta.
            Console.WriteLine("Client: Testando o código do cliente com o primeiro tipo de fábrica...");
            ClientMethod(new ConcreteFactory1());
            Console.WriteLine();
        }

        public void ClientMethod(IAbstractFactory factory)
        {
            var productA = factory.CreateProdutoA();
        }
    }

    // visitor---------------------
    // A interface Component declara um método `accept` que deve levar o
    // basear a interface do visitante como um argumento.
    public interface IComponent
    {
        void Accept(IVisitor visitor);
    }

    // Cada Componente Concreto deve implementar o método `Accept` de tal forma
    // que chama o método do visitante correspondente ao componente
    // aula.
    public class ConcreteComponentA : IComponent
    {
        // Observe que estamos chamando `VisitConcreteComponentA`, que corresponde ao
        // nome da classe atual. Desta forma informamos ao visitante a classe do
        // componente com o qual trabalha.
        public void Accept(IVisitor visitor)
        {
            visitor.VisitConcreteComponentA(this);
        }

        // Componentes concretos podem ter métodos especiais que não existem em
        // sua classe base ou interface. O Visitante ainda pode usar esses
        // métodos desde que esteja ciente da classe concreta do componente.
        public string ExclusiveMethodOfConcreteComponentA()
        {
            return "A";
        }
    }

    // A interface do visitante declara um conjunto de métodos de visita que correspondem
    // para classes de componentes. A assinatura de um método de visita permite ao
    // visitante para identificar a classe exata do componente que está tratando
    // com.
    public interface IVisitor
    {
        void VisitConcreteComponentA(ConcreteComponentA element);

    }

    // Concrete Visitors implementam várias versões do mesmo algoritmo, que
    // pode trabalhar com todas as classes de componentes concretos.
    //
    // Você pode experimentar o maior benefício do padrão Visitor ao usar
    // com uma estrutura de objeto complexa, como uma árvore composta. Nisso
    // caso, pode ser útil armazenar algum estado intermediário do
    // algoritmo enquanto executa os métodos do visitante em vários objetos do
    // estrutura.
    class ConcreteVisitor1 : IVisitor
    {
        public void VisitConcreteComponentA(ConcreteComponentA element)
        {
            Console.WriteLine(element.ExclusiveMethodOfConcreteComponentA() + " + ConcreteVisitor1");
        }

    }

    class ConcreteVisitor2 : IVisitor
    {
        public void VisitConcreteComponentA(ConcreteComponentA element)
        {
            Console.WriteLine(element.ExclusiveMethodOfConcreteComponentA() + " + ConcreteVisitor2");
        }

    }

    public class Client2
    {
        // O código do cliente pode executar operações do visitante em qualquer conjunto de elementos
        // sem descobrir suas classes concretas. A operação de aceitação
        // direciona uma chamada para a operação apropriada no objeto visitante.
        public static void ClientCode(List<IComponent> components, IVisitor visitor)
        {
            foreach (var component in components)
            {
                component.Accept(visitor);
            }
        }
    }

    //state----------------------
    // O Contexto define a interface de interesse dos clientes. Isso também
    // mantém uma referência a uma instância de uma subclasse State, que
    // representa o estado atual do Context.
    class Context
    {
        // Uma referência ao estado atual do Contexto.
        private State _state = null;

        public Context(State state)
        {
            this.TransitionTo(state);
        }

        // O Context permite alterar o objeto State em tempo de execução.
        public void TransitionTo(State state)
        {
            Console.WriteLine($"Context: Transition to {state.GetType().Name}.");
            this._state = state;
            this._state.SetContext(this);
        }

        // O Context delega parte de seu comportamento ao State atual
        // objeto.
        public void Request1()
        {
            this._state.Handle1();
        }

        public void Request2()
        {
            this._state.Handle2();
        }
    }

    // A classe base State declara métodos que todo Concrete State deve
    // implementa e também fornece uma referência anterior ao objeto Context,
    // associado ao Estado. Esta referência anterior pode ser usada pelos Estados para
    // transiciona o Context para outro State.
    abstract class State
    {
        protected Context _context;

        public void SetContext(Context context)
        {
            this._context = context;
        }

        public abstract void Handle1();

        public abstract void Handle2();
    }

    // Estados concretos implementam vários comportamentos, associados a um estado de
    // o contexto.
    class ConcreteStateA : State
    {
        public override void Handle1()
        {
            Console.WriteLine("ConcreteStateA handles request1.");
            Console.WriteLine("ConcreteStateA quer mudar o estado do contexto.");
        }

        public override void Handle2()
        {
            Console.WriteLine("ConcreteStateA handles request2.");
        }
    }
    //abstract Factory-------------------------------
    // A interface Abstract Factory declara um conjunto de métodos que retornam
    // diferentes produtos abstratos. Esses produtos são chamados de família e são
    // relacionado por um tema ou conceito de alto nível. Os produtos de uma família são
    // geralmente capaz de colaborar entre si. Uma família de produtos pode
    // tem várias variantes, mas os produtos de uma variante são incompatíveis
    // com produtos de outro.
    public interface IAbstractFactory
    {
        IAbstractProductA CreateProdutoA();
    }

    // As Fábricas de Concreto produzem uma família de produtos pertencentes a um único
    // variante. A fábrica garante que os produtos resultantes são compatíveis.
    // Observe que as assinaturas dos métodos da Concrete Factory retornam um resumo
    // produto, enquanto dentro do método é instanciado um produto concreto.
    class ConcreteFactory1 : IAbstractFactory
    {
        public IAbstractProductA CreateProdutoA()
        {
            return new ConcreteProductA1();
        }
    }

    // Cada fábrica de concreto tem uma variante de produto correspondente.
    class ConcreteFactory2 : IAbstractFactory
    {
        public IAbstractProductA CreateProdutoA()
        {
            return new ConcreteProductA2();
        }
    }

    // Cada produto distinto de uma família de produtos deve ter uma interface base.
    // Todas as variantes do produto devem implementar esta interface.
    public interface IAbstractProductA
    {
        string FuncaoUtilA();
    }

    // Os Produtos de Concreto são criados pelas Fábricas de Concreto correspondentes.
    class ConcreteProductA1 : IAbstractProductA
    {
        public string FuncaoUtilA()
        {
            return "O resultado do produto A1.";
        }
    }

    class ConcreteProductA2 : IAbstractProductA
    {
        public string FuncaoUtilA()
        {
            return "O resultado do produto A2.";
        }
    }
    //adapter-------------------------
    // O Target define a interface específica do domínio usada pelo código do cliente.
    public interface ITarget
    {
        string GetRequest();
    }
    // O Adaptee contém algum comportamento útil, mas sua interface é
    // incompatível com o código cliente existente. O Adaptado precisa de alguns
    // adaptação antes que o código do cliente possa usá-lo.
    class Adaptee
    {
        public string GetSpecificRequest()
        {
            return " request específica.";
        }
    }

    // O Adaptador torna a interface do Adaptee compatível com a do Target
    // interface.
    class Adapter : ITarget
    {
        private readonly Adaptee _adaptee;

        public Adapter(Adaptee adaptee)
        {
            this._adaptee = adaptee;
        }

        public string GetRequest()
        {
            return $"This is '{this._adaptee.GetSpecificRequest()}'";
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            //proxy
            Client client = new Client();

            Console.WriteLine("Cliente: Executando o código do cliente com um sujeito real");
            RealSubject realSubject = new RealSubject();
            client.ClientCode(realSubject);

            Console.WriteLine();

            Console.WriteLine("Cliente: Executando o mesmo código de cliente com um proxy:");
            Proxy proxy = new Proxy(realSubject);
            client.ClientCode(proxy);

            //visitor
            List<IComponent> components = new List<IComponent>
            {
                new ConcreteComponentA(),
            };

            Console.WriteLine("O código do cliente funciona com todos os visitantes por meio da interface básica do visitante:");
            var visitor1 = new ConcreteVisitor1();
            Client2.ClientCode(components, visitor1);

            Console.WriteLine();

            Console.WriteLine("\r\nPermite que o mesmo código de cliente funcione com diferentes tipos de visitantes:");
            var visitor2 = new ConcreteVisitor2();
            Client2.ClientCode(components, visitor2);

            //state
            // The client code.
            var context = new Context(new ConcreteStateA());
            context.Request1();
            context.Request2();

            //Abstract Factory
            new Client().Main();

            //adapter
            Adaptee adaptee = new Adaptee();
            ITarget target = new Adapter(adaptee);

            Console.WriteLine("A interface adaptada é incompatível com o cliente.");
            Console.WriteLine("Mas com o adaptador, o cliente pode chamar seu método.");

            Console.WriteLine(target.GetRequest());
        }
    }
}
