using System.Collections.Generic;

namespace DiffingAPI.Models
{
    public class Diff
    {
        public int Id { get; set; }
        public int DiffId { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }
        
    }

    public class DiffData
    {
        public string data { get; set; }
    }
    public class DiffPart
    {
        public int Offset { get; set; }
        public int Length { get; set; }
    }
    public class DiffResult{
        public string diffResultType { get; set; }
        public List<DiffPart> diffs { get; set; }
    }
    

    
}