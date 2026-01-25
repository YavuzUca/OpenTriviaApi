using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Trivia.Models.DTO
{
    public class UserDto
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("address")]
        public AddressDto? Address { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("website")]
        public string? Website { get; set; }

        [JsonPropertyName("company")]
        public CompanyDto? Company { get; set; }
    }
    
    public class AddressDto
    {
        [Key]
        public int Id { get; set; }
        [JsonPropertyName("street")]
        public string? Street { get; set; }

        [JsonPropertyName("suite")]
        public string? Suite { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("zipcode")]
        public string? Zipcode { get; set; }

        [JsonPropertyName("geo")]
        public GeoDto? Geo { get; set; }
    }

    public class GeoDto
    {
        [Key]
        public int Id { get; set; }
        [JsonPropertyName("lat")]
        public string? Lat { get; set; }

        [JsonPropertyName("lng")]
        public string? Lng { get; set; }
    }

    public class CompanyDto
    {
        [Key]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("catchPhrase")]
        public string? CatchPhrase { get; set; }

        [JsonPropertyName("bs")]
        public string? Bs { get; set; }
    }

}
