using System;
using System.Collections.Generic;
using System.Text;

namespace HRP.Models;
{
    public class Offer
    {
    public int OfferID { get; set; }
    public decimal ProposalSalary { get; set; }
    public string OfferState { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;

    //Foreign key
    public int ApplicationID { get; set; } = string.Empty;



}
