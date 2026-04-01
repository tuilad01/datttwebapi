// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace datttwebapi.Controllers
{
    [ApiVersion(1.0)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IEnumerable<User> users;
        private readonly IMapper _mapper;
        private readonly ApiVersioningDbContext _context;
        public UsersController(IMapper mapper, ApiVersioningDbContext context)
        {
            _mapper = mapper;
            _context = context;

            users = new string[] { "Dat", "Truong", "Tan" }.Select(user =>
            {
                return new User
                {
                    Id = Guid.NewGuid(),
                    Name = user,
                    Email = $"{user.ToLower()}@email.com",
                    Phone = "123456"
                };
            });
        }
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<IEnumerable<UserDto>> Get()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(_mapper.Map<UserDto>);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
