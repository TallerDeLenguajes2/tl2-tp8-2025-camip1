using System.ComponentModel.DataAnnotations;
using tl2_tp8_2025_camip1.Models;

namespace tl2_tp8_2025_camip1.ViewModels
{
    public class PresupuestoViewModel
    {

        public PresupuestoViewModel()
        {
            // Fecha por defecto cada vez que se crea un nuevo PresupuestoViewModel
            FechaCreacion = DateTime.Today;
        }

        public PresupuestoViewModel(Presupuesto presupuesto)
        {
            IdPresupuesto = presupuesto.IdPresupuesto;
            NombreDestinatario = presupuesto.NombreDestinatario;
            FechaCreacion = presupuesto.FechaCreacion;
        }

        public int IdPresupuesto { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Destinatario")]
        //Opcional/alternativa: (Si se decide guardar Email valido el formato del mismo)
        //[EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string NombreDestinatario { get; set; }
        
        [Display(Name = "Fecha de Creación")]
        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        // Formatea el valor para mostrar solo fecha (sin hora).
        // Genera un input HTML5 de tipo <input type="date">, habilitando el calendario nativo.
        public DateTime FechaCreacion { get; set; } //Validacion en el controlador

    }
}