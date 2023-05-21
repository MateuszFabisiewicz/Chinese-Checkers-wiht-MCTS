using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Logic.Algorithms
{
    public class Node
    {
        public Node parent { get; private set; }

        public List<Node> children { get; private set; }

        public double winningProbability { get; set; } // X - win ratio

        public int visitCount { get; set; } // N

        public Board state { get; private set; }

        public (FieldInBoard newField, int checkerIndex) move { get; private set; }

        public Node (Board state, Node parentNode)
        {
            this.state = state;
            this.parent = parentNode;
            children = new List<Node>();
            visitCount = 0;
        }
        public Node (Board state, Node parentNode, int changedChecker, FieldInBoard fieldOfChecker)
        {
            this.state = state;
            this.parent = parentNode;
            children = new List<Node> ();
            visitCount = 0;
            move = (fieldOfChecker, changedChecker);
        }


        public void GenerateChildren () 
        {
            throw new NotImplementedException();
        }

        public void UpdateWinningProbability ()
        {
            throw new NotImplementedException();
        }
    }
}
