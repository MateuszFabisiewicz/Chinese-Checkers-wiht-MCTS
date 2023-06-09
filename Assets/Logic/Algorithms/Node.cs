using Codice.CM.Client.Differences;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using static PlasticPipe.PlasticProtocol.Messages.Serialization.ItemHandlerMessagesSerialization;
using static UnityEditor.Graphs.Styles;

namespace Assets.Logic.Algorithms
{
    public class Node
    {
        public Node parent { get; private set; }

        public List<Node> children { get; private set; }

        public double winningProbability { get; set; } // X - win ratio

        public int visitCount { get; set; } // N

        public Board state { get; private set; }

        public double velocity { get; set; } // dla gracza AUCT

        public double accWinRation { get; set; } // dla gracza AUCT

        public (FieldInBoard newField, int checkerIndex, PlayerColor playerWhoMoved) move { get; private set; }

        public Node (Board state, Node parentNode)
        {
            this.state = state;
            this.parent = parentNode;
            children = new List<Node> ();
            visitCount = 1;
            velocity = 1;
        }

        public Node (Board state, Node parentNode, int changedChecker, FieldInBoard fieldOfChecker, PlayerColor player)
        {
            this.state = state;
            this.parent = parentNode;
            children = new List<Node> ();
            visitCount = 1;
            velocity = 1;
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

            for (int i = 0; i < Game.checkerCount; i++)
            {
                FieldInBoard field = state.FindCheckersPosition (i, color);

                #region zwykły ruch
                // możemy iść +1 na y, +1 na x, -1 na y, -1 na x, -1 na y +1 na x, +1 na y -1 na x

                Node newNode = OneMove ((field.x, field.y + 1), state, field, color);
                if (newNode != null)
                {
                    children.Add (newNode);
                }

                newNode = OneMove ((field.x + 1, field.y), state, field, color);
                if (newNode != null)
                {
                    children.Add (newNode);
                }

                newNode = OneMove ((field.x, field.y - 1), state, field, color);
                if (newNode != null)
                {
                    children.Add (newNode);
                }

                newNode = OneMove ((field.x - 1, field.y), state, field, color);
                if (newNode != null)
                {
                    children.Add (newNode);
                }

                newNode = OneMove ((field.x + 1, field.y - 1), state, field, color);
                if (newNode != null)
                {
                    children.Add (newNode);
                }

                newNode = OneMove ((field.x - 1, field.y + 1), state, field, color);
                if (newNode != null)
                {
                    children.Add (newNode);
                }

                #endregion

                #region skok
                // patrzymy na możliwości skoku - nie rozważamy cofnięć!
                // while można się ruszyć 
                var listOfJumpResults = Jump (field, state, field, field, color);
                listOfJumpResults = listOfJumpResults.Distinct ().ToList (); // sprawdzić czy distnict działa poprawnie

                foreach (var result in listOfJumpResults)
                {
                    Checker checker = new Checker (field.checker);

                    Node node = new Node (result.newState, this, checker.ID, result.newField, color);//(newState, this, checker.ID, result.newField, color);
                    children.Add (node);
                }

                #endregion
            }

            return children.Count > 0;
        }

        private Node OneMove ((int x, int y) end, Board board, FieldInBoard oldField, PlayerColor playerColor)
        {
            PlayerColor opponentColor = playerColor == PlayerColor.Red ? PlayerColor.Blue : PlayerColor.Red;
            bool isInOpponent = oldField.fieldType == opponentColor;

            if (end.x < Board.side && end.y < Board.side && end.x >= 0 && end.y >= 0
                && board.fields[end.x, end.y].playerOnField == PlayerColor.None &&
                ((isInOpponent && board.fields[end.x, end.y].fieldType == opponentColor) || !isInOpponent))
            {
                Board newState = new Board (board, oldField, board.fields[end.x, end.y]);

                Node node = new Node (newState, this, oldField.checker.ID, newState.fields[end.x, end.y], oldField.checker.color);
                return node;
            }
            else 
                return null;
        }

        private List<(FieldInBoard newField, Board newState)> OneJump ((int x, int y) end, (int x, int y) inter, Board board, FieldInBoard oldField, FieldInBoard lastPlace, FieldInBoard originalPlace, PlayerColor playerColor)
        {
            List<(FieldInBoard newField, Board newState)> jumps = new List<(FieldInBoard newField, Board newState)> ();
            PlayerColor opponentColor = playerColor == PlayerColor.Red ? PlayerColor.Blue : PlayerColor.Red;
            bool isInOpponent = oldField.fieldType == opponentColor;

            if (end.x < Board.side && end.y < Board.side && end.x >= 0 && end.y >= 0
                && board.fields[inter.x, inter.y].playerOnField != PlayerColor.None
                && board.fields[end.x, end.y].playerOnField == PlayerColor.None
                && end.x != lastPlace.x && end.y != lastPlace.y
                && end.x != originalPlace.x && end.y != lastPlace.y &&
                ((isInOpponent && board.fields[end.x, end.y].fieldType == opponentColor) || !isInOpponent))
            {
                Board newState = new Board (board, oldField, board.fields[end.x, end.y]);

                jumps.Add ((newState.fields[end.x, end.y], newState));
                jumps.AddRange (Jump (newState.fields[end.x, end.y], newState, oldField, originalPlace, playerColor));
            }

            return jumps;
        }

        private List<(FieldInBoard newField, Board newState)> Jump (FieldInBoard oldField, Board board, FieldInBoard lastPlace, FieldInBoard originalPlace, PlayerColor playerColor) // dodać zapobieganie zawracania (w stosunku do rodzica)
        {
            // rozważamy też te 6 pól dookoła oraz pole w linii prostej
            // po wykonaniu skoku możemy dalej skakać (potencjalnie)
            List<(FieldInBoard newField, Board newState)> jumps = new List<(FieldInBoard newField, Board newState)> ();

            jumps.AddRange (OneJump ((oldField.x, oldField.y + 2), (oldField.x, oldField.y + 1), board, oldField, lastPlace, originalPlace, playerColor));

            jumps.AddRange (OneJump ((oldField.x + 2, oldField.y), (oldField.x + 1, oldField.y), board, oldField, lastPlace, originalPlace, playerColor));

            jumps.AddRange (OneJump ((oldField.x, oldField.y - 2), (oldField.x, oldField.y - 1), board, oldField, lastPlace, originalPlace, playerColor));

            jumps.AddRange (OneJump ((oldField.x - 2, oldField.y), (oldField.x - 1, oldField.y), board, oldField, lastPlace, originalPlace, playerColor));

            jumps.AddRange (OneJump ((oldField.x + 2, oldField.y - 2), (oldField.x + 1, oldField.y - 1), board, oldField, lastPlace, originalPlace, playerColor));

            jumps.AddRange (OneJump ((oldField.x - 2, oldField.y + 2), (oldField.x - 1, oldField.y + 1), board, oldField, lastPlace, originalPlace, playerColor));

            return jumps;
        }

        public void UpdateWinningProbability (PlayerColor color)
        {
            if (this.children.Count == 0)
            {
                this.winningProbability = this.state.IsWinning (color);
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

        public void UpdateAccWinRation (PlayerColor color)
        {
            //this.accWinRation = 0;
            if (this.children.Count == 0)
            {
                this.accWinRation = this.state.IsWinning (color);
            }
            else
            {
                this.accWinRation = 0;
                double w = this.CalculateW ();
                foreach (var child in this.children)
                {
                    this.accWinRation += w * child.accWinRation;
                }
            }
        }

        public double CalculateW ()
        {
            double sumOfChildV = 0;
            foreach (var child in this.children)
            {
                sumOfChildV += child.velocity;
            }

            if (sumOfChildV != 0)
                return this.velocity / sumOfChildV;
            else 
                return 0;
        }

        public Node (Node original)
        {
            this.state = original.state;
            this.parent = original.parent;
            this.winningProbability = original.winningProbability;
            this.visitCount = original.visitCount;
            this.move = original.move;
            this.children = new List<Node> ();
            foreach (var child in original.children)
            {
                this.children.Add (new Node (child));
            }
        }
    }
}
