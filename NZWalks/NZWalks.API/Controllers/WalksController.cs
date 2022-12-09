using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {

            // Fetch Data from Db
            var walksDomain = await walkRepository.GetallAsync();


            // COnvert domainwalks t DTO Walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            // Return Response

            return Ok(walksDTO);

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            // Get Walk Domain object from Db
            var walkDomain = await walkRepository.GetAsync(id);

            // Convert Domain obj to DTO

            var walksDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            //Return Response

            return Ok(walksDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkASync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            // Convert DTO To Daomain obj

            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId,

            };

            // Pass domain obj to Repository
            walkDomain = await walkRepository.AddAsync(walkDomain);

            // Convert the Domain obj back to DTO

            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId,
            };

            // Send DTO response back to client

            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id,
            [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            // Convert DTO to Domain obj
            var walkDomain = new Models.Domain.Walk
            {
             
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId,
            };

            // Pass details to Repository - Get Domain obj in reponse (or null)

            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            // Handle Null (not found)

            if (walkDomain == null)
            {
                return NotFound();
            }
            
                // Convert back to Doamin to DTO
                var walkDTO = new Models.DTO.Walk
                {
                    Id = walkDomain.Id,
                    Length = walkDomain.Length,
                    Name = walkDomain.Name,
                    RegionId = walkDomain.RegionId,
                    WalkDifficultyId = walkDomain.WalkDifficultyId,
                };
            return Ok(walkDTO);



            // Return response
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAysnc(Guid id)
        {
            // call Repo
            var walkDomain = await walkRepository.DeletecAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok();


        }

        #region Private Methods
        private bool ValidateAddWalkASync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Area),
                   $"{nameof(addWalkRequest)} cannot be empty");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name),
               $"{nameof(addWalkRequest.Name)} is required");
            }

            if (addWalkRequest.Length > 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length),
            $"{nameof(addWalkRequest.Length)} should be greater than zero");
            }

        }

        #endregion
    }
}
