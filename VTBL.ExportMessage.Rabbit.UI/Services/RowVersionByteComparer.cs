using System.Collections;
using System.Collections.Generic;

namespace VTBL.ExportMessage.Rabbit.UI.Services
{
    /// <summary>
    /// Сравнение SQL Server rowversion/timestamp (<see cref="byte[]"/>), порядок как у BINARY(8).
    /// </summary>
    internal sealed class RowVersionByteComparer : IComparer<byte[]>
    {
        public static RowVersionByteComparer Instance { get; } = new RowVersionByteComparer();

        private RowVersionByteComparer()
        {
        }

        public int Compare(byte[] x, byte[] y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            var xEmpty = x == null || x.Length == 0;
            var yEmpty = y == null || y.Length == 0;

            if (xEmpty && yEmpty)
            {
                return 0;
            }

            if (xEmpty)
            {
                return -1;
            }

            if (yEmpty)
            {
                return 1;
            }

            return StructuralComparisons.StructuralComparer.Compare(x, y);
        }
    }
}
