using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace HRP.Models;

public class Interview
{
    [Key]
    [JsonIgnore]
    public int InterviewID { get; set; }

    public DateTime InterviewDate { get; set; }

    public string InterviewType { get; set; } = string.Empty;

    public string InterviewStage { get; set; } = string.Empty;


    public string? Result_Offer { get; set; } = "notyet";

    // Foreign Key
    public int ApplicationID { get; set; }

    // Navigation Property
    [JsonIgnore]
    public Application? Application { get; set; } = null!;
}