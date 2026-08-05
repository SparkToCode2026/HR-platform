using System;
using System.Collections.Generic;
using System.Text;

namespace HRP.Models;

public class Interview
{
    public int InterviewID { get; set; }

    public DateTime InterviewDate { get; set; }

    public string InterviewType { get; set; } = string.Empty;

    public string InterviewStage { get; set; } = string.Empty;

    public string? Result_Offer { get; set; }

    // Foreign Key
    public int ApplicationID { get; set; }

    // Navigation Property
    public Application Application { get; set; } = null!;
}