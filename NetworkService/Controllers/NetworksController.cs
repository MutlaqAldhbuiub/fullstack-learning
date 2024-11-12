namespace NetworkService.controllers;
using NetworkService.Repository;
using Microsoft.AspNetCore.Mvc;
using NetworkService.Models;

[ApiController]
[Route("api/[controller]")]
public class NetworksController : ControllerBase
{

    private readonly INetworkRepo _networkRepository;


    public NetworksController(INetworkRepo networkRepository)
    {
        _networkRepository = networkRepository;
    }


    [HttpGet]
    public async Task<ActionResult> getAllNetworks()
    {
        var result = await _networkRepository.GetAllNetworks();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> getNetworkById(int id)
    {
        var result = await _networkRepository.getNetworkById(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> addNetwork(Network network)
    {
        await _networkRepository.addNetwork(network);
        return Ok(network);
    }

    [HttpDelete]
    public async Task<ActionResult> deleteNetwork(int id)
    {
        await _networkRepository.deleteNetwork(id);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> changeNetworkStatus(int id, bool status)
    {
        await _networkRepository.changeNetworkStatus(id, status);
        return Ok();
    }

}