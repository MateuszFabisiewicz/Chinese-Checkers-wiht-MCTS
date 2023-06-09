﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;

namespace Assets.Logic.Algorithms
{
    public class UCTPlayer : Player
    {
        internal int loopCount = 1500;
        internal double C = Math.Sqrt (2); // stała eksploracji
        Random random;

        public UCTPlayer (PlayerColor color,int seed) : base (color)
        {
            type = PlayerType.UCT;
            random = new Random (seed);
        }

        public override (FieldInBoard, int, PlayerColor) MakeChoice (Board board, Player opponentStats)
        {
            root = new Node (board, null);
            root.winningProbability = 0.5;

            bool childrenGenerated = root.GenerateChildren (this.color); // tworzymy startowe drzewo - póki co tylko dzieci root, każde z szansą 0.5 wygranej,
                                                                         // chyba że akurat jest wygrana

            int i = 0;
            while (i < loopCount)
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
            root.children.Sort ((x, y) => x.winningProbability.CompareTo (y.winningProbability));

            return root.children.Last ();
        }

        internal virtual void Backpropagate (Node leaf, Node simulationResult) 
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

        internal virtual Node Rollout (Node leaf)
        {
            Node node = leaf;
            while (node.children.Count > 0) // póki możemy iść dalej
            {
                //stosujemy random rollout policy
                Random random = new Random ();

                int randomIndex = random.Next (0, node.children.Count);
                node = node.children[randomIndex];
            }

            return node; // zwracamy końcowy node
        }

        internal virtual Node Traverse (Node root)
        {
            // zgodnie z UCT
            Node node = root;

            while (node.children.Count != 0)
            {
                node.visitCount++;
                Node tmpNode = NotYetVisitedChild (node);
                if (tmpNode == null)
                    node = BestUCT (node);
                else
                    node = tmpNode;
            }

            node.visitCount++;
            return node;
        }

        internal Node NotYetVisitedChild (Node node)
        {
            for (int i = 0; i < node.children.Count; i++)
            {
                if (node.children[i].visitCount == 1) // tworzenie ustawia visitCount na 1
                {
                    return node.children[i];
                }
            }

            return null;
        }

        internal Node BestUCT (Node node)
        {
            double[] UCTValues = new double[node.children.Count];

            for (int i = 0; i < node.children.Count; i++)
            {
                UCTValues[i] = node.children[i].winningProbability + C * Math.Sqrt (Math.Log (node.visitCount) / node.children[i].visitCount);
            }

            return node.children[UCTValues.ToList ().IndexOf (UCTValues.Max ())];
        }

        private void Expand (Node leaf)
        {
            while (leaf.children.Count > 0)
            {
                leaf = Traverse (leaf);
            }

            if (leaf.children.Count == 0)
            {
                bool expanded = leaf.GenerateChildren (this.color);
            }
        }
    }
}
