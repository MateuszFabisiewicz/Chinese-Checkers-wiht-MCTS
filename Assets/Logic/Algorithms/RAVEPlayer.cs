using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Logic.Algorithms
{
    public class RAVEPlayer : UCTPlayer
    {
        private double b = 0.5; // chyba?

        public RAVEPlayer(PlayerColor color) : base(color)
        {
            type = PlayerType.RAVE;
        }
        internal override Node Traverse(Node root)
        {
            Node node = root;

            while (node.children.Count != 0)
            {
                node.visitCount++;
                node = BestRAVE(node);
            }
            
            node.visitCount++;
           
            return node;
        }
        internal override void Backpropagate(Node leaf, Node simulationResult) // leaf niepotrzebny?
        {
            //idziemy od simulationResult po parentach, a� dojdziemy do leaf
            simulationResult.UpdateWinningProbability(color);
            simulationResult.UpdateRAVEWinRationAndRAVEVisitCount(color);

            Node node = simulationResult.parent;

            while (node.parent != null)
            {
                node.UpdateWinningProbability(color);
                node.UpdateRAVEWinRationAndRAVEVisitCount(color);
                node = node.parent;
            }
        }

        // n - liczba wizyt w stanie
        // nn - liczba wszystkich symulacji zawieraj�cych ruch i
        private double Beta(int n, int nn)
        {
            return nn / (n + nn + 4 * b * b * n * nn);
        }
        // zmieniamy BestUCT na BestRAVE
        internal Node BestRAVE(Node node)
        {
            double[] RAVEValues = new double[node.children.Count];
            for (int i = 0; i < node.children.Count; i++)
            {
                var beta = Beta(node.children[i].visitCount, node.children[i].RAVEVisitCount);
                RAVEValues[i] = (1 - beta) * node.children[i].winningProbability + beta * node.children[i].RAVEWinRation + C * Math.Sqrt(Math.Log(node.visitCount) / node.children[i].visitCount);
            }

            return node.children[RAVEValues.ToList().IndexOf(RAVEValues.Max())];
        }
    }
}
