using System;
using Microsoft.EntityFrameworkCore;
using Music.Domain.Entities;
using Music.Domain.Associates;

namespace Music.Infrastructure.UnitsOfWork
{
    public class SecondGenUnitOfWork : DbContext, IUnitOfWork
    {
        public DbSet<ArtistAlbum> ArtistAlbums { get; set; }
        public DbSet<AlbumTrack> AlbumTracks { get; set; }
        public DbSet<ArtistTrack> ArtistTracks { get; set; }
        public DbSet<TrackGenre> TrackGenres { get; set; }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public SecondGenUnitOfWork(DbContextOptions<SecondGenUnitOfWork> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Artist>(artist =>
            {
                artist.HasKey("Id");
                artist.Property("Id").HasColumnName("artist_id");
                artist.Property("Name").HasColumnName("artist_name");
                artist.Property("Description").HasColumnName("artist_description");
                artist.Property("Country").HasColumnName("artist_country");
                artist.Property("YearActive").HasColumnName("artist_year_started");
                artist.Property(ar => ar.ConcurrencyStamp).ForNpgsqlHasColumnName("xmin").ForNpgsqlHasColumnType("xid").ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();
                artist.ToTable("artist");
            });

            builder.Entity<Album>(album =>
            {
                album.HasKey("Id");
                album.Property("Id").HasColumnName("album_id");
                album.Property("Title").HasColumnName("album_title");
                album.Property("RecordLabel").HasColumnName("album_label");
                album.Property("ReleaseDate").HasColumnName("album_release_date");
                album.ToTable("album");
            });

            builder.Entity<Track>(track =>
            {
                track.HasKey("Id");
                track.Property("Id").HasColumnName("track_id");
                track.Property("Title").HasColumnName("track_title");
                track.Property("Duration").HasColumnName("track_duration");
                track.Property("ReleaseDate").HasColumnName("track_release_date");
                track.ToTable("track");
            });

            builder.Entity<Genre>(genre =>
            {
                genre.HasKey("Id");
                genre.Property("Id").HasColumnName("genre_id");
                genre.Property("Type").HasColumnName("genre_type");
                genre.ToTable("genre");
            });

            builder.Entity<ArtistAlbum>(artist_album =>
            {
                artist_album.HasKey(aa => new { aa.ArtistId, aa.AlbumId });
                artist_album.Property("ArtistId").HasColumnName("artist_id");
                artist_album.Property("AlbumId").HasColumnName("album_id");
                artist_album.HasOne(aa => aa.Artist).WithMany(al => al.Albums).HasForeignKey(al => al.ArtistId);
                artist_album.HasOne(aa => aa.Album).WithMany(ar => ar.Artists).HasForeignKey(ar => ar.AlbumId);
                artist_album.ToTable("artist_album");
            });

            builder.Entity<AlbumTrack>(album_track =>
            {
                album_track.HasKey(at => new { at.AlbumId, at.TrackId });
                album_track.Property("AlbumId").HasColumnName("album_id");
                album_track.Property("TrackId").HasColumnName("track_id");
                album_track.HasOne(at => at.Album).WithMany(tr => tr.Tracks).HasForeignKey(al => al.AlbumId);
                album_track.HasOne(at => at.Track).WithMany(al => al.Albums).HasForeignKey(tr => tr.TrackId);
                album_track.ToTable("album_track");
            });

            builder.Entity<TrackGenre>(track_genre =>
            {
                track_genre.HasKey(tg => new { tg.TrackId, tg.GenreId });
                track_genre.Property("TrackId").HasColumnName("track_id");
                track_genre.Property("GenreId").HasColumnName("genre_id");
                track_genre.HasOne(tg => tg.Track).WithMany(ge => ge.Genres).HasForeignKey(tr => tr.TrackId);
                track_genre.HasOne(tg => tg.Genre).WithMany(tr => tr.Tracks).HasForeignKey(ge => ge.GenreId);
                track_genre.ToTable("track_genre");
            });

            builder.Entity<ArtistTrack>(artist_track =>
            {
                artist_track.HasKey(at => new { at.ArtistId, at.TrackId });
                artist_track.Property("ArtistId").HasColumnName("artist_id");
                artist_track.Property("TrackId").HasColumnName("track_id");
                artist_track.HasOne(at => at.Artist).WithMany(tr => tr.Tracks).HasForeignKey(ar => ar.ArtistId);
                artist_track.HasOne(at => at.Track).WithMany(ar => ar.Artists).HasForeignKey(tr => tr.TrackId);
                artist_track.ToTable("artist_track");
            });

            base.OnModelCreating(builder);
        }

        public void Commit()
        {
            SaveChanges();
        }

        public void Rollback()
        {
            Dispose();
        }
    }
}