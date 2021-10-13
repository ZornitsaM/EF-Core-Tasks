namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Console.WriteLine(ExportAlbumsInfo(context,9));
            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var sb = new StringBuilder();

            var albums = context.Albums.Where(x => x.ProducerId == producerId)
                         .ToList()
                         .Select(a => new
                         {
                             AlbumName = a.Name,
                             ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                             ProducerName = a.Producer.Name,
                             AlbumSongs = a.Songs.Select(s => new
                             {
                                 SongName = s.Name,
                                 SongPrice = s.Price,
                                 SongWriterName = s.Writer.Name
                             })
                                .OrderByDescending(s => s.SongName)
                                .ThenBy(s => s.SongWriterName)
                                .ToList(),

                             TotalPrice = a.Price,
                         })
                         .OrderByDescending(a => a.TotalPrice)
                         .ToList();


            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                int count = 1;
                sb.AppendLine("-Songs:");

                foreach (var song in album.AlbumSongs)
                {
                    sb.AppendLine($"---#{count}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.SongPrice:f2}");
                    sb.AppendLine($"---Writer: {song.SongWriterName}");
                    count++;
                }

                sb.AppendLine($"-AlbumPrice: {album.TotalPrice:f2}");
            }

            return sb.ToString().TrimEnd();

        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {

            var sb = new StringBuilder();

            var songs = context.Songs.ToList().Where(x => x.Duration.TotalSeconds > duration)
                        .Select(x => new
                        {
                            SongName = x.Name,
                            PerformersFullName = x.SongPerformers
                            .Select(sp=>sp.Performer.FirstName+" "+sp.Performer.LastName)
                            .FirstOrDefault(),
                            WriterName = x.Writer.Name,
                            AlbumProducer = x.Album.Producer.Name,
                            TimeDuration = x.Duration
                        })
                        .OrderBy(x => x.SongName)
                        .ThenBy(x => x.WriterName)
                        .ThenBy(x => x.PerformersFullName)
                        .ToList();


            int count = 1;
            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{count}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.WriterName}");
                sb.AppendLine($"---Performer: {song.PerformersFullName}");
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.TimeDuration}");
                count++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}
