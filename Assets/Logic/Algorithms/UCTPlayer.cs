using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeEditor;
using Unity.VisualScripting;

namespace Assets.Logic.Algorithms
{
    public class UCTPlayer : Player
    {
        int loopCount = 1000;
        double C = Math.Sqrt (2); // stała eksploracji

        public UCTPlayer (PlayerColor color) : base (color)
        {
            type = PlayerType.UCT;
        }

        public override (FieldInBoard, int) MakeChoice (Board board, Player opponentStats)
        {
            root = new Node (board, null);
            root.winningProbability = 0.5; // lub 0, jeszcze rozważyć

            bool childreanGenerated = root.GenerateChildren (); // tworzymy startowe drzewo - póki co tylko dzieci root, każde z szansą 0.5 wygranej,
                                                                // chyba że akurat jest wygrana

            int i = 0;
            while (i < loopCount) // ileś loopów
            {
                Node leaf = Traverse (root); // wybieramy liść do rozbudowy
                Expand (leaf);
                Node simulationResult = Rollout (leaf); // symulujemy rozgrywkę, zwracamy liść z wynikiem
                Backpropagate (leaf, simulationResult); // aktualizujemy wartości w drzewie
                
                i++;
            }

            Node bestChildNode = BestChild (root);

            return bestChildNode.move;
        }

        private Node BestChild (Node root)
        {
            double currentBest = 0;
            Node bestChild = null;

            foreach (Node child in root.children)
            {
                if (child.winningProbability > currentBest)
                {
                    currentBest = child.winningProbability;
                    bestChild = child;
                }
            }

            return bestChild;
        }

        private void Backpropagate (Node leaf, Node simulationResult) // leaf niepotrzebny?
        {
            //idziemy od simulationResult po parentach, aż dojdziemy do leaf
            simulationResult.UpdateWinningProbability (color);
            Node node = simulationResult.parent;

            while (node.parent != null)
            {
                node.UpdateWinningProbability (color);
                node = node.parent;
            }
        }

        private Node Rollout (Node leaf)
        {
            Node node = new Node(leaf);
            while (node.children.Count > 0) // póki możemy iść dalej
            {
                // stosujemy random rollout policy
                Random random = new Random ();
                int randomIndex = random.Next (0, node.children.Count);
                node = new (node.children[randomIndex]);
            }

            return node; // zwracamy końcowy node
        }

        private Node Traverse (Node root) // selection/exploration and exploitation
        {
            // choose node according to UCT policy
            Node node = root;

            while (node.children.Count != 0)
            {
                node = BestUCT (node);
            }

            return node;
        }

        private Node BestUCT (Node node)
        {
            double[] UCTValues = new double[node.children.Count];

            foreach (Node child in node.children)
            {
                UCTValues[child.move.checkerIndex] = child.winningProbability + C * Math.Sqrt (Math.Log (node.visitCount) / child.visitCount);
            }

            return node.children[UCTValues.ToList ().IndexOf (UCTValues.Max ())];
        }

        private void Expand (Node leaf)
        {
            bool expanded = leaf.GenerateChildren ();

            if (expanded) // powstały dzieci, możemy iść dalej
            {
                foreach (Node child in leaf.children)
                {
                    child.GenerateChildren ();
                }
            }
        }

        public void GenerateChildren (Board board)
        {
            throw new System.NotImplementedException ();
        }
    }
}
