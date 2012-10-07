namespace Eirikb.SharePoint.Extreme.Questions
{
    public class Addition : IQuestion
    {
        #region IQuestion Members

        public int Level { get; set; }

        public bool Run(string line)
        {
            return line == "Hack";
        }

        #endregion
    }
}