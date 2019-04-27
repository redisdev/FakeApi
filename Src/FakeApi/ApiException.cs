namespace FakeApi
{
    internal class ApiException
    {
        public string FullTypeName { get; set; }

        public object[] ConstructorArgs { get; set; }
    }
}
