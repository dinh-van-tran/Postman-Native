namespace DataAccessLibrary
{
    public class Parameter
    {
        private readonly int id;
        public int Id
        {
            get { return id; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string value;
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public Parameter()
        {

        }

        public Parameter(string name, string value)
        {
            this.id = -1;
            this.name = name;
            this.value = value;
        }

        public Parameter(int id, string name, string value)
        {
            this.id = id;
            this.name = name;
            this.value = value;
        }
    }
}