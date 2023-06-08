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
            // pierwszy ruch: któryś z zewnętrznych kołków
            // potem: chcemy ruszyć kołek który jeszcze jest w naszym trójkącie
            // staramy się wybrać skok
            // chcemy być blisko pozostałych kołków
            // czyli każdy ruch będzie miał jakąś wartość h i będziemy wybierać ten z największą
            // skok ma automatycznie 0.2, odległość od każdego pionka tuż obok to 0.1, a pole dalej to 0.05
            // jeśli jeszcze nie było przesunięte z naszego trójkąta to 0.2
            throw new System.NotImplementedException ();
        }

    }
}
