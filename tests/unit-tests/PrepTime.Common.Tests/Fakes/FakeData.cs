namespace PrepTime.Common.Tests.Fakes
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class FakeData
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public int Id { get; set; } = int.MinValue;

        public string Name { get; set; } = string.Empty;

        public FakeData()
        {
        }

        public FakeData(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public FakeData(int id)
            : this(id, string.Empty)
        {
        }

        public FakeData(string name)
            : this(int.MinValue, name)
        {
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not FakeData data)
            {
                return false;
            }

            return Id == data.Id && Name == data.Name;
        }
    }
}
