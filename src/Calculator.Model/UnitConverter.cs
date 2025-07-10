using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CalculatorApp.Model
{
    public class UnitConverter<TName>
    {
        private Dictionary<TName, Node> _nodes = new Dictionary<TName, Node>();

        public const decimal Precision = 1e-10m;

        public static decimal[] Scalar(decimal value) => new decimal[] { value, 1 };
        public static decimal[] Line(decimal k, decimal b) => new decimal[] { k, b };

        // we homogeneous transformations to reduce translation into linear transformations
        // parameter ratio is an array of diagonal elements of the transformation matrix
        public void ClaimRatio(TName to, TName from, decimal[] ratio)
        {
            if (Equals(to, from))
            {
                throw new ArgumentException("to and from can't be the same name");
            }
            if (IsZeroCoeffMat(ratio))
            {
                throw new ArgumentException("ratio can't be zero");
            }

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
                    MulDiagMat(InvDiagMat(_nodes[from].Ratio), ratio),
                    _nodes[to].Ratio);
            }
        }

        public void ClaimRatio(TName to, TName from, decimal ratio) => ClaimRatio(to, from, Scalar(ratio));

        // parameter `value` is an array consisting of the first (N-1) elements of
        // the homogeneous vector of rank N, and it's a row-major vector.
        public decimal[] Convert(TName to, TName from, decimal[] value)
        {
            var rootTo = Find(to);
            var rootFrom = Find(from);
            if (rootTo != rootFrom)
            {
                throw new ArgumentException($"can't find the relationship between {to} and {from}");
            }
            var ration = MulDiagMat(_nodes[from].Ratio, InvDiagMat(_nodes[to].Ratio));
            return ReduceHomogeneousPoint(MulDiagMat(HomogeneousPoint(value), ration));
        }

        public decimal Convert(TName to, TName from, decimal value)
        {
            return Convert(to, from, new decimal[] { value })[0];
        }

        private class Node
        {
            public TName Name { get; set; }
            public TName Parent { get; set; }
            public decimal[] Ratio { get; set; }
        }

        private Node Find(TName name)
        {
            var curr = _nodes[name];
            if (!Equals(curr.Parent, name))
            {
                var parent = _nodes[curr.Parent];
                var root = Find(curr.Parent);
                if (!Equals(parent.Name, root.Name))
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
            for (int i = 0; i < matA.Length; ++i)
            {
                result[i] = matA[i] * matB[i];
            }
            return result;
        }

        private decimal[] InvDiagMat(decimal[] mat)
        {
            var result = new decimal[mat.Length];
            for (int i = 0; i < mat.Length; ++i)
            {
                result[i] = 1 / mat[i];
            }
            return result;
        }

        private decimal[] IdentityMat(int size)
        {
            var result = new decimal[size];
            for (int i = 0; i < size; ++i)
            {
                result[i] = 1;
            }
            return result;
        }

        private bool IsZeroCoeffMat(decimal[] mat)
        {
            for (int i = 0; i < mat.Length - 1; ++i)
            {
                if (Math.Abs(mat[i]) > Precision)
                {
                    return false;
                }
            }
            return true;
        }

        private decimal[] HomogeneousPoint(decimal[] point)
        {
            var result = new decimal[point.Length + 1];
            for (int i = 0; i < point.Length; ++i)
            {
                result[i] = point[i];
            }
            result[point.Length] = 1;
            return result;
        }

        private decimal[] ReduceHomogeneousPoint(decimal[] hgPoint)
        {
            var result = new decimal[hgPoint.Length - 1];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = hgPoint[i] / hgPoint[hgPoint.Length - 1];
            }
            return result;
        }
    }
}
