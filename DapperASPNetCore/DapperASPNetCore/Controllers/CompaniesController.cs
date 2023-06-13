using DapperASPNetCore.Contracts;
using DapperASPNetCore.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DapperASPNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        public CompaniesController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _companyRepository.GetCompanies();
            return Ok(companies);
        }

        [HttpGet("{id}", Name = "CompanyById")]
        public async Task<IActionResult> GetCompany(int id)
        {
            var company = await _companyRepository.GetCompany(id);
            if (company is null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            var createdCompany = await _companyRepository.CreateCompany(company);
            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyForUpdateDto company)
        {
            try
            {
                var dbCompany = await _companyRepository.GetCompany(id);
                if (dbCompany == null)
                    return NotFound();

                await _companyRepository.UpdateCompany(id, company);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var dbCompany = await _companyRepository.GetCompany(id);
            if (dbCompany is null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("ByEmployeeId/{id}")]
        public async Task<IActionResult> GetCompanyForEmployee(int id)
        {
            var company = await _companyRepository.GetCompanyByEmployeeId(id);
            if (company is null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [HttpGet("{id}/MultipleResult")]
        public async Task<IActionResult> GetMultipleResults(int id)
        {
            var company = await _companyRepository.GetMultipleResults(id);
            if (company is null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [HttpGet("MultipleMapping")]
        public async Task<IActionResult> GetMultipleMapping()
        {
            var companies = await _companyRepository.MultipleMapping();

            return Ok(companies);
        }
    }
}
