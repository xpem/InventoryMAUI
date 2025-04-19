using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Item
{
    public record ItemSituation
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public int Sequence { get; set; }
    }

    /// <summary>
    /// depois isso será passado para um parametro do item situation
    /// </summary>
    public static class OutSituationsIds
    {
        public const int ResaleStatusId = 5;
        public static int[] OutSituations = [4, 5, 3, 7];
    }
}
