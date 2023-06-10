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
        private const int strongDist = 1;
        private const int distance = 2;
        private const double jumpH = 0.3;
        private const double strongDistH = 0.1;
        private const double distH = 0.05;
        private const double triangleH = 0.5;
        private const double opponentTriangleH = 0.2;
        private const double forwardMove = 0.4; // zhardkodowane dla kolorów
        private const double notYetMoved = 0.4; // startingField w checker
        private const int maxList = 1000000000; // żeby zapobiec StackOverflowException

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
            // wejście w trójkąt przeciwnika to 0.15
            // pójście "do przodu" jakoś uwzględnić?

            bool isFirstMove = true;
            List<(FieldInBoard newField, int checkerId, double heuristic)> possibleMoves = new List<(FieldInBoard newField, int checkerId, double heuristic)> ();


            for (int i = 0; i < Board.side; i++)
            {
                for (int j = 0; j < Board.side; j++)
                {
                    if (board.fields[i, j].fieldType == color && board.fields[i, j].playerOnField != color)
                    {
                        isFirstMove = false;
                        break;
                    }
                }
            }

            if (isFirstMove)
            {
                // znajdujemy brzegowe checkery (indeksy [0,3], [3,0] lub [5,8][8,5])
                // i ruszamy je na ukosy
                if (board.fields[0, 3].fieldType == color)
                {
                    possibleMoves.Add ((board.fields[0, 4], board.fields[0, 3].checker.ID, 1));
                    possibleMoves.Add ((board.fields[1, 3], board.fields[0, 3].checker.ID, 1));
                    possibleMoves.Add ((board.fields[3, 1], board.fields[3, 0].checker.ID, 1));
                    possibleMoves.Add ((board.fields[4, 0], board.fields[3, 0].checker.ID, 1));
                }
                else
                {
                    possibleMoves.Add ((board.fields[4, 8], board.fields[5, 8].checker.ID, 1));
                    possibleMoves.Add ((board.fields[5, 7], board.fields[5, 8].checker.ID, 1));
                    possibleMoves.Add ((board.fields[7, 5], board.fields[8, 5].checker.ID, 1));
                    possibleMoves.Add ((board.fields[8, 4], board.fields[8, 5].checker.ID, 1));
                }
            }
            else
            {
                possibleMoves = GenerateMoves (board, opponentStats, color);
            }

            // zwracamy ruch z największą wartością heurystyki
            possibleMoves.Sort ((x, y) => y.heuristic.CompareTo (x.heuristic));
            //possibleMoves.OrderByDescending (x => x.heuristic);
            return (possibleMoves[0].newField, possibleMoves[0].checkerId, color);
        }

        private List<(FieldInBoard newField, int checkerId, double heuristic)> GenerateMoves (Board board, Player opponentStats, PlayerColor color)
        {
            List<(FieldInBoard, int, double)> moves = new List<(FieldInBoard, int, double)> ();

            for (int i = 0; i < Game.checkerCount; i++)
            {
                // sprawdzamy czy możemy się ruszyć
                List<FieldInBoard> possibleJumps = GetPossibleFieldsThroughJumps (board.FindCheckersPosition (i, color), board, board.FindCheckersPosition (i, color));
                foreach (FieldInBoard field in possibleJumps)
                {
                    if (moves.Count < maxList)
                        moves.Add ((field, i, CalculateHeuristic (field, i, board, true, color, board.FindCheckersPosition (i, color))));
                }

                List<FieldInBoard> possibleFieldsOneApart = GetPossibleFieldsOneApart (i, color, board);
                foreach (FieldInBoard field in possibleFieldsOneApart)
                {
                    if (moves.Count < maxList)
                        moves.Add ((field, i, CalculateHeuristic (field, i, board, false, color, board.FindCheckersPosition (i, color))));
                }

            }

            return moves;
        }

        private List<FieldInBoard> GetPossibleFieldsThroughJumps (FieldInBoard startField, Board board, FieldInBoard previousPlace)
        {
            List<FieldInBoard> moves = new List<FieldInBoard> ();
            List<FieldInBoard> addMoves = new List<FieldInBoard> ();

            addMoves = OneJump (startField, board, (startField.x, startField.y + 2), (startField.x, startField.y + 1), previousPlace);
            for (int i = 0; i < addMoves.Count; i++)
            {
                if (moves.Count >= maxList)
                    break;
                moves.Add (addMoves[i]);
            }
            //if (moves.Count < maxList)
            //    moves.AddRange (OneJump (startField, board, (startField.x, startField.y + 2), (startField.x, startField.y + 1), previousPlace));

            addMoves = OneJump (startField, board, (startField.x, startField.y - 2), (startField.x, startField.y - 1), previousPlace);
            for (int i = 0; i < addMoves.Count; i++)
            {
                if (moves.Count >= maxList)
                    break;
                moves.Add (addMoves[i]);
            }
            //if (moves.Count < maxList)
            //    moves.AddRange (OneJump (startField, board, (startField.x, startField.y - 2), (startField.x, startField.y - 1), previousPlace));

            addMoves = OneJump (startField, board, (startField.x + 2, startField.y), (startField.x + 1, startField.y), previousPlace);
            for (int i = 0; i < addMoves.Count; i++)
            {
                if (moves.Count >= maxList)
                    break;
                moves.Add (addMoves[i]);
            }
            //if (moves.Count < maxList)
            //    moves.AddRange (OneJump (startField, board, (startField.x + 2, startField.y), (startField.x + 1, startField.y), previousPlace));

            addMoves = OneJump (startField, board, (startField.x - 2, startField.y), (startField.x - 1, startField.y), previousPlace);
            for (int i = 0; i < addMoves.Count; i++)
            {
                if (moves.Count >= maxList)
                    break;
                moves.Add (addMoves[i]);
            }
            //if (moves.Count < maxList)
            //    moves.AddRange (OneJump (startField, board, (startField.x - 2, startField.y), (startField.x - 1, startField.y), previousPlace));

            addMoves = OneJump (startField, board, (startField.x - 2, startField.y + 2), (startField.x - 1, startField.y + 1), previousPlace);
            for (int i = 0; i < addMoves.Count; i++)
            {
                if (moves.Count >= maxList)
                    break;
                moves.Add (addMoves[i]);
            }
            //if (moves.Count < maxList)
            //    moves.AddRange (OneJump (startField, board, (startField.x - 2, startField.y + 2), (startField.x - 1, startField.y + 1), previousPlace));

            addMoves = OneJump (startField, board, (startField.x + 2, startField.y - 2), (startField.x + 1, startField.y - 1), previousPlace);
            for (int i = 0; i < addMoves.Count; i++)
            {
                if (moves.Count >= maxList)
                    break;
                moves.Add (addMoves[i]);
            }
            //if (moves.Count < maxList)
            //    moves.AddRange (OneJump (startField, board, (startField.x + 2, startField.y - 2), (startField.x + 1, startField.y - 1), previousPlace));

            return moves;
        }

        private List<FieldInBoard> OneJump (FieldInBoard startField, Board board, (int X, int Y) newField, (int X, int Y) interField, FieldInBoard previousPlace)
        {
            List<FieldInBoard> moves = new List<FieldInBoard> ();
            PlayerColor opponentColor = color == PlayerColor.Blue ? PlayerColor.Red : PlayerColor.Blue;
            bool isInOpponent = startField.fieldType == opponentColor;

            if (newField.X < Board.side && newField.X >= 0 && newField.Y < Board.side && newField.Y >= 0 &&
                previousPlace.x != newField.X && previousPlace.y != newField.Y &&
                board.fields[interField.X, interField.Y].playerOnField != PlayerColor.None &&
                board.fields[newField.X, newField.Y].playerOnField == PlayerColor.None &&
                (!isInOpponent || (isInOpponent && board.fields[newField.X, newField.Y].fieldType == opponentColor)))
            {
                if (moves.Count < maxList)
                {
                    moves.Add (board.fields[newField.X, newField.Y]);

                    List<FieldInBoard> addMoves = GetPossibleFieldsThroughJumps (board.fields[newField.X, newField.Y], board, startField);

                    for (int i = 0; i < addMoves.Count; i++)
                    {
                        if (moves.Count >= maxList)
                            break;
                        moves.Add (addMoves[i]);
                    }
                    //moves.AddRange (addMoves);
                }
            }

            return moves;
        }

        private List<FieldInBoard> GetPossibleFieldsOneApart (int i, PlayerColor color, Board board)
        {
            FieldInBoard checkerField = board.FindCheckersPosition (i, color);
            PlayerColor opponentColor = color == PlayerColor.Blue ? PlayerColor.Red : PlayerColor.Blue;
            bool isInOpponent = checkerField.fieldType == opponentColor;

            List<FieldInBoard> possibleFields = new List<FieldInBoard> ();
            if (checkerField.x + 1 < Board.side && board.fields[checkerField.x + 1, checkerField.y].playerOnField == PlayerColor.None &&
                (!isInOpponent || (isInOpponent && board.fields[checkerField.x + 1, checkerField.y].fieldType == opponentColor)))
            {
                possibleFields.Add (board.fields[checkerField.x + 1, checkerField.y]);
            }
            if (checkerField.x - 1 >= 0 && board.fields[checkerField.x - 1, checkerField.y].playerOnField == PlayerColor.None &&
                (!isInOpponent || (isInOpponent && board.fields[checkerField.x - 1, checkerField.y].fieldType == opponentColor)))
            {
                possibleFields.Add (board.fields[checkerField.x - 1, checkerField.y]);
            }
            if (checkerField.y + 1 < Board.side && board.fields[checkerField.x, checkerField.y + 1].playerOnField == PlayerColor.None &&
                (!isInOpponent || (isInOpponent && board.fields[checkerField.x, checkerField.y + 1].fieldType == opponentColor)))
            {
                possibleFields.Add (board.fields[checkerField.x, checkerField.y + 1]);
            }
            if (checkerField.y - 1 >= 0 && board.fields[checkerField.x, checkerField.y - 1].playerOnField == PlayerColor.None &&
                (!isInOpponent || (isInOpponent && board.fields[checkerField.x, checkerField.y - 1].fieldType == opponentColor)))
            {
                possibleFields.Add (board.fields[checkerField.x, checkerField.y - 1]);
            }
            if (checkerField.x + 1 < Board.side && checkerField.y - 1 >= 0 && board.fields[checkerField.x + 1, checkerField.y - 1].playerOnField == PlayerColor.None &&
                (!isInOpponent || (isInOpponent && board.fields[checkerField.x + 1, checkerField.y - 1].fieldType == opponentColor)))
            {
                possibleFields.Add (board.fields[checkerField.x + 1, checkerField.y - 1]);
            }
            if (checkerField.x - 1 >= 0 && checkerField.y + 1 < Board.side && board.fields[checkerField.x - 1, checkerField.y + 1].playerOnField == PlayerColor.None &&
                (!isInOpponent || (isInOpponent && board.fields[checkerField.x - 1, checkerField.y + 1].fieldType == opponentColor)))
            {
                possibleFields.Add (board.fields[checkerField.x - 1, checkerField.y + 1]);
            }

            return possibleFields;
        }

        private double CalculateHeuristic (FieldInBoard newField, int checkerId, Board board, bool jump, PlayerColor color, FieldInBoard oldField)
        {
            double heuristic = 0;
            bool inOpponent = newField.fieldType != PlayerColor.None && newField.fieldType != color && oldField.fieldType == newField.fieldType; // już weszliśmy wcześniej

            if (jump)
            {
                heuristic += jumpH;
            }

            if (oldField.fieldType == color && newField.fieldType != color)
            {
                heuristic += triangleH;
            }

            if (newField.fieldType != color && newField.fieldType != PlayerColor.None && (oldField.fieldType == PlayerColor.None || oldField.fieldType == color))
            {
                heuristic += opponentTriangleH;
            }

            if (oldField.x == checkers[checkerId].startingPlace.x && oldField.y == checkers[checkerId].startingPlace.y)
            {
                heuristic += notYetMoved;
            }

            // patrzymy na pola o 1 i 2 odsunięte od newField w liniach prostych
            for (int i = 1; i < distance; i++)
            {
                if (newField.x + i < Board.side && board.fields[newField.x + i, newField.y].playerOnField == color)
                {
                    if (i <= strongDist)
                        heuristic += strongDistH;
                    else
                        heuristic += distH;
                }
                if (newField.x - i >= 0 && board.fields[newField.x - i, newField.y].playerOnField == color)
                {
                    if (i <= strongDist)
                        heuristic += strongDistH;
                    else
                        heuristic += distH;
                }
                if (newField.y + i < Board.side && board.fields[newField.x, newField.y + i].playerOnField == color)
                {
                    if (i <= strongDist)
                        heuristic += strongDistH;
                    else
                        heuristic += distH;
                }
                if (newField.y - i >= 0 && board.fields[newField.x, newField.y - i].playerOnField == color)
                {
                    if (i <= strongDist)
                        heuristic += strongDistH;
                    else
                        heuristic += distH;
                }
                if (newField.x + i < Board.side && newField.y - i >= 0 && board.fields[newField.x + i, newField.y - i].playerOnField == color)
                {
                    if (i <= strongDist)
                        heuristic += strongDistH;
                    else
                        heuristic += distH;
                }
                if (newField.x - i >= 0 && newField.y + i < Board.side && board.fields[newField.x - i, newField.y + i].playerOnField == color)
                {
                    if (i <= strongDist)
                        heuristic += strongDistH;
                    else
                        heuristic += distH;
                }
            }

            if (!inOpponent)
            {
                if (color == PlayerColor.Blue)
                {
                    // backward jest gdy x lub y były zmniejszone
                    if (newField.x >= oldField.x && newField.y >= oldField.y)
                    {
                        heuristic += forwardMove;
                    }
                }
                else // RED
                {
                    // backward jest gdy x lub y były zwiększone
                    if (newField.x <= oldField.x && newField.y <= oldField.y)
                    {
                        heuristic += forwardMove;
                    }
                }
            }

            return heuristic;
        }
    }
}
