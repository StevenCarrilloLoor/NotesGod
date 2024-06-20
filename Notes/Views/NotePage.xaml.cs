using Notes.Models;
using System;
using System.IO;
using System.Text.Json;

namespace Notes.Views
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public partial class NotePage : ContentPage
    {
        // Propiedad pública ItemId para recibir el valor desde otra página
        public int ItemId { get; set; }

        public NotePage()
        {
            InitializeComponent();

            // Cargar una nota nueva si no se proporciona un ID de item.
            LoadNote();
        }

        private void LoadNote()
        {
            // Inicializar una nueva nota con valores predeterminados.
            Note noteModel = new Note
            {
                Id = 0, // El ID se establecerá cuando se guarde la nota.
                Name = "",
                Image = "",
                Price = 0M,
                CategoryId = 0,
                CategoryName = ""
            };

            BindingContext = noteModel;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (BindingContext is Note note)
            {
                string notesFolder = Path.Combine(FileSystem.AppDataDirectory, "notes");
                Directory.CreateDirectory(notesFolder); // Asegurarse de que el directorio existe.
                string noteFileName = Path.Combine(notesFolder, $"{note.Id}.json");

                string noteJson = JsonSerializer.Serialize(note);
                File.WriteAllText(noteFileName, noteJson);
            }

            await Shell.Current.GoToAsync("..");
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            if (BindingContext is Note note)
            {
                string noteFileName = Path.Combine(FileSystem.AppDataDirectory, "notes", $"{note.Id}.json");
                if (File.Exists(noteFileName))
                    File.Delete(noteFileName);
            }

            await Shell.Current.GoToAsync("..");
        }
    }
}
