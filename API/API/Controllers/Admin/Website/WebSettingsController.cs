using API.DTOs;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin.Website
{
    public class WebSettingsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IWebSettingServices _webSettingServices;
        public WebSettingsController(IMapper mapper, IWebSettingServices webSettingServices) 
        {
            _mapper = mapper;
            _webSettingServices = webSettingServices;
        }

        [HttpPost("create-slide")]
        public async Task<ActionResult<SlideImageDTO>> CreateSlide(SlideImageDTO slideImageDTO)
        {
            if(slideImageDTO == null) return BadRequest(new ApiResponse(400, "Problem creating slide"));

            var slideToCreate = _mapper.Map<SlideImageDTO, SlideImage>(slideImageDTO);

            var slideCreated = await _webSettingServices.CreateSlide(slideToCreate);

            if (slideCreated == null) return BadRequest("Error create slide");

            return Ok(_mapper.Map<SlideImage,SlideImageDTO>(slideCreated));
        }

        [HttpPut("update-slide")]
        public async Task<ActionResult> UpdateSlide(SlideImageDTO slideImageDTO)
        {
            if(slideImageDTO == null) return BadRequest();

            SlideImage slideToUpdate = _mapper.Map<SlideImageDTO,SlideImage>(slideImageDTO);

            SlideImage slideUpdateResult = await _webSettingServices.UpdateSlide(slideToUpdate);

            if (slideUpdateResult == null) return BadRequest("Error update slide");

            return Ok();
        }

        [HttpGet("get-slides")]
        public async Task<ActionResult<IReadOnlyList<SlideImageDTO>>> GetSlides()
        {
            var slides = await _webSettingServices.GetSlides();

            return Ok(_mapper.Map<IReadOnlyList<SlideImage>, IReadOnlyList<SlideImageDTO>>(slides));
        }
    }
}