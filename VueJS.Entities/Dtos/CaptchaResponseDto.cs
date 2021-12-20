using Newtonsoft.Json;
using System.Collections.Generic;

namespace VueJS.Entities.Dtos
{
    public class CaptchaResponseDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}