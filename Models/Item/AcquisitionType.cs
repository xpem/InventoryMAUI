namespace Models.Item
{
    public record AcquisitionType
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public int Sequence { get; set; }
    }
}
