using Newtonsoft.Json;

namespace metronic_extensions_api.Services.Models
{
    public class UserResponse
    {
        public int start { get; set; }

        public int limit { get; set; }

        public int length { get; set; }

        [JsonProperty("users")]
        public List<UserModel>? data { get; set; }
        public int total { get; set; }

        public Pagination pagination { get; set; }
    }

    public class Link
    {
        public int page { get; set; }

        public string label { get; set; }
    }

    public class Pagination
    {
        public int page { get; set; }

        public int items_per_page { get; set; }

        public List<Link> links { get; set; }
    }
    public class Identity
    {
        public string? provider { get; set; }

        public string? expires_in { get; set; }
        public string? connection { get; set; }
        public bool isSocial { get; set; }
        public string? user_id { get; set; }


    }
    public class UserModel
    {
            public DateTime created_at { get; set; }

            public string? email { get; set; }

            public bool email_verified { get; set; }
            public string? family_name { get; set; }
            public string? given_name { get; set; }
            public List<Identity>? identities { get; set; }
            public string? locale { get; set; }
            public string? name { get; set; }
            public string? nickname { get; set; }

            public string? picture { get; set; }
            public DateTime updated_at { get; set; }
            public string? user_id { get; set; }
            public DateTime last_login { get; set; }

            public string? last_ip { get; set; }

            public int logins_count { get; set; }
    }
}
