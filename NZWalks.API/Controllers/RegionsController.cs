using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    //https://localhost:portnumber/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        // private object dbContext;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        //GET ALL Regions
        //https://localhost:portnumber/api/regions

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get Data From Database - Domain models
            var regionsDomain = await regionRepository.GetAllAsync();
            //Map Domain Models to DTOs
            /* var regionsDto = new List<RegionDto>();
             foreach (var regionDomain in regionsDomain)
             {
                 regionsDto.Add(new RegionDto()
                 {
                     Id = regionDomain.Id,
                     Code = regionDomain.Code,
                     Name = regionDomain.Name,
                     RegionImageUrl = regionDomain.RegionImageUrl
                 });
             }*/

            //Return DTOs

            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }

        //GET SINGLE REGION (Get region by ID)
        //GET https://localhost:portnumber/api/regions/{id}

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // var regionDomain = dbContext.Regions.Find(id);
            //Get Region Domain Model From Database

            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map/Convert Region Domain Model to Region DTO


            //Return DTO back to client
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        //POST To Create New Region
        //POST: https://localhost:portnumber/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map or convert DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
            //Use Domain Model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);
            //Map Domain model back to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDomainModel);
        }

        //Update region
        //PUT https://localhost:portnumber/api/regions/{id}

        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map DTO to domain model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
            //Check if region exists
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Convert Domain Model to DTO

            return Ok(mapper.Map<RegionDto>(regionDomainModel));

        }

        //Delete Region
        // DELETE: https://localhost:portnumber/api/regions/{id}

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            //return deleted Region back
            //map Domain Model to DTO

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }


    }
}
