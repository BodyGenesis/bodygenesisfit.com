using System;

namespace BodyGenesis.Presentation.Website
{
    public class WebsiteOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public FileStorageOptions FileStorage { get; set; } = new FileStorageOptions();
        public string[] EmailRecipients { get; set; } = Array.Empty<string>();

        public class FileStorageOptions
        {
            public string Key { get; set; } = string.Empty;
            public string Secret { get; set; } = string.Empty;
            public string Endpoint { get; set; } = string.Empty;
            public string Region { get; set; } = string.Empty;
            public string Bucket { get; set; } = string.Empty;
        }
    }
}
