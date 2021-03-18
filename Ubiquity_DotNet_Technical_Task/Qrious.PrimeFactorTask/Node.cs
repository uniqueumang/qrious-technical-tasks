using System;

namespace Qrious.PrimeFactorTask
{
    public class Node
    {
        private const uint MaxHeight = 11;
        private const uint MaxBaseValue = 9;
        private readonly ulong _data;
        private readonly Node _left;
        private readonly Node _right;

        public Node(ulong root) : this(root, root, 0)
        {
        }

        private Node(ulong root, ulong baseValue, int height)
        {
            _data = root;
            _left = null;
            _right = null;

            if (height >= MaxHeight) return;

            const int i = 10;
            var placeValueShifter = root * i;
            var nextHeight = height + 1;
            var leftRoot = placeValueShifter + baseValue;
            var rightRoot = leftRoot + 1;

            _left = new Node(leftRoot, baseValue, nextHeight);

            if (baseValue < MaxBaseValue)
            {
                _right = new Node(rightRoot, baseValue + 1, nextHeight);
            }
        }

        public ulong[] GetDataOfAllLeaf()
        {
            if (_left == null)
            {
                return new[] {_data};
            }

            var leftArray = _left.GetDataOfAllLeaf();

            if (_right == null)
            {
                return leftArray;
            }

            var rightArray = _right.GetDataOfAllLeaf();
            Array.Resize(ref leftArray, leftArray.Length + rightArray.Length);
            Array.Copy(rightArray, 0, leftArray, leftArray.Length - rightArray.Length, rightArray.Length);

            return leftArray;
        }
    }
}