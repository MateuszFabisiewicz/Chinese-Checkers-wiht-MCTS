using System;
using System.Collections.Generic;
using System.Drawing;
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


        public bool GenerateChildren () 
        {
            throw new NotImplementedException();
        }

        public void UpdateWinningProbability (PlayerColor color)
        {
            throw new NotImplementedException();
        }

        public Node (Node original)
        {
            //this = new Node(original.state, original.parent);
            this.state = original.state;
            this.parent = original.parent;
            this.winningProbability = original.winningProbability;
            this.visitCount = original.visitCount;
            this.move = original.move;
            foreach (var child in original.children)
            {
                this.children.Add(new Node(child));
            }
        }
    }
}
