namespace DataAccessLibrary
{
    public class Variable
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
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

        public Variable()
        {
            this.id = -1;
            this.name = "";
            this.value = "";
        }

        public Variable(string name, string value)
        {
            this.id = -1;
            this.name = name;
            this.value = value;
        }

        public Variable(int id, string name, string value)
        {
            this.id = id;
            this.name = name;
            this.value = value;
        }
    }
}