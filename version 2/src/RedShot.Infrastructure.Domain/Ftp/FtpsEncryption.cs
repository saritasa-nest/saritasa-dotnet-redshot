namespace RedShot.Infrastructure.Domain.Ftp
{
    /// <summary>
    /// FtpsEncryption enum.
    /// </summary>
    public enum FtpsEncryption
    {
        /// <summary>
        /// Connection starts in plain text and encryption is enabled with the AUTH command immediately after the server greeting.
        /// </summary>
        Explicit = 0,

        /// <summary>
        /// Encryption is used from the start of the connection, port 990
        /// </summary>
        Implicit = 1
    }
}
