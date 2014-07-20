
namespace kokos.Abstractions
{
    public class Symbol
    {
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public double? Bid { get; set; }
        public double? Ask { get; set; }
    }
}
