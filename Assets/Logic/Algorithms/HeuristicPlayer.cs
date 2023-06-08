using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;

namespace Assets.Logic.Algorithms
{
    public class HeuristicPlayer : Player
    {
        public HeuristicPlayer (PlayerColor color) : base (color)
        {
            type = PlayerType.Heuristic;
        }

        public override (FieldInBoard, int, PlayerColor) MakeChoice (Board board, Player opponentStats)
        {
            throw new System.NotImplementedException ();
        }

    }
}
