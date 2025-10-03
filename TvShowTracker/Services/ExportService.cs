using TvShowTracker.Data;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace TvShowTracker.Services;

public class ExportService
{
    private readonly ApplicationDbContext _context;

    public ExportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public string ExportUserDataAsCsv(int userId)
    {
        var user = _context.Users.Find(userId);
        if (user == null)
            throw new Exception($"Utilizador com ID {userId} não encontrado.");

        var favorites = _context.Favorites
            .Where(f => f.UserId == userId)
            .Select(f => f.TvShow.Title)
            .ToList();

        var sb = new StringBuilder();
        sb.AppendLine("UserId,Email,Favorites");
        sb.AppendLine($"{user.Id},{user.Email},\"{string.Join(" | ", favorites)}\"");

        return sb.ToString();
    }

    public byte[] ExportUserDataAsPdf(int userId)
    {
        var user = _context.Users.Find(userId);
        if (user == null)
            throw new Exception($"Utilizador com ID {userId} não encontrado.");

        var favorites = _context.Favorites
            .Where(f => f.UserId == userId)
            .Select(f => f.TvShow.Title)
            .ToList();

        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Content().Column(col =>
                {
                    col.Item().Text($"📄 Exportação de Dados (RGPD)").FontSize(16).Bold().Underline();
                    col.Item().Text($"Utilizador: {user.Email}").FontSize(14).Bold();
                    col.Item().Text($"ID: {user.Id}");
                    col.Item().Text($"Data de exportação: {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC");

                    col.Item().Text("Séries favoritas:").FontSize(13).Underline();

                    foreach (var title in favorites)
                    {
                        col.Item().Text($"• {title}");
                    }

                    if (!favorites.Any())
                    {
                        col.Item().Text("— Sem favoritos registados —").Italic().FontColor(Colors.Grey.Darken2);
                    }
                });
            });
        });

        return pdf.GeneratePdf();
    }
}
