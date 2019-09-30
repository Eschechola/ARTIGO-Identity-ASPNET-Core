using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{
    public class Usuario
    {
        [Required(ErrorMessage = "O login deve ser inserido.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "A senha deve ser inserida.")]
        public string Senha { get; set; }

    }
}
