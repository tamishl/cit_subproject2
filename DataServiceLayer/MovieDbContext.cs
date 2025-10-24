using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServiceLayer.Domains;

namespace DataServiceLayer;

public class MovieDbContext : DbContext
{
    public DbSet<Title> Titles { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Casting> Castings { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Profession> Professions { get; set; }
    public DbSet<TitleType> TitleTypes { get; set; }
    public DbSet<TitleAka> TitleAkas { get; set; }
    public DbSet<Episode> Episodes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Search> Searches { get; set; }


    // connect to db
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // show full query details in console
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.EnableSensitiveDataLogging();

        // Remember to set the environment variable PG_PASSWORD before running the application
        string password = Environment.GetEnvironmentVariable("PG_PASSWORD");
        string connectionString = $"host=localhost;db=imdb_new;uid=postgres;pwd={password}";
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // map Casting to casting   
        modelBuilder.Entity<Casting>().ToTable("casting");
        modelBuilder.Entity<Casting>().Property(c => c.TitleId).HasColumnName("title_id");
        modelBuilder.Entity<Casting>().Property(c => c.PersonId).HasColumnName("person_id");
        modelBuilder.Entity<Casting>().Property(c => c.CharacterName).HasColumnName("character_name");
        modelBuilder.Entity<Casting>().Property(c => c.Job).HasColumnName("job");
        modelBuilder.Entity<Casting>().Property(c => c.Ordering).HasColumnName("ordering");
        modelBuilder.Entity<Casting>().HasKey(c => new { c.TitleId, c.PersonId, c.Ordering });// composite PK

        //map category to Profession
        modelBuilder.Entity<Casting>().HasOne(c => c.Category) 
                                      .WithMany()
                                      .HasForeignKey("profession_id");

        // map Genre to genre
        modelBuilder.Entity<Genre>().ToTable("genre");
        modelBuilder.Entity<Genre>().Property(g => g.Id).HasColumnName("genre_id");

        // map Person to person
        modelBuilder.Entity<Person>().ToTable("person");
        modelBuilder.Entity<Person>().Property(p => p.Id).HasColumnName("person_id");
        modelBuilder.Entity<Person>().Property(p => p.Name).HasColumnName("name");
        modelBuilder.Entity<Person>().Property(p => p.BirthYear).HasColumnName("birth_year");
        modelBuilder.Entity<Person>().Property(p => p.DeathYear).HasColumnName("death_year");
        modelBuilder.Entity<Person>()

        // map Person to Casting
                            .HasMany(p => p.Castings)
                            .WithOne(c => c.Person)
                            .HasForeignKey("person_id");

        // map Person to KnownFor. I haven't created a class for the join table(person_notable_title) since it only contains the two FKs
        modelBuilder.Entity<Person>()
                            .HasMany(p => p.KnownFor)
                            .WithMany(t => t.KnownForPersons)
                            .UsingEntity(j => j.ToTable("person_notable_title")
                            .HasData());
        // map Person to Profession. I haven't created a class for the join table(person_profession) since it only contains the two FKs
        modelBuilder.Entity<Person>().HasMany(p => p.Professions)
                            .WithMany()
                            .UsingEntity(j => j.ToTable("person_profession")
                            .HasData());

        // map Profession to profession
        modelBuilder.Entity<Profession>().ToTable("profession");
        modelBuilder.Entity<Profession>().Property(pr => pr.Id).HasColumnName("profession_id");


        //map Title to title
        modelBuilder.Entity<Title>().ToTable("title");
        modelBuilder.Entity<Title>().Property(t => t.Id).HasColumnName("title_id");
        modelBuilder.Entity<Title>().Property(t => t.PrimaryTitle).HasColumnName("primary_title");
        modelBuilder.Entity<Title>().Property(t => t.OriginalTitle).HasColumnName("original_title");
        modelBuilder.Entity<Title>().Property(t => t.IsAdult).HasColumnName("is_adult");
        modelBuilder.Entity<Title>().Property(t => t.StartYear).HasColumnName("start_year");
        modelBuilder.Entity<Title>().Property(t => t.EndYear).HasColumnName("end_year");
        modelBuilder.Entity<Title>().Property(t => t.RuntimeMinutes).HasColumnName("runtime_minutes");
        modelBuilder.Entity<Title>().Property(t => t.Plot).HasColumnName("plot");
        modelBuilder.Entity<Title>().Property(t => t.Poster).HasColumnName("poster");

        // map Title to Casting
        modelBuilder.Entity<Title>()
                            .HasMany(t => t.Cast)
                            .WithOne(c => c.Title)
                            .HasForeignKey("title_id");

        //map Title to Director. I haven't created a class for the join table(title_director) since it only contains the two FKs
        modelBuilder.Entity<Title>()
                            .HasMany(t => t.Directors)
                            .WithMany(p => p.DirectorOf)
                            .UsingEntity(j => j.ToTable("title_director")
                            .HasData());

        //map Title to Writer. I haven't created a class for the join table(title_writer) since it only contains the two FKs
        modelBuilder.Entity<Title>()
                            .HasMany(t => t.Writers)
                            .WithMany(p => p.WriterOf)
                            .UsingEntity(j => j.ToTable("title_writer")
                            .HasData());

        //map Title to Genre
        modelBuilder.Entity<Title>()
                            .HasMany(t => t.Genres)
                            .WithMany(g => g.Titles)
                            .UsingEntity(j => j.ToTable("title_genre")
                            .HasData());

        //map Title to TitleType
        modelBuilder.Entity<Title>()
                            .HasOne(t => t.Type)
                            .WithMany(tt => tt.Titles)
                            .HasForeignKey("title_type_id");

        //map TitleType to title_type
        modelBuilder.Entity<TitleType>().ToTable("title_type");
        modelBuilder.Entity<TitleType>().Property(tt => tt.Id).HasColumnName("title_type_id");


        // map TitleAka to title_akas
        modelBuilder.Entity<TitleAka>().ToTable("title_aka");
        modelBuilder.Entity<TitleAka>().Property(ta => ta.TitleId).HasColumnName("title_id");
        modelBuilder.Entity<TitleAka>().Property(ta => ta.Ordering).HasColumnName("ordering");
        modelBuilder.Entity<TitleAka>().Property(ta => ta.Region).HasColumnName("region");
        modelBuilder.Entity<TitleAka>().Property(ta => ta.Language).HasColumnName("language");
        modelBuilder.Entity<TitleAka>().Property(ta => ta.Description).HasColumnName("description");
        modelBuilder.Entity<TitleAka>().Property(ta => ta.IsOriginalTitle).HasColumnName("is_original_title");
        modelBuilder.Entity<TitleAka>().HasKey(ta => new { ta.TitleId, ta.Ordering });// composite PK

        // map TitleAka to Context
        modelBuilder.Entity<TitleAka>()
                            .HasOne(ta => ta.Context)
                            .WithMany()
                            .HasForeignKey("context");



        // map TitleAka to Title
        modelBuilder.Entity<TitleAka>()
                            .HasOne(ta => ta.Title)
                            .WithMany()
                            .HasForeignKey("title_id");


        // map Episode to episode
        modelBuilder.Entity<Episode>().ToTable("title_episode");
        modelBuilder.Entity<Episode>().Property(e => e.EpisodeId).HasColumnName("tconst");
        modelBuilder.Entity<Episode>().Property(e => e.SeasonNumber).HasColumnName("seasonnumber");
        modelBuilder.Entity<Episode>().Property(e => e.EpisodeNumber).HasColumnName("episodenumber");

        // map Episode to Title
        modelBuilder.Entity<Episode>()
                            .HasOne(e => e.Title)
                            .WithMany()
                            .HasForeignKey("parenttconst");

        // map Context to context
        modelBuilder.Entity<Context>().ToTable("context");
        modelBuilder.Entity<Context>().Property(c => c.Id).HasColumnName("context_id");



        // map Rating to rating_history
        modelBuilder.Entity<Rating>().ToTable("rating_history");
        modelBuilder.Entity<Rating>().Property(r => r.Id).HasColumnName("ratingid");
        modelBuilder.Entity<Rating>().Property(r => r.RatingValue).HasColumnName("rating");
        modelBuilder.Entity<Rating>().Property(r => r.RatingDate).HasColumnName("ratetime");

        // map Rating to Title and User
        modelBuilder.Entity<Rating>()
                            .HasOne(r => r.Title)
                            .WithMany(t => t.Ratings)
                            .HasForeignKey("title_id");
        modelBuilder.Entity<Rating>()
                            .HasOne(r => r.User)
                            .WithMany(u => u.RatedTitles)
                            .HasForeignKey("username");

        // map Search to search_history
        modelBuilder.Entity<Search>().ToTable("search_history");
        modelBuilder.Entity<Search>().Property(s => s.Id).HasColumnName("search_id");
        modelBuilder.Entity<Search>().Property(s => s.SearchTime).HasColumnName("timestamp");
        modelBuilder.Entity<Search>().Property(s => s.Query).HasColumnName("query");



        // map User to user
        modelBuilder.Entity<User>().ToTable("user");
        modelBuilder.Entity<User>().Property(u => u.UserName).HasColumnName("username");
        modelBuilder.Entity<User>().Property(u => u.FirstName).HasColumnName("first_name");
        modelBuilder.Entity<User>().Property(u => u.LastName).HasColumnName("last_name");
        modelBuilder.Entity<User>().Property(u => u.Email).HasColumnName("email");
        modelBuilder.Entity<User>().Property(u => u.Password).HasColumnName("password");
        modelBuilder.Entity<User>().HasKey(u => u.UserName); // define PK

        // map User with BookmarkedTitles. I havent created a class for the join table(title_bookmark) since it only contains the two FKs
        modelBuilder.Entity<User>()
                            .HasMany(u => u.BookmarkedTitles)
                            .WithMany()
                            .UsingEntity(j => j.ToTable("title_bookmark")
                            .HasData());

        // map User with BookmarkedPersons. I havent created a class for the join table(person_bookmark) since it only contains the two FKs
        modelBuilder.Entity<User>().HasMany(u => u.BookmarkedPersons)
                            .WithMany()
                            .UsingEntity(j => j.ToTable("person_bookmark")
                            .HasData());

        // map User with Search
        modelBuilder.Entity<User>()
                            .HasMany(u => u.SearchHistory)
                            .WithOne(s => s.User)
                            .HasForeignKey("username");
    }
}
