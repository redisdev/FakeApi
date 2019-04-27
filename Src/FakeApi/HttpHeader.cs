namespace FakeApi
{
    /// <summary>
    /// Represents a http web request header
    /// </summary>
    internal class HttpHeader
    {
        /// <summary>
        /// Gets or sets the name of the header
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the header
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }
}
