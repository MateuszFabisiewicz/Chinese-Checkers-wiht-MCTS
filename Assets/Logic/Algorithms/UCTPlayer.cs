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
            throw new System.NotImplementedException ();
        }

        public void GenerateChildren (Board board)
        {
            //throw new System.NotImplementedException ();

            root = new Node (board, null);
            root.winningProbability = 0.5; // lub 0, jeszcze rozważyć

            root.GenerateChildren ();
        }
    }
}
