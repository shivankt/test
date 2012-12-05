
namespace Common.Util.Models
{
    public class ComboBoxData
    {
        public string Text { get; set; }
        
        public string Value { get; set; }
        
        public bool IsSelected { get; set; }
        
        public ComboBoxData()
        {
            Text = string.Empty;
            Value = string.Empty;
        }
    }
}
