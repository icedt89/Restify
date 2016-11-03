namespace JanHafner.Restify.Services.OAuth2.UserInteractive.WindowsForms
{
    using System;

    /// <summary>
    /// Uses a Window and user input to retrieve the access code.
    /// </summary>
    public sealed class WindowsFormsAccessCodeHandler : IAccessCodeHandler
    {
        /// <summary>
        /// Gets the access code.
        /// </summary>
        /// <param name="authorizationUri">
        /// The Uri from which the access code is to retrieve.
        /// </param>
        /// <returns>
        /// The access code.
        /// </returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="authorizationUri"/>' cannot be null. </exception>
        public string GetAccessCode(Uri authorizationUri)
        {
            if (authorizationUri == null)
            {
                throw new ArgumentNullException(nameof(authorizationUri));
            }

            using (var webBrowserDialog = new AccessCodeHandlerDialog())
            {
                webBrowserDialog.webBrowser1.Url = authorizationUri;
                webBrowserDialog.ShowDialog();

                var documentTitle = webBrowserDialog.webBrowser1.DocumentTitle;
                var indexOfAccessCode = documentTitle.IndexOf("code=", StringComparison.InvariantCultureIgnoreCase);
                if (indexOfAccessCode > -1)
                {
                    return documentTitle.Remove(0, indexOfAccessCode + 5);
                }

                return null;
            }
        }
    }
}