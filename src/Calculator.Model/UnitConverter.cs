// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace CalculatorApp.Model
{
    public class UnitConverter<TName, TCategory>
    {
        private Dictionary<TName, Node> _nodes = new Dictionary<TName, Node>();
        private Dictionary<TName, TCategory> _categories = new Dictionary<TName, TCategory>();

        public const decimal Precision = 1e-10m;

        public void ClaimRatio(TName to, TName from, decimal scalarRatio) =>
            ClaimRatio(to, from, new UnitTransform { m11 = scalarRatio, m12 = 0, m21 = 0, m22 = 1 });

        public void ClaimRatio(TName to, TName from, decimal k, decimal b) =>
            ClaimRatio(to, from, new UnitTransform { m11 = k, m12 = 0, m21 = b, m22 = 1 });

        public decimal Convert(TName to, TName from, decimal value)
        {
            var res = Convert(to, from, new UnitPoint { x = value, w = 1 });
            return res.x / res.w;
        }

        public void Classify(TName name, TCategory category)
        {
            var root = Find(name);
            if (_categories.ContainsKey(root.Name))
            {
                _categories[root.Name] = category;
            }
            else
            {
                _categories.Add(root.Name, category);
            }
        }

        public TCategory Category(TName name)
        {
            var root = Find(name);
            return _categories[root.Name];
        }

        private void ClaimRatio(TName to, TName from, UnitTransform ratio)
        {
            if (Equals(to, from))
            {
                throw new ArgumentException("to and from can't be the same name");
            }
            if (Math.Abs(ratio.m11) < Precision)
            {
                throw new ArgumentException("ratio can't be zero");
            }

            if (!_nodes.ContainsKey(to))
            {
                _nodes.Add(to, new Node { Name = to, Parent = to, Ratio = UnitMath.IdentityMat() });
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
                rootFrom.Ratio = UnitMath.Mul(
                    UnitMath.Mul(UnitMath.Inv(_nodes[from].Ratio), ratio),
                    _nodes[to].Ratio);
            }
        }

        private UnitPoint Convert(TName to, TName from, UnitPoint value)
        {
            var rootTo = Find(to);
            var rootFrom = Find(from);
            if (rootTo != rootFrom)
            {
                throw new ArgumentException($"can't find the relationship between {to} and {from}");
            }
            var ratio = UnitMath.Mul(_nodes[from].Ratio, UnitMath.Inv(_nodes[to].Ratio));
            return UnitMath.Mul(value, ratio);
        }


        private class Node
        {
            public TName Name { get; set; }
            public TName Parent { get; set; }
            public UnitTransform Ratio { get; set; }
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
                    curr.Ratio = UnitMath.Mul(curr.Ratio, parent.Ratio);
                }
                curr.Parent = root.Name;
                return root;
            }
            return curr;
        }
    }

    internal struct UnitPoint
    {
        public decimal x, w;
    }

    internal struct UnitTransform
    {
        public decimal m11, m12;
        public decimal m21, m22;
    }

    internal static class UnitMath
    {
        public static UnitTransform IdentityMat() => new UnitTransform
        {
            m11 = 1,
            m12 = 0,
            m21 = 0,
            m22 = 1
        };

        public static UnitPoint Mul(UnitPoint point, UnitTransform mat)
        {
            return new UnitPoint
            {
                x = point.x * mat.m11 + point.w * mat.m21,
                w = point.x * mat.m12 + point.w * mat.m22
            };
        }

        public static UnitTransform Mul(UnitTransform matA, UnitTransform matB)
        {
            var prod = new UnitTransform();
            prod.m11 = matA.m11 * matB.m11 + matA.m12 * matB.m21;
            prod.m12 = matA.m11 * matB.m12 + matA.m12 * matB.m22;
            prod.m21 = matA.m21 * matB.m11 + matA.m22 * matB.m21;
            prod.m22 = matA.m21 * matB.m12 + matA.m22 * matB.m22;
            return prod;
        }

        public static UnitTransform Inv(UnitTransform mat)
        {
            var rdet = 1 / (mat.m11 * mat.m22 - mat.m12 * mat.m22);
            return new UnitTransform
            {
                m11 = rdet * mat.m22,
                m12 = -rdet * mat.m12,
                m21 = -rdet * mat.m21,
                m22 = rdet * mat.m11
            };
        }
    }
}
