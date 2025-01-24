using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skinet_Store.Core.Entities;
using Skinet_Store.Core.Interfaces;
using Skinet_Store.Core.Specification;
using Skinet_Store.RequestHelpers;


namespace Skinet_Store.Controller
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json", "application/xml")]
	[Consumes("application/json", "application/xml")]
	public class ProductController : ControllerBase
	{
		//private readonly IProductRepository _productRepository;
		private readonly IGenericRepository<Product> _productRepository;
		private readonly ILogger<ProductController> _logger;
		private readonly IProductSpecificationFactory _specificationFactory;

		public ProductController(IGenericRepository<Product> productRepository,
								 ILogger<ProductController> logger,
								 IProductSpecificationFactory specificationFactory)
		{
			_productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_specificationFactory = specificationFactory ?? throw new ArgumentNullException(nameof(specificationFactory));
		}

		[HttpGet(Name = nameof(GetAllProducts))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<IReadOnlyList<Product>>> GetAllProducts([FromQuery] ProductSpecificationParameters specificationParameters)
		{

			try
			{
				var spec = _specificationFactory.Create(specificationParameters);
				var productList = await _productRepository.GetAllWithSpecAsync(spec);
				var count =  await _productRepository.CountAsync(spec);
				var pagination = new Pagination<Product>(specificationParameters.PageIndex,
														specificationParameters.PageSize, 
														count, 
														productList);


			
				return Ok(pagination);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"An Error Occurred While Retrieving The Products Info {nameof(GetAllProducts)}");
				return StatusCode(500);
			}
		}

		[HttpGet("{productId:int}", Name = nameof(GetProductById))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Product>> GetProductById([FromRoute] int productId)
		{
			var product = await _productRepository.GetByIdAsync(productId);

			if (product == null)
			{
				return NotFound();
			}

			return Ok(product);
		}


		[HttpPost(Name = nameof(CreateNewProduct))]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]

		public async Task<IActionResult> CreateNewProduct([FromBody] Product product)
		{
			if (!ModelState.IsValid)
			{
				_logger.LogError($"Invalid POST attempt in {nameof(CreateNewProduct)}");
				return BadRequest(ModelState);
			}
			try
			{
				if (CreateNewProduct == null) return BadRequest(ModelState);

				var createdRestauratns = await _productRepository.CreateAsync(product);

				return StatusCode(201, createdRestauratns);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"An error occurred while creating and adding new Restaurant {nameof(CreateNewProduct)}.");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpDelete("{productId:int}", Name = nameof(DeleteProduct))]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> DeleteProduct([FromRoute] int productId)
		{
			try
			{
				var getProduct = await _productRepository.GetByIdAsync(productId);

				var isDeleted = await _productRepository.DeleteAsync(getProduct);
				if (!isDeleted)
				{
					_logger.LogInformation("Product with ID {.DeleteRestaurantAsync(ProductId)} not found.", productId);
					return NotFound($"Product with ID {productId} not found.");
				}
				_logger.LogInformation("Product with ID {RestaurantId} successfully deleted.", productId);
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while deleting the Product with ID {ProductId}.", productId);
				return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
			}
		}
		[HttpPut(Name = nameof(UpdateProduct))]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> UpdateProduct(Product product)
		{
			if (!ModelState.IsValid)
			{
				_logger.LogError($"Invalid Update attempt in {nameof(UpdateProduct)}");
				return BadRequest(ModelState);
			}
			try
			{
				if (product == null) return BadRequest(ModelState);
				var updatedProduct = await _productRepository.UpdateAsync(product);
				return StatusCode(201, updatedProduct);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"An error occurred while updating an exisitng Product {nameof(UpdateProduct)}.");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet("types", Name = nameof(GetProductTypes))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes()
		{
			try
			{
				var spec = new TypeListSpecification();
				var productTypesList = await _productRepository.GetAllWithSpecAsync<string>(spec);

				return Ok(productTypesList);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"An Error Occurred While Retreiveing The Types Info {nameof(GetProductTypes)}");
				return StatusCode(500);
			}
		}

		[HttpGet("brands", Name = nameof(GetProductBrands))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands()
		{
			try
			{
				var spec = new BrandListSpecification();
				var productBrandsList = await _productRepository.GetAllWithSpecAsync<string>(spec);

				return Ok(productBrandsList);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"An Error Occurred While Retreiveing The Brands Info {nameof(GetProductBrands)}");
				return StatusCode(500);
			}
		}

		private bool ProductExists(int id)
		{
			return _productRepository.EntityExistsAsync(id).Result;
		}
	}

}
