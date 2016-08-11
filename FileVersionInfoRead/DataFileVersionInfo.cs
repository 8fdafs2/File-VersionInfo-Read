
namespace FileVersionRead
{
    public class DataFileVersionInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public DataFileVersionInfo(int ID, string Name, string Value)
        {
            this.ID = ID;
            this.Name = Name;
            this.Value = Value;
        }
    }
}
