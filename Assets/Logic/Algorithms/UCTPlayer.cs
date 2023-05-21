using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            root.GenerateChildren (); // tworzymy startowe drzewo - póki co tylko dzieci root, każde z szansą 0.5 wygranej,
                                      // chyba że akurat jest wygrana

            int i = 0;
            while (i < loopCount) // ileś loopów
            {
                Node leaf = Traverse (root); // wybieramy liść do rozbudowy
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

        private void Backpropagate (Node leaf, Node simulationResult)
        {
            throw new NotImplementedException ();
        }

        private Node Rollout (Node leaf)
        {
            throw new NotImplementedException ();
        }

        private Node Traverse (Node root) // selection/exploration and exploitation
        {
            // choose node according to UCT policy
            double[] UCTValues = new double[root.children.Count];

            foreach (Node child in root.children)
            {
                UCTValues[child.move.checkerIndex] = child.winningProbability + C * Math.Sqrt (Math.Log (root.visitCount) / child.visitCount);
            }

            return root.children[UCTValues.ToList ().IndexOf (UCTValues.Max ())];
        }

        public void GenerateChildren (Board board)
        {
            //throw new System.NotImplementedException ();
        }
    }
}
