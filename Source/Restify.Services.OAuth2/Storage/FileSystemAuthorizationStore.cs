namespace JanHafner.Restify.Services.OAuth2.Storage
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Cryptography;
    using Configuration;
    using JetBrains.Annotations;

    /// <summary>
    /// Stores and restores the <see cref="AuthorizationState"/> from a file in the filesystem.
    /// Uses Data Protection API to encrypt and decrypt the saved file for the current user account.
    /// </summary>
    public sealed class FileSystemAuthorizationStore : IAuthorizationStore
    {
        [NotNull]
        private readonly IAuthorizationContextConfiguration configuration;

        [NotNull]
        private readonly IFormatter serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="IAuthorizationStore"/>.
        /// </summary>fg
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="configuration"/>' cannot be null. </exception>
        public FileSystemAuthorizationStore(IAuthorizationContextConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.configuration = configuration;
            this.serializer = new BinaryFormatter();
        }

        /// <summary>
        /// Tries to restore the <see cref="AuthorizationState"/>.
        /// </summary>
        /// <param name="authorizationState">The <see cref="AuthorizationState"/>.</param>
        /// <returns>A value indicating if the restore was successful.</returns>
        public Boolean TryRestoreAuthorization(out AuthorizationState authorizationState)
        {
            try
            {
                using (var fileStream = new FileStream(this.configuration.FileSystemAuthorizationStoreConfiguration.FilePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    var buffer = new Byte[fileStream.Length];
                    fileStream.Read(buffer, 0, (Int32) fileStream.Length);

                    var unprotectedData = ProtectedData.Unprotect(buffer, new Byte[0], DataProtectionScope.CurrentUser);

                    using (var memoryStream = new MemoryStream(unprotectedData))
                    {
                        authorizationState = (AuthorizationState) this.serializer.Deserialize(memoryStream);
                    }
                }

                return true;
            }
            catch
            {
                authorizationState = null;
                return false;
            }
        }

        /// <summary>
        /// Stores the <see cref="AuthorizationState"/>.
        /// </summary>
        /// <param name="authorizationState">The <see cref="AuthorizationState"/> to store.</param>
        public void StoreAuthorization(AuthorizationState authorizationState)
        {
            using (var fileStream = new FileStream(this.configuration.FileSystemAuthorizationStoreConfiguration.FilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                using (var memoryStream = new MemoryStream())
                {
                    this.serializer.Serialize(memoryStream, authorizationState);
                    memoryStream.Position = 0;
                    var buffer = new Byte[memoryStream.Length];
                    memoryStream.Read(buffer, 0, (Int32) memoryStream.Length);

                    var protectedData = ProtectedData.Protect(buffer, new Byte[0], DataProtectionScope.CurrentUser);

                    fileStream.Write(protectedData, 0, protectedData.Length);
                }
            }

            File.SetAttributes(this.configuration.FileSystemAuthorizationStoreConfiguration.FilePath, FileAttributes.Encrypted | FileAttributes.NotContentIndexed | FileAttributes.Hidden);
        }

        /// <summary>
        /// If necessary, clear all stored information about the authorization.
        /// </summary>
        public void ClearStorage()
        {
            var filePath = this.configuration.FileSystemAuthorizationStoreConfiguration.FilePath;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}