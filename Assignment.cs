namespace Deadline
{
    public struct Assignment
    {
        public string name;
        public string deadline;

        public Assignment(string name, string deadline) 
        {
            this.name = name;
            this.deadline = deadline;
        }

        public override string ToString()
        {
            return $"{name} - {deadline}";
        }
    }
}
