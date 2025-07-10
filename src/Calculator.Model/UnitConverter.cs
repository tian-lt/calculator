using System.Collections.Generic;
using System.Diagnostics;

namespace CalculatorApp.Model
{
    public class UnitConverter
    {
        private Dictionary<string, Node> _nodes = new Dictionary<string, Node>();

        // we homogeneous transformations to reduce translation into linear transformations
        // parameter ratio is an array of diagonal elements of the transformation matrix
        public void ClaimRatio(string to, string from, decimal[] ratio)
        {
            if (!_nodes.ContainsKey(to))
            {
                _nodes.Add(to, new Node { Name = to, Parent = to, Ratio = IdentityMat(ratio.Length) });
            }
            if (!_nodes.ContainsKey(from))
            {
                _nodes.Add(from, new Node { Name = from, Parent = to, Ratio = ratio });
            }

            var rootTo = Find(to);
            var rootFrom = Find(from);
            if (rootTo != rootFrom)
            {
                rootFrom.Parent = rootTo.Name;
                rootFrom.Ratio = MulDiagMat(
                    InvDiagMat(_nodes[from].Ratio),
                    MulDiagMat(_nodes[to].Ratio, ratio));
            }
        }

        // parameter `value` is an array consisting of the first (N-1) elements of
        // the homogeneous vector of rank N, and it's a row-major vector.
        public decimal Convert(string to, string from, decimal[] value)
        {
            var rootTo = Find(to);
            var rootFrom = Find(from);
            if (rootTo != rootFrom)
            {
                throw new System.ArgumentException($"can't find the relationship between {to} and {from}");
            }
            return 0;
        }

        private class Node
        {
            public string Name { get; set; }
            public string Parent { get; set; }
            public decimal[] Ratio { get; set; }
        }

        private Node Find(string name)
        {
            var curr = _nodes[name];
            if (curr.Parent != name)
            {
                var parent = _nodes[curr.Parent];
                var root = Find(curr.Parent);
                if (parent.Name != root.Name)
                {
                    curr.Ratio = MulDiagMat(curr.Ratio, parent.Ratio);
                }
                curr.Parent = root.Name;
                return root;
            }
            return curr;
        }

        private decimal[] MulDiagMat(decimal[] matA, decimal[] matB)
        {
            Debug.Assert(matA.Length == matB.Length);
            var result = new decimal[matA.Length];
            for (int i = 0; i < matA.Length; i++)
            {
                result[i] = matA[i] * matB[i];
            }
            return result;
        }

        private decimal[] InvDiagMat(decimal[] mat)
        {
            var result = new decimal[mat.Length];
            for (int i = 0; i < mat.Length; i++)
            {
                result[i] = 1 / mat[i];
            }
            return result;
        }

        private decimal[] IdentityMat(int size)
        {
            var result = new decimal[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = 1;
            }
            return result;
        }
    }
}
