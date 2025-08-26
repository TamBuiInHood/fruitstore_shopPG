using AutoMapper;
using Contracts.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Presistance;
using Product.API.Repositories.Interfaces;
using Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        #region CRUD
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _repository.GetProducts();
            return Ok(result);
        }

        [HttpGet(RouteAPI.GetById)]
        public async Task<IActionResult> GetProduct([Required] long id)
        {
            var product = await _repository.GetProduct(id);
            if (product == null)
                return NotFound();
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            var productEnttiy = await _repository.GetProductByNo(productNo: productDto.No);
            if (productEnttiy != null)
                return BadRequest($"Product No: {productDto.No} is exist.");
            var product = _mapper.Map<CatalogProduct>(productDto);
            await _repository.CreateProduct(product);
            await _repository.SaveChangesAsync();
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        [HttpPut(RouteAPI.GetById)]
        public async Task<IActionResult> UpdateProduct([Required] long id, [FromBody] UpdateProductDto productDto)
        {
            var product = await _repository.GetProduct(id);
            if (product == null)
                return NotFound();

            var updateProduct = _mapper.Map(productDto, product);
            await _repository.UpdateProduct(updateProduct);
            await _repository.SaveChangesAsync();
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }


        [HttpDelete(RouteAPI.GetById)]
        public async Task<IActionResult> DeleteProduct([Required] long id, [FromBody] UpdateProductDto productDto)
        {
            var product = await _repository.GetProduct(id);
            if (product == null)
                return NotFound();

            var updateProduct = _mapper.Map(productDto, product);
            await _repository.UpdateProduct(updateProduct);
            await _repository.SaveChangesAsync();
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }
        #endregion

        #region Additional resource
        [HttpGet(RouteAPI.GetByNo)]
        public async Task<IActionResult> GetProductByNo([Required] string productNo)
        {
            var product = await _repository.GetProductByNo(productNo);
            if (product == null)
                return NotFound();

            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }
        #endregion
    }
}
