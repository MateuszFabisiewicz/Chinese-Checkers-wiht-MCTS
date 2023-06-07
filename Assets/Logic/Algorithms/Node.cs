using Codice.CM.Client.Differences;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace Assets.Logic.Algorithms
{
    public class Node
    {
        public Node parent { get; private set; }

        public List<Node> children { get; private set; }

        public double winningProbability { get; set; } // X - win ratio

        public int visitCount { get; set; } // N

        public Board state { get; private set; }

        public (FieldInBoard newField, int checkerIndex, PlayerColor playerWhoMoved) move { get; private set; }

        public Node (Board state, Node parentNode)
        {
            this.state = state;
            this.parent = parentNode;
            children = new List<Node> ();
            visitCount = 0;
        }

        public Node (Board state, Node parentNode, int changedChecker, FieldInBoard fieldOfChecker, PlayerColor player)
        {
            this.state = state;
            this.parent = parentNode;
            children = new List<Node> ();
            visitCount = 0;
            move = (fieldOfChecker, changedChecker, player);
        }


        // color - gracz komputer który chce wykonać ruch; ważne tylko jak rodzic nie ma move
        public bool GenerateChildren (PlayerColor playerColor)
        {
            // dla każdego pionka patrzymy czy może się ruszyć
            // jeśli tak to tworzymy dzieci z każdego możliwego ruchu
            PlayerColor color = playerColor;

            if (parent != null)
            {
                color = parent.move.playerWhoMoved == PlayerColor.Red ? PlayerColor.Blue : PlayerColor.Red;
            }

            //if (parent == null) // jesteśmy w root
            //{
            for (int i = 0; i < Game.checkerCount; i++)
            {
                FieldInBoard field = state.FindCheckersPosition (i, color);

                #region zwykły ruch
                // możemy iść +1 na y, +1 na x, -1 na y, -1 na x, -1 na y +1 na x, +1 na y -1 na x
                if (field.y + 1 < Board.side && state.fields[field.x, field.y + 1].playerOnField == PlayerColor.None)
                {
                    Checker checker = new Checker (field.checker);
                    Board newState = new Board (state, field, state.fields[field.x, field.y + 1]);

                    Node node = new Node (newState, this, checker.ID, state.fields[field.x, field.y + 1], color);
                    children.Add (node);
                }

                if (field.x + 1 < Board.side && state.fields[field.x + 1, field.y].playerOnField == PlayerColor.None)
                {
                    Checker checker = new Checker (field.checker);
                    Board newState = new Board (state, field, state.fields[field.x + 1, field.y]);

                    Node node = new Node (newState, this, checker.ID, state.fields[field.x + 1, field.y], color);
                    children.Add (node);
                }

                if (field.y - 1 >= 0 && state.fields[field.x, field.y - 1].playerOnField == PlayerColor.None)
                {
                    Checker checker = new Checker (field.checker);
                    Board newState = new Board (state, field, state.fields[field.x, field.y - 1]);

                    Node node = new Node (newState, this, checker.ID, state.fields[field.x, field.y - 1], color);
                    children.Add (node);
                }

                if (field.x - 1 >= 0 && state.fields[field.x - 1, field.y].playerOnField == PlayerColor.None)
                {
                    Checker checker = new Checker (field.checker);
                    Board newState = new Board (state, field, state.fields[field.x - 1, field.y]);

                    Node node = new Node (newState, this, checker.ID, state.fields[field.x - 1, field.y], color);
                    children.Add (node);
                }

                if (field.y - 1 >= 0 && field.x + 1 < Board.side && state.fields[field.x + 1, field.y - 1].playerOnField == PlayerColor.None)
                {
                    Checker checker = new Checker (field.checker);
                    Board newState = new Board (state, field, state.fields[field.x + 1, field.y - 1]);

                    Node node = new Node (newState, this, checker.ID, state.fields[field.x + 1, field.y - 1], color);
                    children.Add (node);
                }

                if (field.x - 1 >= 0 && field.y + 1 < Board.side && state.fields[field.x - 1, field.y + 1].playerOnField == PlayerColor.None)
                {
                    Checker checker = new Checker (field.checker);
                    Board newState = new Board (state, field, state.fields[field.x - 1, field.y + 1]);

                    Node node = new Node (newState, this, checker.ID, state.fields[field.x - 1, field.y + 1], color);
                    children.Add (node);
                }

                #endregion

                #region skok
                // patrzymy na możliwości skoku - nie rozważamy cofnięć!
                // while można się ruszyć 
                var listOfJumpResults = Jump (field, state, field, field);
                listOfJumpResults = listOfJumpResults.Distinct ().ToList (); // sprawdzić czy distnict działa poprawnie

                foreach (var result in listOfJumpResults)
                {
                    Checker checker = new Checker (field.checker);

                    Node node = new Node (result.newState, this, checker.ID, result.newField, color);//(newState, this, checker.ID, result.newField, color);
                    children.Add (node);
                }

                #endregion
            }
            //}
            //else
            //{
            // jak w root, ale color to odwrotność koloru rodzica
            //}

            return children.Count > 0;
            //throw new NotImplementedException();
        }

        private List<(FieldInBoard newField, Board newState)> OneJump ((int x, int y) end, (int x, int y) inter, Board board, FieldInBoard oldField, FieldInBoard lastPlace, FieldInBoard originalPlace)
        {
            List<(FieldInBoard newField, Board newState)> jumps = new List<(FieldInBoard newField, Board newState)> ();

            if (end.x < Board.side && end.y < Board.side && end.x >= 0 && end.y >= 0
                && board.fields[inter.x, inter.y].playerOnField != PlayerColor.None
                && board.fields[end.x, end.y].playerOnField == PlayerColor.None
                && end.x != lastPlace.x && end.y != lastPlace.y
                && end.x != originalPlace.x && end.y != lastPlace.y)
            {
                Board newState = new Board (board, oldField, board.fields[end.x, end.y]);

                jumps.Add ((newState.fields[end.x, end.y], newState));
                jumps.AddRange (Jump (newState.fields[end.x, end.y], newState, oldField, originalPlace));
            }

            return jumps;
        }

        private List<(FieldInBoard newField, Board newState)> Jump (FieldInBoard oldField, Board board, FieldInBoard lastPlace, FieldInBoard originalPlace) // dodać zapobieganie zawracania (w stosunku do rodzica)
        {
            // rozważamy też te 6 pól dookoła oraz pole w linii prostej
            // po wykonaniu skoku możemy dalej skakać (potencjalnie)
            List<(FieldInBoard newField, Board newState)> jumps = new List<(FieldInBoard newField, Board newState)> ();

            jumps.AddRange (OneJump ((oldField.x, oldField.y + 2), (oldField.x, oldField.y + 1), board, oldField, lastPlace, originalPlace));
            //if (oldField.y + 2 < Board.side && board.fields[oldField.x, oldField.y + 1].playerOnField != PlayerColor.None
            //    && board.fields[oldField.x, oldField.y + 2].playerOnField == PlayerColor.None
            //    && oldField.x != lastPlace.x && oldField.y + 2 != lastPlace.y 
            //    && oldField.x != originalPlace.x && oldField.y + 2 != lastPlace.y)
            //{
            //    Board newState = new Board (board, oldField, board.fields[oldField.x, oldField.y + 2]);

            //    jumps.Add ((newState.fields[oldField.x, oldField.y + 2], newState));
            //    jumps.AddRange (Jump (newState.fields[oldField.x, oldField.y + 2], newState, oldField, originalPlace));
            //}

            jumps.AddRange (OneJump ((oldField.x + 2, oldField.y), (oldField.x + 1, oldField.y), board, oldField, lastPlace, originalPlace));
            //if (oldField.x + 2 < Board.side && board.fields[oldField.x + 1, oldField.y].playerOnField != PlayerColor.None
            //    && board.fields[oldField.x + 2, oldField.y].playerOnField == PlayerColor.None
            //    && oldField.x + 2 != lastPlace.x && oldField.y != lastPlace.y
            //    && oldField.x + 2 != originalPlace.x && oldField.y != lastPlace.y)
            //{
            //    Board newState = new Board (board, oldField, board.fields[oldField.x + 2, oldField.y]);

            //    jumps.Add ((newState.fields[oldField.x + 2, oldField.y], newState));
            //    jumps.AddRange (Jump (newState.fields[oldField.x + 2, oldField.y], newState, oldField, originalPlace));
            //}

            jumps.AddRange (OneJump ((oldField.x, oldField.y - 2), (oldField.x, oldField.y - 1), board, oldField, lastPlace, originalPlace));
            //if (oldField.y - 2 >= 0 && board.fields[oldField.x, oldField.y - 1].playerOnField != PlayerColor.None
            //    && board.fields[oldField.x, oldField.y - 2].playerOnField == PlayerColor.None
            //    && oldField.x != lastPlace.x && oldField.y - 2 != lastPlace.y
            //    && oldField.x != originalPlace.x && oldField.y - 2 != lastPlace.y)
            //{
            //    Board newState = new Board (board, oldField, board.fields[oldField.x, oldField.y - 2]);

            //    jumps.Add ((newState.fields[oldField.x, oldField.y - 2], newState));
            //    jumps.AddRange (Jump (newState.fields[oldField.x, oldField.y - 2], newState, oldField, originalPlace));
            //}

            jumps.AddRange (OneJump ((oldField.x - 2, oldField.y), (oldField.x - 1, oldField.y), board, oldField, lastPlace, originalPlace));
            //if (oldField.x - 2 >= 0 && board.fields[oldField.x - 1, oldField.y].playerOnField != PlayerColor.None
            //    && board.fields[oldField.x - 2, oldField.y].playerOnField == PlayerColor.None
            //    && oldField.x - 2 != lastPlace.x && oldField.y != lastPlace.y
            //    && oldField.x - 2 != originalPlace.x && oldField.y != lastPlace.y)
            //{
            //    Board newState = new Board (board, oldField, board.fields[oldField.x - 2, oldField.y]);

            //    jumps.Add ((newState.fields[oldField.x - 2, oldField.y], newState));
            //    jumps.AddRange (Jump (newState.fields[oldField.x - 2, oldField.y], newState, oldField, originalPlace));
            //}

            jumps.AddRange (OneJump ((oldField.x + 2, oldField.y - 2), (oldField.x + 1, oldField.y - 1), board, oldField, lastPlace, originalPlace));
            //if (oldField.y - 2 >= 0 && oldField.x + 2 < Board.side && board.fields[oldField.x + 1, oldField.y - 1].playerOnField != PlayerColor.None
            //    && board.fields[oldField.x + 2, oldField.y - 2].playerOnField == PlayerColor.None
            //    && oldField.x + 2 != lastPlace.x && oldField.y - 2 != lastPlace.y
            //    && oldField.x + 2 != originalPlace.x && oldField.y - 2 != lastPlace.y)
            //{
            //    Board newState = new Board (board, oldField, board.fields[oldField.x + 2, oldField.y - 2]);

            //    jumps.Add ((newState.fields[oldField.x + 2, oldField.y - 2], newState));
            //    jumps.AddRange (Jump (newState.fields[oldField.x + 2, oldField.y - 2], newState, oldField, originalPlace));
            //}

            jumps.AddRange (OneJump ((oldField.x - 2, oldField.y + 2), (oldField.x - 1, oldField.y + 1), board, oldField, lastPlace, originalPlace));
            //if (oldField.x - 2 >= 0 && oldField.y + 2 < Board.side && board.fields[oldField.x - 1, oldField.y + 1].playerOnField != PlayerColor.None
            //    && board.fields[oldField.x - 2, oldField.y + 2].playerOnField == PlayerColor.None
            //    && oldField.x - 2 != lastPlace.x && oldField.y + 2 != lastPlace.y
            //    && oldField.x - 2 != originalPlace.x && oldField.y + 2 != lastPlace.y)
            //{
            //    Board newState = new Board (board, oldField, board.fields[oldField.x - 2, oldField.y + 2]);

            //    jumps.Add ((newState.fields[oldField.x - 2, oldField.y + 2], newState));
            //    jumps.AddRange (Jump (newState.fields[oldField.x - 2, oldField.y + 2], newState, oldField, originalPlace));
            //}

            return jumps;
        }

        public void UpdateWinningProbability (PlayerColor color)
        {
            if (this.children.Count == 0)
            {
                if (this.state.IsWinning (color))
                {
                    this.winningProbability = 1;
                }
                else
                {
                    this.winningProbability = 0;
                }
            }
            else
            {
                double sum = 0;
                foreach (var child in this.children)
                {
                    sum += child.winningProbability;
                }
                this.winningProbability = sum / this.children.Count;
            }
        }

        public Node (Node original)
        {
            this.state = original.state;
            this.parent = original.parent;
            this.winningProbability = original.winningProbability;
            this.visitCount = original.visitCount;
            this.move = original.move;
            foreach (var child in original.children)
            {
                this.children.Add (new Node (child));
            }
        }
    }
}
