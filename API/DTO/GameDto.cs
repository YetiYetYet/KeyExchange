﻿using API.Models;

namespace API.DTO;

public class GameDto
{
    public UserDto User { get; set; }
    public bool IsAvailable { get; set; }
    public string Name { get; set; }
    public string Link { get; set; }
    public string Platforme { get; set; }
    public GameInfoFromPlatformDto GameInfoFromPlatform { get; set; }
    public string PublicComment { get; set; }
    public string AdminComment { get; set; }
    public DateTime ReceivedDate { get; set; }
    public DateTime GivenDate { get; set; }
    public string GivenTo { get; set; }
}