namespace EFCoreRelationsshipsTutorial
{
    public class AddWeaponDTO
    {
        public string? Name { get; set; }
        public int Damage { get; set; } = 10;
        public int CharacterId { get; set; }
    }
}
