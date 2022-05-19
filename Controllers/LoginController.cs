using KSiwiak_Urzad_API.Data;
using KSiwiak_Urzad_API.Models;
using Microsoft.AspNetCore.Mvc;
using Urzad_KSiwiak.Models;

namespace KSiwiak_Urzad_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly UrzadDBContext _context;

        public LoginController(UrzadDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("PostKierownikLogin")]
        public ActionResult<Token> PostKierownikLogin([FromBody] Login login) //nie wchodzą dane :(
        {
            Konta_kierownikow konto = _context.Konta_kierownikow.Where(w => w.email == login.login && w.haslo == login.haslo).FirstOrDefault();

            if (konto != null)
            {

                var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                var resultToken = new string(
                   Enumerable.Repeat(allChar, 20)
                   .Select(token => token[random.Next(token.Length)]).ToArray());

                Kierownicy kierownicy = _context.Kierownicy.Find(konto.id_kierownika);

                if (kierownicy != null)
                {
                    string connectionStringLogin = login.login.Split("_")[0] + konto.id.ToString();
                    Token token = new Token { token = resultToken.ToString(), rola = "kierownik", userID = kierownicy.id, connectionString = "User Id="+ connectionStringLogin + ";Password=" + login.haslo + ";"};
                    return token;
                }
                else return NotFound();
            }
            else return NotFound();
        }

        [HttpPost]
        [Route("PostUrzednikLogin")]
        public ActionResult<Token> PostUrzednikLogin([FromBody] Login login)
        {
            Konta_urzednikow konto = _context.Konta_urzednikow.Where(w => w.login == login.login && w.haslo == login.haslo).FirstOrDefault();

            if (konto != null)
            {

                var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                var resultToken = new string(
                   Enumerable.Repeat(allChar, 20)
                   .Select(token => token[random.Next(token.Length)]).ToArray());

                Urzednicy urzednik = _context.Urzednicy.Find(konto.id_urzednika);

                if (urzednik != null)
                {
                    string connectionStringLogin = login.login.Split("_")[0] + konto.id.ToString();
                    Token token = new Token { token = resultToken.ToString(), rola = "urzednik", userID = urzednik.id, connectionString = "User Id=" + connectionStringLogin + ";Password=" + login.haslo + ";" };
                    return token;
                }
                else return NotFound();
            }
            else return NotFound();
        }

        [HttpPost]
        [Route("PostObywatelLogin")]
        public async Task<ActionResult<Token>> PostObywatelLogin([FromBody] Login login)
        {
            Konta_obywateli konto = _context.Konta_obywateli.Where(w => w.login == login.login && w.haslo == login.haslo).FirstOrDefault();

            if (konto != null)
            {
                var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                var resultToken = new string(
                   Enumerable.Repeat(allChar, 20)
                   .Select(token => token[random.Next(token.Length)]).ToArray());

                Obywatele obywatel = _context.Obywatele.Find(konto.id_obywatela);

                if (obywatel != null)
                {
                    string connectionStringLogin = login.login.Split("_")[0] + konto.id.ToString();
                    Token token = new Token { token = resultToken.ToString(), rola = "obywatel", userID = obywatel.id, connectionString = "User Id=" + connectionStringLogin + ";Password=" + login.haslo + ";" };
                    return token;
                }
                else return NotFound();
            }
            else return NotFound();
        }

        [HttpPost]
        [Route("PostAdministratorLogin")]
        public async Task<ActionResult<Token>> PostAdministratorLogin([FromBody] Login login)
        {
            if (login.login.Equals("administrator") && login.haslo.Equals("administrator"))
            {
                var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                var resultToken = new string(
                   Enumerable.Repeat(allChar, 20)
                   .Select(token => token[random.Next(token.Length)]).ToArray());

                string connectionStringLogin = "User Id=administrator;Password=administrator;";
                Token token = new Token { token = resultToken.ToString(), rola = "administrator", userID = 0, connectionString = connectionStringLogin };
                return token;
            }
            else return NotFound();
        }

        [HttpGet("CheckToken")]
        public async Task<ActionResult<Token>> CheckToken(string getToken)
        {
            byte[] tokenb = new Byte[20];
            byte[] rolab = new Byte[10];
            byte[] userIDb = new Byte[10];

            bool isToken = HttpContext.Session.TryGetValue("token", out tokenb);
            bool isrola = HttpContext.Session.TryGetValue("rola", out rolab);
            bool isUserID = HttpContext.Session.TryGetValue("userID", out userIDb);

            if (isToken || isrola || isUserID)
            {
                string token = HttpContext.Session.GetString("token");
                string rola = HttpContext.Session.GetString("rola");
                int userID = (int)HttpContext.Session.GetInt32("userID");

                if (token.Equals(getToken))
                {
                    return new Token { token = token, rola = rola, userID = userID };
                }
                return Forbid();
            }

            return NotFound();
        }

        [HttpGet("isLogged")]
        public async Task<ActionResult<Token>> isLogged()
        {
            byte[] tokenb = new Byte[30];
            byte[] rolab = new Byte[30];
            byte[] userIDb = new Byte[30];

            bool isToken = HttpContext.Session.TryGetValue("token", out tokenb);
            bool isrola = HttpContext.Session.TryGetValue("rola", out rolab);
            bool isUserID = HttpContext.Session.TryGetValue("userID", out userIDb);

            string token = HttpContext.Session.GetString("token");

            if (isToken || isrola || isUserID)
            {
                token = HttpContext.Session.GetString("token");
                string rola = HttpContext.Session.GetString("rola");
                int userID = (int)HttpContext.Session.GetInt32("userID");

                return new Token { token = token, rola = rola, userID = userID };

            }

            return NotFound();
        }

        [HttpGet("Logout")]
        public async Task<ActionResult<Token>> Logout()
        {
            HttpContext.Session.Clear();
            return NoContent();
        }
    }
}
