using HRP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjectX.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OffersController : ControllerBase
{
    private readonly ProjectContext _context;

    public OffersController(ProjectContext context)
    {
        _context = context;
    }

    // Case 1: Create a new offer
    [HttpPost]
    public async Task<IActionResult> CreateOffer(CreateOfferRequest request)
    {
        var applicationExists = await _context.Applications
            .AnyAsync(a => a.ApplicationID == request.ApplicationID);

        if (!applicationExists)
        {
            return BadRequest("The application does not exist.");
        }

        var offerAlreadyExists = await _context.Offers
            .AnyAsync(o => o.ApplicationID == request.ApplicationID);

        if (offerAlreadyExists)
        {
            return BadRequest("An offer already exists for this application.");
        }

        if (request.ProposalSalary <= 0)
        {
            return BadRequest("The proposed salary must be greater than zero.");
        }

        var offer = new Offer
        {
            ProposalSalary = request.ProposalSalary,
            OfferState = request.OfferState,
            JobTitle = request.JobTitle,
            ApplicationID = request.ApplicationID
        };

        _context.Offers.Add(offer);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetOfferById),
            new { id = offer.OfferID },
            offer);
    }

