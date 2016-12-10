using System;
using MailChimp.Net.Interfaces;

namespace MailChimp.Net
{
    public class MailChimpConfiguration : IMailChimpConfiguration
    {
        public static int DefaultLimit { get { return Common.DefaultLimit; } }

        private string _apiKey;
        public string ApiKey
        {
            get
            {
                if (_apiKey == null)
                {
                    throw new InvalidOperationException("MailChimp API key is not set.  MailChimpApiKey in the configuration file is no longer supported - you must set it at runtime.");
                }

                return _apiKey;
            }
            set { _apiKey = value; }
        }

        public int Limit { get; set; } = DefaultLimit;

        public string DataCenter
            => string.IsNullOrWhiteSpace(this.ApiKey)
                ? string.Empty
                : this.ApiKey.Substring(
                    this.ApiKey.LastIndexOf("-", StringComparison.Ordinal) + 1,
                    this.ApiKey.Length - this.ApiKey.LastIndexOf("-", StringComparison.Ordinal) - 1);

        public string AuthHeader => $"apikey {this.ApiKey}";
    }
}