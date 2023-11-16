using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree
{
    class BinaryExpressionTree
    {

        // Enum az matematikai operátorok reprezentálásához
        protected enum Operator
        {
            Add = 43, Sub = 45, Mul = 42, Div = 47, Pow = 94
        }

        // Absztrakt alaposztály a fa csomópontjainak
        abstract class Node
        {
            // Konstruktor az adat, bal és jobb tulajdonságok inicializálásához
            protected Node(char data, Node left, Node right)
            {
                Data = data;
                Left = left;
                Right = right;
            }

            // Konstruktor csak az adat tulajdonság beállításához, a bal és jobb értékek null-ok lesznek
            protected Node(char data) : this(data, null, null) { }
            public char Data { get; }
            public Node Left { get; }
            public Node Right { get; }
        }

        // Csomópont osztály operandusok reprezentálásához
        class OperandNode : Node
        {
            // Konstruktor az ősosztály adatával inicializálásához
            public OperandNode(char data) : base(data)
            {
            }
        }

        // Csomópont osztály operátorok reprezentálásához
        class OperatorNode : Node
        {
            // Csak olvasható tulajdonság az operátor típusához
            Operator Operator { get; }

            // Háromparaméteres konstruktor az ősosztály és az operátor típus beállításához
            public OperatorNode(char data, Node left, Node right) : base(data, left, right)
            {
                this.Operator = (Operator)System.Convert.ToInt32(data);
            }
        }

        // Csak olvasható tulajdonság a fa gyökéreleméhez
        Node Root { get; }

        // Privát konstruktor a fa gyökérének beállításához
        private BinaryExpressionTree(Node Root)
        {
            this.Root = Root;
        }

        // Publikus metódus a bináris kifejezési fa felépítéséhez egy karakterlánc alapján
        public static BinaryExpressionTree Build(string expression)
        {
            char[] tomb = expression.ToCharArray();
            return Build(tomb);
        }

        // Statikus metódus a bináris kifejezési fa felépítéséhez egy karaktertömb alapján (RPN algoritmus)
        static BinaryExpressionTree Build(char[] expression)
        {
            // Verem a csomópontok tárolásához a fa felépítése során
            Stack<Node> verem = new Stack<Node>();

            // Végigiterálás minden karakteren a kifejezésben
            for (int i = 0; i < expression.Length; i++)
            {
                // Konvertálja a karaktert ASCII értékké
                int h = System.Convert.ToInt32(expression[i]);

                // Ha a karakter szám, létrehoz egy OperandNode-ot és beteszi a verembe
                if (Char.IsDigit(expression[i]))
                {
                    verem.Push(new OperandNode(expression[i]));

                }

                // Ha az érték nem esik a megengedett tartományba vagy egy pont, kivételt dob
                else if ((h < 42 || h > 57) || h == 46)
                {
                    throw new InvalidExpressionException(new string(expression), i);
                }

                // Ha operátor, kiveszi a veremből a két csomópontot, létrehozza az OperatorNode-ot és beteszi a verembe
                else
                {
                    Node jobb = verem.Pop();
                    Node bal = verem.Pop();
                    OperatorNode temp = new OperatorNode(expression[i], bal, jobb);
                    verem.Push(temp);

                }
            }

            // A verem utolsó eleme a fa gyökerelem
            return new BinaryExpressionTree(verem.Pop());
        }

        // Override-olt ToString metódus a fa karakterláncá alakításához postorder bejárással
        public override string ToString()
        {

            if (Root is null)
            {
                return null;
            }
            else
            {
                return ToString(Root);
            }
        }

        // Privát metódus postorder bejárásra a ToString metódus számára
        private string ToString(Node node)
        {
            string rpn = "";
            _PostOrderBejaras(ref rpn, node);

            return rpn;
        }

        // Privát metódus postorder bejárásra
        private void _PostOrderBejaras(ref string rpn, Node p)
        {
            if (p != null)
            {
                char teszt = p.Data;
                _PostOrderBejaras(ref rpn, p.Left);
                _PostOrderBejaras(ref rpn, p.Right);
                rpn += p.Data;
            }
        }

        // Publikus metódus a fát zárójelezett formában átalakításhoz
        public string Convert()
        {
            if (Root is null)
            {
                return null;
            }
            else
            {
                return Convert(Root);
            }
        }

        // Privát metódus inorder bejárásra a Convert metódus számára
        private string Convert(Node Root)
        {
            string convertedrpm = "";

            _InOrderBejaras(ref convertedrpm, Root);

            return convertedrpm;
        }

        // Privát metódus inorder bejárásra
        private void _InOrderBejaras(ref string convertedrpm, Node p)
        {
            if (p != null)
            {
                if (p is OperatorNode)
                {
                    convertedrpm += '(';
                }
                _InOrderBejaras(ref convertedrpm, p.Left);

                convertedrpm += p.Data;
                _InOrderBejaras(ref convertedrpm, p.Right);
                if (p is OperatorNode)
                {
                    convertedrpm += ')';
                }

            }
        }

        // Publikus metódus az kifejezés eredményének kiszámolásához
        public double Evaluate()
        {
            if (Root == null)
            {
                return 0;
            }
            else
            {
                return Evaluate(Root);
            }
        }

        // Privát metódus postorder bejárásra az Evaluate metódus számára
        private double Evaluate(Node Root)
        {

            return _PostOrderBejarasModositott(Root);
        }

        // Privát metódus módosított postorder bejárásra
        private double _PostOrderBejarasModositott(Node p)
        {

            if (p is null)
            {
                return 0;
            }
            else if (p is OperandNode)
            {

                return System.Convert.ToDouble(System.Convert.ToString(p.Data));

            }
            else if (!(p is OperandNode))
            {
                switch ((Operator)p.Data)
                {
                    case Operator.Add:
                        return _PostOrderBejarasModositott(p.Left) + _PostOrderBejarasModositott(p.Right);
                    case Operator.Sub:
                        return _PostOrderBejarasModositott(p.Left) - _PostOrderBejarasModositott(p.Right);
                    case Operator.Mul:
                        return _PostOrderBejarasModositott(p.Left) * _PostOrderBejarasModositott(p.Right);
                    case Operator.Div:
                        return _PostOrderBejarasModositott(p.Left) / _PostOrderBejarasModositott(p.Right);
                    case Operator.Pow:
                        return Math.Pow(_PostOrderBejarasModositott(p.Left), _PostOrderBejarasModositott(p.Right));
                    default:
                        break;
                }
                return (double)p.Data;
            }
            else
            {
                return 'a';
            }
        }
    }
}
