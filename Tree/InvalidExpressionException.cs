using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree
{
    // A Bináris Kifejezésfa építési folyamatában keletkező kivételeket kezelő osztály
    class InvalidExpressionException : Exception
    {
        // Konstruktor, ami megkapja a hibás pozíciót és karaktert, majd összeállít egy üzenetet
        public InvalidExpressionException(string position, int expression) : base($"Invalid character found at position: {position}, in the following: '{expression}'!")
        {

        }

        // Felüldefiniált ToString metódus, ami az üzenetet adja vissza
        public override string ToString()
        {
            return $"InvalidExpressionException: {Message}";
        }
    }
}
