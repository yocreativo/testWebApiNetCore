﻿namespace WebApplication1.Data
{
    //Definición de la entity MarcasAutos
    public partial class SPMarcasAutos
    {
        public int IdMarca { get; set; }
        public string Marca { get; set; }
        public SPMarcasAutos()
        {
            Marca = string.Empty; // Se asigna un valor por defecto, por que el campo es requerido
        }
    }
    
}
