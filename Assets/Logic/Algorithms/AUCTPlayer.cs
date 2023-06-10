using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Logic.Algorithms
{
    public class AUCTPlayer : UCTPlayer
    {
        internal double lambda = 0.5; // do przetestowania ewentualnie

        public AUCTPlayer (PlayerColor color) : base (color)
        {
            type = PlayerType.AUCT;
        }

        // nadpisujemy Backpropagate aby aktualizować też accelerated winning ratio
        internal override void Backpropagate (Node leaf, Node simulationResult)
        {
            simulationResult.UpdateAccWinRation (color);
            simulationResult.UpdateWinningProbability (color);
            Node node = simulationResult.parent;

            while (node.parent != null)
            {
                node.UpdateAccWinRation (color);
                node.UpdateWinningProbability (color);
                node = node.parent;
            }
        }

        // nadpisujemy też Traverse aby aktualizować velocity
        internal override Node Traverse (Node root) 
        {
            Node node = root;

            while (node.children.Count != 0)
            {
                node.visitCount++;
                foreach (var child in node.children)
                {
                    child.velocity = lambda * child.velocity;
                }
                Node tmpNode = NotYetVisitedChild (node);
                if (tmpNode == null)
                    node = BestAUCT (node);
                else 
                    node = tmpNode;

                node.velocity += 1;
            }
            // aktualizacja velocity wygląda tak, że jesteśmy w pewnym node, rozważamy dzieci
            // i każdemu zmieniamy velocity - czyli robimy to w AUCT
            // i jeśli zostanie ten wybrany, to dodajemy mu 1 (wpp. "dodajemy" 0)

            node.visitCount++;
            return node;
        }

        //internal override Node Rollout (Node leaf)
        //{
        //    Node node = leaf;
        //    while (node.children.Count > 0) // póki możemy iść dalej
        //    {
        //         stosujemy random rollout policy -- zmienić też na UCT??
        //        Random random = new Random ();
        //        int randomIndex = random.Next (0, node.children.Count);
        //        node = node.children[randomIndex];

        //        node = BestAUCT (node);
        //    }

        //    return node; // zwracamy końcowy node
        //}

        // zmieniamy BestUCT na BestAUCT
        internal Node BestAUCT(Node node)
        {
            double[] AUCTValues = new double[node.children.Count];

            for (int i = 0; i < node.children.Count; i++)
            {
                //node.children[i].velocity = lambda * node.children[i].velocity;
                //node.children[i].UpdateAccWinRation (color); // chyba?
                AUCTValues[i] = node.children[i].accWinRation + C * Math.Sqrt (Math.Log (node.visitCount) / node.children[i].visitCount);
            }

            return node.children[AUCTValues.ToList ().IndexOf (AUCTValues.Max ())];
        }
    }
}
