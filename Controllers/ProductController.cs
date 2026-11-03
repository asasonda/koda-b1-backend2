using KodaStore.Api.Data;
using KodaStore.Api.DTOs;
using KodaStore.Api.Models;
using KodaStore.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace KodaStore.Api.Controllers;

[ApiController]
[Route("api/v1/products")]

public sealed class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<Products>> GetAll()
    {
        return Ok(_service.GetAll());
    }


    [HttpGet("{productCode}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]

    public ActionResult<ProductResponse> GetByCode([FromRoute] string productCode)
    {
        var product = _service.GetByCode(productCode);

        return product is null? Problem(statusCode: StatusCodes.Status404NotFound, title: "Data tidak ditemukan"): Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<ProductResponse> Create([FromBody] ProductRequest request)
    {
        try
        {
            var product = _service.Create(request);

            if(product is null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, title: "Data tidak dapat di dibuat");
            }

            return CreatedAtAction(
                nameof(GetByCode),
                new
                {
                    productCode = product.ProductCode
                },
                product
            );
        }
        catch (System.Exception e)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "invalid create data",
                detail: e.Message
            );
        }
    }

    [HttpDelete("{productCode}")]
    [ProducesResponseType(typeof(ProductResponse),StatusCodes.Status200OK )]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public IActionResult Delete([FromRoute] string productCode)
    {
        return _service.Delete(productCode) ? NoContent() : Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: "Gagal delete",
            detail: $"gagal {productCode}"
        );
    }

    [HttpPut("{productCode}")]
    [ProducesResponseType(typeof(ProductResponse),StatusCodes.Status200OK )]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<ProductResponse> Update([FromBody] UpdateProductRequest request, string productCode)
    {
        try
        {
            var existingProduct = _service.Update(productCode, request);

            return existingProduct is null ? Problem(statusCode: StatusCodes.Status404NotFound, title: "gagal update", detail: $"No product was found for code {productCode}") : Ok(existingProduct);
        }
        catch (System.Exception e)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "invalid update data",
                detail: e.Message
            );
        }
    }

}