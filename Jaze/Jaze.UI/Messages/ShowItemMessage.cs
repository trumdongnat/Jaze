namespace Jaze.UI.Messages
{
    public class ShowItemMessage
    {
        public object Item { get; set; }

        public ShowItemMessage(object item)
        {
            Item = item;
        }
    }
}