using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

// Design Patterns Commands,FacthoryMethod,Iterator, Observer, Singleton
namespace ProjetoFinalDesignPatterns
{   
    //command----------------------------------
    public interface ICommand
    {
        void Execute();
    }
    class SimpleCommand : ICommand
    {
        private string _payload = string.Empty;

        public SimpleCommand(string payload)
        {
            this._payload = payload;
        }

        public void Execute()
        {
            Console.WriteLine($"SimpleCommand:Veja, eu posso fazer coisas simples como imprimir ({this._payload})");
        }
    }
    class ComplexCommand : ICommand
    {
        private Receiver _receiver;
        private string _a;

        private string _b;
        public ComplexCommand(Receiver receiver, string a, string b)
        {
            this._receiver = receiver;
            this._a = a;
            this._b = b;
        }
        public void Execute()
        {
            Console.WriteLine("ComplexCommand: coisas complexas devem ser feitas por um objeto receptor.");
            this._receiver.DoSomething(this._a);
            this._receiver.DoSomethingElse(this._b);
        }
    }
    class Receiver
    {
        public void DoSomething(string a)
        {
            Console.WriteLine($"Receiver: Working on ({a}.)");
        }

        public void DoSomethingElse(string b)
        {
            Console.WriteLine($"Receiver: Also working on ({b}.)");
        }
    }
    class Invoker
    {
        private ICommand _onStart;

        private ICommand _onFinish;

        // Inicializando commands.
        public void SetOnStart(ICommand command)
        {
            this._onStart = command;
        }

        public void SetOnFinish(ICommand command)
        {
            this._onFinish = command;
        }
        public void DoSomethingImportant()
        {
            Console.WriteLine("Invocador: Alguém quer que algo seja feito antes de eu começar?");
            if (this._onStart is ICommand)
            {
                this._onStart.Execute();
            }

            Console.WriteLine("Invocador: ...fazendo algo realmente importante...");

            Console.WriteLine("Invocador: Alguém quer que algo seja feito depois que eu terminar?");
            if (this._onFinish is ICommand)
            {
                this._onFinish.Execute();
            }
        }
    }
    //decorator-----------------------------
    public abstract class Component
    {
        public abstract string Operation();
    }
    class ConcreteComponent : Component
    {
        public override string Operation()
        {
            return "ConcreteComponent";
        }
    }

    abstract class Decorator : Component
    {
        protected Component _component;

        public Decorator(Component component)
        {
            this._component = component;
        }

        public void SetComponent(Component component)
        {
            this._component = component;
        }
        public override string Operation()
        {
            if (this._component != null)
            {
                return this._component.Operation();
            }
            else
            {
                return string.Empty;
            }
        }
    }
    class ConcreteDecoratorA : Decorator
    {
        public ConcreteDecoratorA(Component comp) : base(comp)
        {
        }
        public override string Operation()
        {
            return $"ConcreteDecoratorA({base.Operation()})";
        }
    }
    class ConcreteDecoratorB : Decorator
    {
        public ConcreteDecoratorB(Component comp) : base(comp)
        {
        }

        public override string Operation()
        {
            return $"ConcreteDecoratorB({base.Operation()})";
        }
    }

    public class Client
    {
        public void ClientCode(Component component)
        {
            Console.WriteLine("RESULT: " + component.Operation());
        }
    }

    //factoryMethod--------------------------------
    abstract class Creator
    {
        public abstract IProduct FactoryMethod();
        public string SomeOperation()
        {
            // Chame o método de fábrica para criar um objeto Product.
            var product = FactoryMethod();
            // Now, use the product.
            var result = "Creator: O código do mesmo criador acabou de trabalhar com"
                + product.Operation();

            return result;
        }
    }
    class ConcreteCreator1 : Creator
    {
        public override IProduct FactoryMethod()
        {
            return new ConcreteProduct1();
        }
    }

    class ConcreteCreator2 : Creator
    {
        public override IProduct FactoryMethod()
        {
            return new ConcreteProduct2();
        }
    }
    public interface IProduct
    {
        string Operation();
    }
    class ConcreteProduct1 : IProduct
    {
        public string Operation()
        {
            return "{Result of ConcreteProduct1}";
        }
    }

    class ConcreteProduct2 : IProduct
    {
        public string Operation()
        {
            return "{Result of ConcreteProduct2}";
        }
    }

    class Client2
    {
        public void Main()
        {
            Console.WriteLine("App: Lançado com o ConcreteCreator1.");
            ClientCode(new ConcreteCreator1());

            Console.WriteLine("");

            Console.WriteLine("App: Lançado com o ConcreteCreator2.");
            ClientCode(new ConcreteCreator2());
        }
        public void ClientCode(Creator creator)
        {
            // ...
            Console.WriteLine("Client: Desconheço a classe do criador," +
                "but it still works.\n" + creator.SomeOperation());
            // ...
        }
    }

    //iterator------------------
    abstract class Iterator : IEnumerator
    {
        object IEnumerator.Current => Current();
        public abstract int Key();
        public abstract object Current();
        public abstract bool MoveNext();
        public abstract void Reset();
    }

    abstract class IteratorAggregate : IEnumerable
    {
        public abstract IEnumerator GetEnumerator();
    }
    class AlphabeticalOrderIterator : Iterator
    {
        private WordsCollection _collection;
        private int _position = -1;

        private bool _reverse = false;

        public AlphabeticalOrderIterator(WordsCollection collection, bool reverse = false)
        {
            this._collection = collection;
            this._reverse = reverse;

            if (reverse)
            {
                this._position = collection.getItems().Count;
            }
        }

        public override object Current()
        {
            return this._collection.getItems()[_position];
        }

        public override int Key()
        {
            return this._position;
        }

        public override bool MoveNext()
        {
            int updatedPosition = this._position + (this._reverse ? -1 : 1);

            if (updatedPosition >= 0 && updatedPosition < this._collection.getItems().Count)
            {
                this._position = updatedPosition;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Reset()
        {
            this._position = this._reverse ? this._collection.getItems().Count - 1 : 0;
        }
    }
    class WordsCollection : IteratorAggregate
    {
        List<string> _collection = new List<string>();

        bool _direction = false;

        public void ReverseDirection()
        {
            _direction = !_direction;
        }

        public List<string> getItems()
        {
            return _collection;
        }

        public void AddItem(string item)
        {
            this._collection.Add(item);
        }

        public override IEnumerator GetEnumerator()
        {
            return new AlphabeticalOrderIterator(this, _direction);
        }
    }

    //observer------------------------
    public interface IObserver
    {
        void Update(ISubject subject);
    }

    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }
    public class Subject : ISubject
    {
        public int State { get; set; } = -0;
        private List<IObserver> _observers = new List<IObserver>();
        public void Attach(IObserver observer)
        {
            Console.WriteLine("Subject: Anexou um observador.");
            this._observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            this._observers.Remove(observer);
            Console.WriteLine("Subject: Destacou um observador.");
        }
        public void Notify()
        {
            Console.WriteLine("Subject: Observadores notificadores...");

            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }
        public void SomeBusinessLogic()
        {
            Console.WriteLine("\nSubject: estou fazendo algo importante.");
            this.State = new Random().Next(0, 10);

            Thread.Sleep(15);

            Console.WriteLine("Subject: Meu estado acabou de mudar para: " + this.State);
            this.Notify();
        }
    }
    class ConcreteObserverA : IObserver
    {
        public void Update(ISubject subject)
        {
            if ((subject as Subject).State < 3)
            {
                Console.WriteLine("ConcreteObserverA: reagiu ao evento.");
            }
        }
    }

    class ConcreteObserverB : IObserver
    {
        public void Update(ISubject subject)
        {
            if ((subject as Subject).State == 0 || (subject as Subject).State >= 2)
            {
                Console.WriteLine("ConcreteObserverB: reagiu ao evento.");
            }
        }
    }

    //Singleton---------------------------------
    public sealed class Singleton
    {
        private Singleton() { }
        private static Singleton _instance;
        public static Singleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Singleton();
            }
            return _instance;
        }
        public void someBusinessLogic()
        {
            // ...
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            // O código do cliente pode parametrizar um invocador com qualquer comando.
                        Invoker invoker = new Invoker();
            invoker.SetOnStart(new SimpleCommand("Say Hi!"));
            Receiver receiver = new Receiver();
            invoker.SetOnFinish(new ComplexCommand(receiver, "Send email", "Save report"));

            invoker.DoSomethingImportant();

            //decorator
            Client client = new Client();

            var simple = new ConcreteComponent();
            Console.WriteLine("Client: Eu recebo um componente simples:");
            client.ClientCode(simple);
            Console.WriteLine();

            // ...assim como os decorados.
            //
            // Observe como os decoradores podem agrupar não apenas componentes simples, mas o
            // outros decoradores também.
            ConcreteDecoratorA decorator1 = new ConcreteDecoratorA(simple);
            ConcreteDecoratorB decorator2 = new ConcreteDecoratorB(decorator1);
            Console.WriteLine("Client: Agora eu tenho um componente decorado:");
            client.ClientCode(decorator2);

            //FactoryMethod
            new Client2().Main();

            //iterator
            var collection = new WordsCollection();
            collection.AddItem("First");
            collection.AddItem("Second");
            collection.AddItem("Third");

            Console.WriteLine("Straight traversal:");

            foreach (var element in collection)
            {
                Console.WriteLine(element);
            }

            Console.WriteLine("\nReverse traversal:");

            collection.ReverseDirection();

            foreach (var element in collection)
            {
                Console.WriteLine(element);
            }

            //observer
            // The client code.
            var subject = new Subject();
            var observerA = new ConcreteObserverA();
            subject.Attach(observerA);

            var observerB = new ConcreteObserverB();
            subject.Attach(observerB);

            subject.SomeBusinessLogic();
            subject.SomeBusinessLogic();

            subject.Detach(observerB);

            subject.SomeBusinessLogic();

            //Singleton
            // The client code.
            Singleton s1 = Singleton.GetInstance();
            Singleton s2 = Singleton.GetInstance();

            if (s1 == s2)
            {
                Console.WriteLine("Singleton funciona, ambas as variáveis ​​contêm a mesma instância.");
            }
            else
            {
                Console.WriteLine("Singleton falhou, as variáveis ​​contêm instâncias diferentes.");
            }
        }

    }
}
