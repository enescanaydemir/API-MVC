using Microsoft.AspNetCore.Mvc;
using example_api.Models;
using System.Text;
using Newtonsoft.Json;

namespace example_api.Controllers;

public class HomeController : Controller
{
    private readonly HttpClient _httpClient;

    public HomeController(IHttpClientFactory httpClientFactory) // HttpClientFactory sınıfından bir instance oluşturduk ve bu instance'ı httpClientFactory parametresi ile alıyoruz.
    {
        _httpClient = httpClientFactory.CreateClient("FakeStoreApi");
    }
    public async Task<IActionResult> Index()
    {
        HttpResponseMessage responseMessage = await _httpClient.GetAsync("products?limit=9");
        string responseContent = await responseMessage.Content.ReadAsStringAsync();

        ApiResponse? apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
        List<Product> product = apiResponse.Products;

        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"products/{id}");
        string responseContent = await responseMessage.Content.ReadAsStringAsync();

        ApiResponse? apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);

        var product = apiResponse.Product; //önceki hata; burda apiResponse.Products yazdığım için hata verdi. Tek ürün vermen gerek
        return View(product);
    }

    public async Task<IActionResult> GetCategory()
    {
        var responseMessage = await _httpClient.GetAsync("products/category");
        string responseContent = await responseMessage.Content.ReadAsStringAsync();
        var categoryResponse = JsonConvert.DeserializeObject<CategoryResponse>(responseContent);
        return View(categoryResponse.Categories);
    }

    public async Task<IActionResult> ProductsByCategory(string category)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"products/category?type={category}");
        string responseContent = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
        return View(apiResponse.Products);
    }


    public async Task<IActionResult> AddProduct()
    {
        var responseMessage = await _httpClient.GetAsync("products/category");

        var responseContent = await responseMessage.Content.ReadAsStringAsync();
        var categories = JsonConvert.DeserializeObject<CategoryResponse>(responseContent);


        ViewBag.Categories = categories.Categories;
        return View();

    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            var serializeProduct = JsonConvert.SerializeObject(product);
            HttpContent content = new StringContent(serializeProduct, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("products", content);
            var newProduct = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Product>(newProduct.Result);
            return View(result);
        }
        var responseMessage = await _httpClient.GetAsync("products/categories");
        var contentResponse = await responseMessage.Content.ReadAsStringAsync();
        var categories = JsonConvert.DeserializeObject<CategoryResponse>(contentResponse);

        ViewBag.Categories = categories.Categories;
        return View(product);
    }
}
