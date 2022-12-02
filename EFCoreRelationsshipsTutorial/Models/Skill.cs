using System.Text.Json.Serialization;

namespace EFCoreRelationsshipsTutorial.Models;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Damage { get; set; }
    [JsonIgnore]
    public List<Character> Characters { get; set; }
}