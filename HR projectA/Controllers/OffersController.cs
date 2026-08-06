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
    // Case 2: Update salary and job title
    [HttpPut("{id}/details")]
    public async Task<IActionResult> UpdateOfferDetails(
        int id,
        UpdateOfferDetailsRequest request)
    {
        var offer = await _context.Offers.FindAsync(id);

        if (offer == null)
        {
            return NotFound("Offer not found.");
        }

        if (request.ProposalSalary <= 0)
        {
            return BadRequest("The proposed salary must be greater than zero.");
        }

        offer.ProposalSalary = request.ProposalSalary;
        offer.JobTitle = request.JobTitle;

        await _context.SaveChangesAsync();

        return Ok(offer);
    }

    // Case 3: Update offer state
    [HttpPatch("{id}/state")]
    public async Task<IActionResult> UpdateOfferState(
        int id,
        UpdateOfferStateRequest request)
    {
        var offer = await _context.Offers.FindAsync(id);

        if (offer == null)
        {
            return NotFound("Offer not found.");
        }

        offer.OfferState = request.OfferState;

        await _context.SaveChangesAsync();

        return Ok(offer);
    }




