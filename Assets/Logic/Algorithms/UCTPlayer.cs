using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Logic.Algorithms
{
    public class UCTPlayer : Player
    {
        public UCTPlayer (PlayerColor color) : base (color)
        {
            type = PlayerType.UCT;
        }

        public override (FieldInBoard, int) MakeChoice (Board board, Player opponentStats)
        {
            //throw new System.NotImplementedException ();

            root = new Node (board, null);
            root.winningProbability = 0.5; // lub 0, jeszcze rozważyć

            root.GenerateChildren (); // tworzymy startowe drzewo - póki co tylko dzieci root, każde z szansą 0.5 wygranej,
                                      // chyba że akurat jest wygrana

            while (true) // ileś loopów
            {
                Node leaf = Traverse (root); // wybieramy liść do rozbudowy
                Node simulationResult = Rollout (leaf); // symulujemy rozgrywkę, zwracamy liść z wynikiem
                Backpropagate (leaf, simulationResult); // aktualizujemy wartości w drzewie
            }
        }

        private void Backpropagate (Node leaf, Node simulationResult)
        {
            throw new NotImplementedException ();
        }

        private Node Rollout (Node leaf)
        {
            throw new NotImplementedException ();
        }

        private Node Traverse (Node root)
        {
            throw new NotImplementedException ();
        }

        public void GenerateChildren (Board board)
        {
            //throw new System.NotImplementedException ();
        }
    }
}
