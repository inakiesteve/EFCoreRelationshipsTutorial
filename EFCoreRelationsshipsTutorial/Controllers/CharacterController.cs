using EFCoreRelationsshipsTutorial.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreRelationsshipsTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public CharacterController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Character>>> Get(int userId)
        {
            var characters = await _dataContext.Characters.Where(c => c.UserId == userId)
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .ToListAsync();

            return characters;
        }

        [HttpPost]
        public async Task<ActionResult<List<Character>>> Create(CreateCharacterDTO characterDto)
        {
            var user = await _dataContext.Users.FindAsync(characterDto.UserId);

            if (user is null)
            {
                return NotFound();
            }

            Character character = new Character
            {
                Name = characterDto.Name,
                RpgClass = characterDto.RpgClass,
                User = user

            };

            _dataContext.Characters.Add(character);
            await _dataContext.SaveChangesAsync();

            return await Get(character.UserId);
        }

        [HttpPost("Weapon")]
        public async Task<ActionResult<Character>> AddWeapon(AddWeaponDTO addWeaponDto)
        {
            var character = await _dataContext.Characters.FindAsync(addWeaponDto.CharacterId);

            if (character is null)
            {
                return NotFound();
            }

            Weapon weapon = new Weapon
            {
                Name = addWeaponDto.Name,
                Damage = addWeaponDto.Damage,
                Character = character,
                CharacterId = addWeaponDto.CharacterId

            };

            _dataContext.Weapons.Add(weapon);
            await _dataContext.SaveChangesAsync();

            return character;
        }

        [HttpPost("Skill")]
        public async Task<ActionResult<Character>> AddCharacterSkill(AddCharacterSkillDto addCharacterSkillDto)
        {
            var character = await _dataContext.Characters.Where(c => c.Id == addCharacterSkillDto.CharacterId).
                Include(c => c.Skills).FirstOrDefaultAsync();
            
            if (character is null)
            {
                return NotFound();
            }

            var skill = await _dataContext.Skills.FindAsync(addCharacterSkillDto.SkillId);

            if (skill is null)
            {
                return NotFound();
            }

            character.Skills.Add(skill);

            await _dataContext.SaveChangesAsync();

            return character;
        }
    }
}
