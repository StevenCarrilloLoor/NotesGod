using Notes.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Notes.Models
{
    internal class AllNotes
    {
        public ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note>();

        public AllNotes() => LoadNotes();

        public void LoadNotes()
        {
            Notes.Clear();

            // Obtener la carpeta donde se almacenan las notas.
            string notesFolder = Path.Combine(FileSystem.AppDataDirectory, "notes");

            // Asegurarse de que el directorio existe.
            if (!Directory.Exists(notesFolder))
                Directory.CreateDirectory(notesFolder);

            // Usar Linq para cargar los archivos .json de las notas.
            var notes = Directory
                            .EnumerateFiles(notesFolder, "*.json")
                            .Select(file => JsonSerializer.Deserialize<Note>(File.ReadAllText(file)))
                            .Where(note => note != null)
                            .OrderBy(note => note.Id); // Ordenar por ID o cualquier otro criterio deseado.

            // Agregar cada nota a la colección ObservableCollection.
            foreach (var note in notes)
                Notes.Add(note);
        }
    }
}
