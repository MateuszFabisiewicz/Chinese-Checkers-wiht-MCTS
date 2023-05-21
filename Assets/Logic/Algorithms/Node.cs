using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Logic.Algorithms
{
    public class Node
    {
        public Node parent { get; set; }

        public List<Node> children { get; set; }

        public double winningProbability { get; set; }

        public Board state { get; set; }

        public Node (Board state, Node parentNode)
        {
            this.state = state;
            this.parent = parentNode;
            children = new List<Node>();
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
