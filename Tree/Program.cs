using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tree;

namespace WZACPC_8
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //BinaryExpressionTree.Teszt1();
                BinaryExpressionTree binaryExpressionTree = BinaryExpressionTree.Build("23*4+5*");
                Console.WriteLine(binaryExpressionTree.ToString());
                Console.WriteLine(binaryExpressionTree.Convert());
                Console.WriteLine(binaryExpressionTree.Evaluate());
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }
    }
}
