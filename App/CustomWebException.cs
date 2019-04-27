namespace App
{
    public class CustomWebException: System.Exception
    {
        public CustomWebException(string message)
            :base(message)
        {
        }
    }
}
