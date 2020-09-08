namespace RedShot.Infrastructure.Uploaders.Ftp.Models
{
    /// <summary>
    /// FtpsEncryption enum.
    /// </summary>
    public enum FtpsEncryption
    {
        /// <summary>
        /// Connection starts in plain text and encryption is enabled with the AUTH command immediately after the server greeting.
        /// </summary>
        Explicit,

        /// <summary>
        /// Encryption is used from the start of the connection, port 990
        /// </summary>
        Implicit
    }
}
