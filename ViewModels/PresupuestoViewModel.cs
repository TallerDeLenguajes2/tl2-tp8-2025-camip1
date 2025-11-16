using System.ComponentModel.DataAnnotations;

namespace tl2_tp8_2025_camip1.ViewModels
{
    public class PresupuestoViewModel
    {
        public int IdPresupuesto { get; set; }

        [Required]
        [Display(Name = "Destinatario")]
        //Opcional/alternativa: (Si se decide guardar Email valido el formato del mismo)
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string NombreDestinatario { get; set; }
        
        [Display(Name = "Fecha de Creación")]
        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; } //Validacion en el controlador

    }
}